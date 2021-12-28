using AutoMapper;
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
    [Route("/api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class CompanyController : GenericController<Company, CompanyBaseResponse, CompanySingleResponse, CompanyMultiResponse, CompanyRequest>
    {
        public CompanyController(ApiDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        protected override DbSet<Company> GetDbSet()
        {
            return dbContext.Companies;

        }
    }
}
