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
    [Route("/api/student")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class StudentController : GenericController<Student, StudentBaseResponse, StudentSingleResponse, StudentMultiResponse, StudentRequest>
    {
        public StudentController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration, FileUploadConfig fileUploadConfig) : base(dbContext, mapper, csvConfiguration, fileUploadConfig)
        {
        }

        [HttpGet]
        [Route("csv")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public override async Task<IActionResult> GetAllCsv()
        {
            List<StudentCsvBaseResponse> results = await GetDbSet().Select(e => mapper.Map<StudentCsvBaseResponse>(e)).ToListAsync();
            string csvString;

            using (var writer = new StringWriter())
            using (var csvWriter = new CsvWriter(writer, csvConfiguration))
            {
                csvWriter.WriteRecords(results);
                csvString = writer.ToString();
            }

            return File(Encoding.UTF8.GetBytes(csvString), "text/csv", fileDownloadName: $"{nameof(Student)}_Export.csv");
        }

        [HttpPost]
        [Route("csv")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public override async Task<IActionResult> ImportFromCsv(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                List<StudentCsvBaseResponse> csvEntries = new List<StudentCsvBaseResponse>();


                if (file == null || file.Length > fileUploadConfig.MaxSize) return BadRequest(new StudentCsvMultiResponse()
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
                        IEnumerable<StudentCsvBaseResponse> rows = csv.GetRecords<StudentCsvBaseResponse>();
                        foreach (StudentCsvBaseResponse row in rows)
                        {
                            csvEntries.Add(row);
                        }
                    }
                }
                catch
                {
                    return BadRequest(new StudentCsvMultiResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid CSV." }
                    });
                }

                List<StudentCsvBaseResponse> updated = new List<StudentCsvBaseResponse>();

                foreach (StudentCsvBaseResponse csvEntry in csvEntries)
                {
                    Student? trackedModel = null;
                    if (csvEntry.Id.HasValue)
                    {
                        Guid guid = csvEntry.Id.Value;
                        trackedModel = await GetDbSet().FindAsync(guid);
                    }

                    if (trackedModel != null) // Entry exists already, update if needed
                    {
                        Student modifiedModel = mapper.Map<Student>(csvEntry);
                        modifiedModel.Id = csvEntry.Id.Value;

                        dbContext.Entry(trackedModel).CurrentValues.SetValues(modifiedModel);

                        updated.Add(mapper.Map<StudentCsvBaseResponse>(modifiedModel));
                    }
                    else // Entry doesn't exist, create it
                    {
                        Student model = mapper.Map<Student>(csvEntry);
                        model.Id = Guid.NewGuid();

                        await GetDbSet().AddAsync(model);
                        updated.Add(mapper.Map<StudentCsvBaseResponse>(model));
                    }
                }

                await dbContext.SaveChangesAsync();

                return Ok(new StudentCsvMultiResponse()
                {
                    Success = true,
                    Results = updated
                });
            }

            return BadRequest(new StudentCsvMultiResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        protected override DbSet<Student> GetDbSet()
        {
            return dbContext.Students;
        }

        protected override void LoadRelated(EntityEntry<Student> entry)
        {
            entry.Reference(e => e.Program).Load();
        }
    }
}
