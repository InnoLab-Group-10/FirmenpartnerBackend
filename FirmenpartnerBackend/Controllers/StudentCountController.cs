using AutoMapper;
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

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/studentcount")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class StudentCountController : GenericController<StudentCount, StudentCountBaseResponse, StudentCountSingleResponse, StudentCountMultiResponse, StudentCountRequest>
    {
        public StudentCountController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration, FileUploadConfig fileUploadConfig) : base(dbContext, mapper, csvConfiguration, fileUploadConfig)
        {
        }

        [HttpGet]
        [Route("company/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetForCompany([FromRoute] Guid id)
        {
            List<StudentCount> models = await GetDbSet().Where(n => n.CompanyId == id).ToListAsync();

            if (models.Count == 0) return Ok(new StudentCountMultiResponse()
            {
                Success = true,
                Results = new List<StudentCountBaseResponse>()
            });

            foreach (StudentCount model in models)
            {
                LoadRelated(dbContext.Entry(model));
            }

            List<StudentCountBaseResponse> results = models.Select(e => mapper.Map<StudentCountBaseResponse>(e)).ToList();
            return Ok(new StudentCountMultiResponse()
            {
                Success = true,
                Results = results
            });
        }

        protected override DbSet<StudentCount> GetDbSet()
        {
            return dbContext.StudentCounts;
        }
    }
}
