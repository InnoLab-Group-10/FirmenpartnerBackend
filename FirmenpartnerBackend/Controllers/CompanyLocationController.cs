using AutoMapper;
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

    [Route("/api/companylocation")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class CompanyLocationController : GenericController<CompanyLocation, CompanyLocationBaseResponse, CompanyLocationSingleResponse, CompanyLocationMultiResponse, CompanyLocationRequest>
    {
        public CompanyLocationController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration) : base(dbContext, mapper, csvConfiguration)
        {
        }

        protected override DbSet<CompanyLocation> GetDbSet()
        {
            return dbContext.CompanyLocations;
        }

        protected override void LoadRelated(EntityEntry<CompanyLocation> entry)
        {
            entry.Reference(e => e.Company).Load();
        }
    }
}
