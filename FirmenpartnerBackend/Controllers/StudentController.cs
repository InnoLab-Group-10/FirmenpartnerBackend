using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/student")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class StudentController : GenericController<Student, StudentBaseResponse, StudentSingleResponse, StudentMultiResponse, StudentRequest>
    {
        public StudentController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration) : base(dbContext, mapper, csvConfiguration)
        {
        }

        [HttpGet]
        [Route("csv")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public override async Task<IActionResult> GetAllCsv()
        {
            List<StudentBaseCsvResponse> results = await GetDbSet().Select(e => mapper.Map<StudentBaseCsvResponse>(e)).ToListAsync();
            string csvString;

            using (var writer = new StringWriter())
            using (var csvWriter = new CsvWriter(writer, csvConfiguration))
            {
                csvWriter.WriteRecords(results);
                csvString = writer.ToString();
            }

            return Ok(new CsvResponse()
            {
                Success = true,
                Csv = csvString
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
