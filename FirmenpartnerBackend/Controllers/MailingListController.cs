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
    [Route("/api/mailinglist")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class MailingListController : GenericController<MailingList, MailingListBaseResponse, MailingListSingleResponse, MailingListMultiResponse, MailingListRequest>
    {
        public MailingListController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration, FileUploadConfig fileUploadConfig) : base(dbContext, mapper, csvConfiguration, fileUploadConfig)
        {
        }

        [HttpPost]
        [Route("{listId}/add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> AddToList([FromRoute] Guid listId, [FromBody] Guid personId)
        {
            if (ModelState.IsValid)
            {
                MailingList? list = await GetDbSet().FindAsync(listId);
                Person? person = await dbContext.People.FindAsync(personId);

                if (list == null || person == null)
                {
                    return NotFound(new MailingListSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    LoadRelated(dbContext.Entry(list));

                    if (list.Recipients == null) list.Recipients = new List<Person>();

                    if (!list.Recipients.Contains(person)) list.Recipients.Add(person);
                    await dbContext.SaveChangesAsync();

                    return Ok();
                }
            }

            return BadRequest(new MailingListSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpPost]
        [Route("{listId}/remove")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> RemoveFromList([FromRoute] Guid listId, [FromBody] Guid personId)
        {
            if (ModelState.IsValid)
            {
                MailingList? list = await GetDbSet().FindAsync(listId);
                Person? person = await dbContext.People.FindAsync(personId);

                if (list == null || person == null)
                {
                    return NotFound(new MailingListSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    LoadRelated(dbContext.Entry(list));

                    if (list.Recipients == null) list.Recipients = new List<Person>();

                    list.Recipients.Remove(person);
                    await dbContext.SaveChangesAsync();

                    return Ok();
                }
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
            entry.Collection(e => e.Recipients).Load();
        }
    }
}
