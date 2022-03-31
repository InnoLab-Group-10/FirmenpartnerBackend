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

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/company")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class CompanyController : GenericController<Company, CompanyBaseResponse, CompanySingleResponse, CompanyMultiResponse, CompanyRequest>
    {
        public CompanyController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration) : base(dbContext, mapper, csvConfiguration)
        {
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public override async Task<IActionResult> GetAll()
        {
            List<FullCompanyInfoResponseEntry> responses = new List<FullCompanyInfoResponseEntry>();

            List<Company> companies = await dbContext.Companies.ToListAsync();

            Dictionary<Guid, IGrouping<Guid, CompanyLocation>>? locations = dbContext.CompanyLocations.AsEnumerable().GroupBy(loc => loc.CompanyId).ToDictionary(x => x.Key);

            Dictionary<Guid, IGrouping<Guid, GetAllFullContactsEntry>>? contacts = dbContext.CompanyAssignments
                .Join(dbContext.People, assignment => assignment.PersonId, person => person.Id, (assignment, person) => new GetAllFullContactsEntry(
                    mapper.Map<CompanyAssignmentBaseResponse>(assignment),
                    mapper.Map<PersonBaseResponse>(person)
                ))
                .AsEnumerable()
                .GroupBy(c => c.Assignment.CompanyId)
                .ToDictionary(x => x.Key);

            foreach (Company company in companies)
            {
                List<CompanyGetAllLocationBaseResponse>? locationResponses = null;
                List<CompanyGetAllContactAssignmentBaseResponse>? contactResponses = null;

                IGrouping<Guid, CompanyLocation> locGroups;
                if (locations.TryGetValue(company.Id, out locGroups))
                {
                    locationResponses = locGroups.Select(loc => mapper.Map<CompanyGetAllLocationBaseResponse>(loc)).ToList();
                }

                IGrouping<Guid, GetAllFullContactsEntry> assignGroups;
                if (contacts.TryGetValue(company.Id, out assignGroups))
                {
                    contactResponses = assignGroups.Select(a =>
                    {
                        CompanyGetAllContactAssignmentBaseResponse r = mapper.Map<CompanyGetAllContactAssignmentBaseResponse>(a.Person);
                        r.From = a.Assignment.From;
                        r.To = a.Assignment.To;

                        return r;
                    }).ToList();
                }

                responses.Add(new FullCompanyInfoResponseEntry()
                {
                    Company = mapper.Map<CompanyBaseResponse>(company),
                    Locations = locationResponses == null ? new List<CompanyGetAllLocationBaseResponse>() : locationResponses,
                    Contacts = contactResponses == null ? new List<CompanyGetAllContactAssignmentBaseResponse>() : contactResponses
                });
            }

            return Ok(new FullCompanyInfoResponse()
            {
                Success = true,
                Results = responses
            });
        }

        protected override DbSet<Company> GetDbSet()
        {
            return dbContext.Companies;
        }

        private record GetAllFullContactsEntry(CompanyAssignmentBaseResponse Assignment, PersonBaseResponse Person);
    }
}
