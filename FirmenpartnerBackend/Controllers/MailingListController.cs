using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/mailinglist")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class MailingListController : GenericController<MailingList, MailingListBaseResponse, MailingListSingleResponse, MailingListMultiResponse, MailingListRequest>
    {
        public MailingListController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration, FileUploadConfig fileUploadConfig) : base(dbContext, mapper, csvConfiguration, fileUploadConfig)
        {
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public override async Task<IActionResult> Put([FromBody] MailingListRequest request, [FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                MailingList? trackedModel = await GetDbSet().FindAsync(id);

                if (trackedModel == null)
                {
                    return NotFound(new MailingListSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    MailingList modifiedModel = mapper.Map<MailingList>(request);
                    modifiedModel.Id = id;

                    // Clear list of existing entries first...
                    dbContext.Entry(trackedModel).Collection(m => m.Entries).Load();
                    dbContext.MailingListsEntries.RemoveRange(trackedModel.Entries);

                    // ...then set the new values - not super efficient, but I couldn't get it to work any other way
                    foreach (MailingListEntry entry in modifiedModel.Entries)
                    {
                        entry.Id = Guid.NewGuid();
                        dbContext.MailingListsEntries.Add(entry);
                    }

                    dbContext.Entry(trackedModel).CurrentValues.SetValues(modifiedModel);
                    trackedModel.Entries = modifiedModel.Entries;

                    await dbContext.SaveChangesAsync();

                    MailingListSingleResponse response = mapper.Map<MailingListSingleResponse>(modifiedModel);
                    response.Success = true;

                    return Ok(response);
                }
            }

            return BadRequest(new MailingListSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpGet]
        [Route("csv/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetCsvForList([FromRoute] Guid id)
        {
            MailingList targetList;

            try
            {
                targetList = await GetDbSet().Where(list => list.Id == id).SingleAsync();
            }
            catch (Exception)
            {
                return NotFound(new MailingListSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "No entity with the given ID exists." }
                });
            }

            LoadRelated(dbContext.Entry(targetList));

            IEnumerable<MailingListEntryResponse> results = targetList.Entries.Select(e => mapper.Map<MailingListEntryResponse>(e));
            string csvString;

            using (var writer = new StringWriter())
            using (var csvWriter = new CsvWriter(writer, csvConfiguration))
            {
                csvWriter.WriteRecords(results);
                csvString = writer.ToString();
            }

            return File(Encoding.UTF8.GetBytes(csvString), "text/csv", fileDownloadName: $"{typeof(MailingListEntry).Name}_Export.csv");
        }

        [HttpPost]
        [Route("csv/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> ImportFromCsvForList(IFormFile file, [FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                MailingList targetList;

                try
                {
                    targetList = await GetDbSet().Where(list => list.Id == id).SingleAsync();
                }
                catch (Exception)
                {
                    return NotFound(new MailingListSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }

                if (file == null || file.Length > fileUploadConfig.MaxSize) return BadRequest(new MailingListSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { $"Invalid file size. Uploaded files can be at most {fileUploadConfig.MaxSize} bytes." }
                });

                List<MailingListEntryResponse> csvEntries = new List<MailingListEntryResponse>();

                try
                {
                    using (var stream = file.OpenReadStream())
                    using (var reader = new StreamReader(stream))
                    using (var csv = new CsvReader(reader, csvConfiguration))
                    {
                        IEnumerable<MailingListEntryResponse> rows = csv.GetRecords<MailingListEntryResponse>();
                        foreach (MailingListEntryResponse row in rows)
                        {
                            csvEntries.Add(row);
                        }
                    }
                }
                catch
                {
                    return BadRequest(new MailingListSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid CSV." }
                    });
                }

                targetList.Entries.Clear();
                targetList.Entries = csvEntries.Select(e => mapper.Map<MailingListEntry>(e)).ToList();

                await dbContext.SaveChangesAsync();

                return Ok(new MailingListSingleResponse()
                {
                    Success = true,
                    Id = targetList.Id,
                    Name = targetList.Name,
                    Entries = targetList.Entries.Select(e => mapper.Map<MailingListEntryResponse>(e)).ToList()
                });
            }

            return BadRequest(new MailingListSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        protected override DbSet<MailingList> GetDbSet()
        {
            return dbContext.MailingLists;
        }

        protected override void LoadRelated(EntityEntry<MailingList> entry)
        {
            entry.Collection(e => e.Entries).Load();
        }
    }
}
