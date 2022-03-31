﻿using AutoMapper;
using CsvHelper.Configuration;
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
    [Route("/api/contact")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class ContactController : GenericController<Contact, ContactBaseResponse, ContactSingleResponse, ContactMultiResponse, ContactRequest>
    {
        public ContactController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration) : base(dbContext, mapper, csvConfiguration)
        {
        }

        protected override DbSet<Contact> GetDbSet()
        {
            return dbContext.Contacts;
        }
    }
}