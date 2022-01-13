﻿using AutoMapper;
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
    
    [Route("/api/companylocation")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class CompanyLocationController : GenericController<CompanyLocation, CompanyLocationBaseResponse, CompanyLocationSingleResponse, CompanyLocationMultiResponse, CompanyLocationRequest>
    {
        public CompanyLocationController(ApiDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        protected override DbSet<CompanyLocation> GetDbSet()
        {
            return dbContext.CompanyLocations;

        }
    }
}
