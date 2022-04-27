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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/companyassignment")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class CompanyAssignmentController : GenericController<CompanyAssignment, CompanyAssignmentBaseResponse, CompanyAssignmentSingleResponse, CompanyAssignmentMultiResponse, CompanyAssignmentRequest>
    {
        public CompanyAssignmentController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration, FileUploadConfig fileUploadConfig) : base(dbContext, mapper, csvConfiguration, fileUploadConfig)
        {
        }

        protected override DbSet<CompanyAssignment> GetDbSet()
        {
            return dbContext.CompanyAssignments;
        }

        protected override void LoadRelated(EntityEntry<CompanyAssignment> entry)
        {
            //entry.Reference(e => e.Company).Load();
            //entry.Reference(e => e.Person).Load();
        }
    }
}
