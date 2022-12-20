using AutoMapper;
using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using FirmenpartnerBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/sendmail")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class SendMailController : ControllerBase
    {
        private readonly ApiDbContext dbContext;
        private readonly IMapper mapper;
        private readonly FileUploadConfig fileUploadConfig;
        private readonly ITemplateMailService mailService;

        public SendMailController(ApiDbContext dbContext, IMapper mapper, FileUploadConfig fileUploadConfig, ITemplateMailService mailService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.fileUploadConfig = fileUploadConfig;
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("{list}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToMailingList([FromBody] SendMailRequest request, [FromRoute] Guid list)
        {
            if (ModelState.IsValid)
            {
                MailingList? mailingList = await dbContext.MailingLists.FindAsync(list);

                if (mailingList == null)
                {
                    return NotFound(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid mailing list ID." }
                    });
                }

                var adresses = mailingList.Entries.Select(x => new MailRecipient(x.Note, x.Mail));
                var response = SendMails(request, adresses);
                return Ok(response);
            }
            else
            {
                return BadRequest(new SendMailSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid request." }
                });
            }
        }

        [HttpPost]
        [Route("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToAllCurrentContacts([FromBody] SendMailRequest request)
        {
            if (ModelState.IsValid)
            {
                IQueryable<Person> contacts = dbContext.CompanyAssignments.Where(a => a.To == null).Select(a => a.Person);
                IEnumerable<MailRecipient> adresses = (await contacts.Where(p => p.Email != null).ToListAsync()).Select(p => new MailRecipient($"{p.Prefix} {p.FirstName} {p.LastName} {p.Suffix}", p.Email));
                var response = SendMails(request, adresses);
                return Ok(response);
            }
            else
            {
                return BadRequest(new SendMailSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid request." }
                });
            }
        }

        [HttpPost]
        [Route("active")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToAllCurrentContactsInActiveCompany([FromBody] SendMailRequest request)
        {
            if (ModelState.IsValid)
            {
                IQueryable<Person> contacts = dbContext.CompanyAssignments.Where(a => a.To == null && a.Company.ContractSigned).Select(a => a.Person);
                IEnumerable<MailRecipient> adresses = (await contacts.Where(p => p.Email != null).ToListAsync()).Select(p => new MailRecipient($"{p.Prefix} {p.FirstName} {p.LastName} {p.Suffix}", p.Email));
                var response = SendMails(request, adresses);
                return Ok(response);
            }
            else
            {
                return BadRequest(new SendMailSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid request." }
                });
            }
        }

        [HttpPost]
        [Route("inactive")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToAllCurrentContactsInInactiveCompany([FromBody] SendMailRequest request)
        {
            if (ModelState.IsValid)
            {
                IQueryable<Person> contacts = dbContext.CompanyAssignments.Where(a => a.To == null && !a.Company.ContractSigned).Select(a => a.Person);
                IEnumerable<MailRecipient> adresses = (await contacts.Where(p => p.Email != null).ToListAsync()).Select(p => new MailRecipient($"{p.Prefix} {p.FirstName} {p.LastName} {p.Suffix}", p.Email));
                var response = SendMails(request, adresses);
                return Ok(response);
            }
            else
            {
                return BadRequest(new SendMailSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid request." }
                });
            }
        }

        private async Task<IActionResult> SendMails(SendMailRequest request, IEnumerable<MailRecipient> recipients)
        {
            MailTemplate? template = await dbContext.MailTemplates.FindAsync(request.Template);
            if (template == null)
            {
                return NotFound(new SendMailSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid mail template ID." }
                });
            }

            List<(string path, string name)> attachments = new();
            foreach (Guid guid in request.Attachments)
            {
                FileEntry? model = await dbContext.FileEntries.FindAsync(guid);

                if (model == null)
                {
                    return NotFound(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { $"Failed to attach file {guid}: No file with the given ID exists." }
                    });
                }
                else
                {
                    string filePath = Path.Combine(fileUploadConfig.TargetPath, model.Id.ToString());
                    if (System.IO.File.Exists(filePath))
                    {
                        attachments.Add((filePath, model.Name));
                    }
                    else
                    {
                        return NotFound(new SendMailSingleResponse()
                        {
                            Success = false,
                            Errors = new List<string>() { $"Failed to attach file {guid}: No file with the given ID exists." }
                        });
                    }
                }
            }

            try
            {
                mailService.SendMail(request.Subject, template, attachments, recipients);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(new SendMailSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { $"Failed to send E-mail: {e.Message}" }
                });
            }

            return null;
        }
    }
}
