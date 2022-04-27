using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;

namespace FirmenpartnerBackend.Controllers
{
    public abstract class GenericController<TModel, TBaseResponse, TSingleResponse, TMultiResponse, TRequest> : ControllerBase where TModel : BaseModel
                                                                                                                        where TBaseResponse : ISingleResponse, new()
                                                                                                                        where TSingleResponse : ISingleResponse, IResponse, new()
                                                                                                                        where TMultiResponse : IMultiResponse<TBaseResponse>, IResponse, new()
    {
        protected ApiDbContext dbContext;
        protected IMapper mapper;
        protected CsvConfiguration csvConfiguration;
        protected FileUploadConfig fileUploadConfig;

        protected GenericController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration, FileUploadConfig fileUploadConfig)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.csvConfiguration = csvConfiguration;
            this.fileUploadConfig = fileUploadConfig;
        }

        protected abstract DbSet<TModel> GetDbSet();
        protected virtual void LoadRelated(EntityEntry<TModel> entry) { }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Get([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                TModel? model = await GetDbSet().FindAsync(id);

                if (model == null)
                {
                    return NotFound(new TSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    LoadRelated(dbContext.Entry(model));
                    TSingleResponse response = mapper.Map<TSingleResponse>(model);
                    response.Success = true;

                    return Ok(response);
                }

            }

            return BadRequest(new TSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetAll()
        {
            List<TModel> models = await GetDbSet().ToListAsync();

            foreach (TModel model in models)
            {
                LoadRelated(dbContext.Entry(model));
            }

            List<TBaseResponse> results = models.Select(e => mapper.Map<TBaseResponse>(e)).ToList();
            return Ok(new TMultiResponse()
            {
                Success = true,
                Results = results
            });
        }

        [HttpGet]
        [Route("csv")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetAllCsv()
        {
            List<TBaseResponse> results = await GetDbSet().Select(e => mapper.Map<TBaseResponse>(e)).ToListAsync();
            string csvString;

            using (var writer = new StringWriter())
            using (var csvWriter = new CsvWriter(writer, csvConfiguration))
            {
                csvWriter.WriteRecords(results);
                csvString = writer.ToString();
            }

            return File(Encoding.UTF8.GetBytes(csvString), "text/csv", fileDownloadName: $"{typeof(TModel).Name}_Export.csv");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Post([FromBody] TRequest request)
        {
            if (ModelState.IsValid)
            {
                TModel model = mapper.Map<TModel>(request);
                model.Id = Guid.NewGuid();

                await GetDbSet().AddAsync(model);
                await dbContext.SaveChangesAsync();

                TSingleResponse response = mapper.Map<TSingleResponse>(model);
                response.Success = true;

                return Ok(response);
            }

            return BadRequest(new TSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpPost]
        [Route("csv")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> ImportFromCsv(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                List<TBaseResponse> csvEntries = new List<TBaseResponse>();


                if (file == null || file.Length > fileUploadConfig.MaxSize) return BadRequest(new TMultiResponse()
                {
                    Success = false,
                    Errors = new List<string>() { $"Invalid file size. Uploaded files can be at most {fileUploadConfig.MaxSize} bytes." }
                });

                try
                {
                    using (var stream = file.OpenReadStream())
                    using (var reader = new StreamReader(stream))
                    using (var csv = new CsvReader(reader, csvConfiguration))
                    {
                        IEnumerable<TBaseResponse> rows = csv.GetRecords<TBaseResponse>();
                        foreach (TBaseResponse row in rows)
                        {
                            csvEntries.Add(row);
                        }
                    }
                }
                catch
                {
                    return BadRequest(new TMultiResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid CSV." }
                    });
                }

                List<TBaseResponse> updated = new List<TBaseResponse>();

                foreach (TBaseResponse csvEntry in csvEntries)
                {
                    TModel? trackedModel = null;
                    if (csvEntry.Id.HasValue)
                    {
                        Guid guid = csvEntry.Id.Value;
                        trackedModel = await GetDbSet().FindAsync(guid);
                    }

                    if (trackedModel != null) // Entry exists already, update if needed
                    {
                        TModel modifiedModel = mapper.Map<TModel>(csvEntry);
                        modifiedModel.Id = csvEntry.Id.Value;

                        dbContext.Entry(trackedModel).CurrentValues.SetValues(modifiedModel);

                        updated.Add(mapper.Map<TBaseResponse>(modifiedModel));
                    }
                    else // Entry doesn't exist, create it
                    {
                        TModel model = mapper.Map<TModel>(csvEntry);
                        model.Id = Guid.NewGuid();

                        await GetDbSet().AddAsync(model);
                        updated.Add(mapper.Map<TBaseResponse>(model));
                    }
                }

                await dbContext.SaveChangesAsync();

                return Ok(new TMultiResponse()
                {
                    Success = true,
                    Results = updated
                });
            }

            return BadRequest(new TMultiResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                TModel? model = await GetDbSet().FindAsync(id);

                if (model == null)
                {
                    return NotFound(new DeleteResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    GetDbSet().Remove(model);
                    await dbContext.SaveChangesAsync();

                    return Ok(new DeleteResponse()
                    {
                        Success = true
                    });
                }

            }

            return BadRequest(new DeleteResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Put([FromBody] TRequest request, [FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                TModel? trackedModel = await GetDbSet().FindAsync(id);

                if (trackedModel == null)
                {
                    return NotFound(new TSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    TModel modifiedModel = mapper.Map<TModel>(request);
                    modifiedModel.Id = id;

                    dbContext.Entry(trackedModel).CurrentValues.SetValues(modifiedModel);
                    await dbContext.SaveChangesAsync();

                    TSingleResponse response = mapper.Map<TSingleResponse>(modifiedModel);
                    response.Success = true;

                    return Ok(response);
                }

            }

            return BadRequest(new TSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }
    }
}
