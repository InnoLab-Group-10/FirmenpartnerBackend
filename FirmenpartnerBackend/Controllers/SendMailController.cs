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
using System.Text;

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

        [HttpGet]
        [Route("preview/{templateId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PreviewMailHtml([FromRoute] Guid templateId)
        {
            if (ModelState.IsValid)
            {
                MailTemplate? template = await dbContext.MailTemplates.FindAsync(templateId);
                if (template == null)
                {
                    return NotFound(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid mail template ID." }
                    });
                }

                List<(Guid guid, string name)> attachments = new();

                try
                {
                    string body = mailService.GetMailHtml(template.Content, attachments);
                    return Ok(body);
                }
                catch (Exception e)
                {
                    return UnprocessableEntity(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { $"Failed to preview E-mail: {e.Message}" }
                    });
                }
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> NewSend([FromBody] SendMailRequest request)
        {
            if (ModelState.IsValid)
            {
                List<MailRecipient> recipients = new List<MailRecipient>();

                if (request.MailingList == "all")
                {
                    IQueryable<Person> contacts = dbContext.CompanyAssignments.Where(a => a.To == null).Select(a => a.Person);
                    IEnumerable<MailRecipient> adresses = (await contacts.Where(p => p.Email != null).ToListAsync()).Select(p => mapper.Map<MailRecipient>(p));
                    recipients.AddRange(adresses);
                }
                else if (request.MailingList == "active")
                {
                    IQueryable<Person> contacts = dbContext.CompanyAssignments.Where(a => a.To == null && a.Company.ContractSigned).Select(a => a.Person);
                    IEnumerable<MailRecipient> adresses = (await contacts.Where(p => p.Email != null).ToListAsync()).Select(p => mapper.Map<MailRecipient>(p));
                    recipients.AddRange(adresses);
                }
                else if (request.MailingList == "inactive")
                {
                    IQueryable<Person> contacts = dbContext.CompanyAssignments.Where(a => a.To == null && !a.Company.ContractSigned).Select(a => a.Person);
                    IEnumerable<MailRecipient> adresses = (await contacts.Where(p => p.Email != null).ToListAsync()).Select(p => mapper.Map<MailRecipient>(p));
                    recipients.AddRange(adresses);
                }
                else if (request.MailingList != null) // Try to use it as a mailing list GUID
                {
                    try
                    {
                        MailingList? mailingList = await dbContext.MailingLists.FindAsync(new Guid(request.MailingList));

                        if (mailingList == null)
                        {
                            return NotFound(new SendMailSingleResponse()
                            {
                                Success = false,
                                Errors = new List<string>() { "Invalid mailing list ID." }
                            });
                        }

                        await dbContext.Entry(mailingList).Collection(e => e.Entries).LoadAsync();

                        var adresses = mailingList.Entries.Select(x => mapper.Map<MailRecipient>(x));
                        recipients.AddRange(adresses);
                    }
                    catch (FormatException e)
                    {
                        return BadRequest(new SendMailSingleResponse()
                        {
                            Success = false,
                            Errors = new List<string>() { $"Invalid request: {request.MailingList} is not a valid value. Allowed values: 'all', 'active', 'inactive', mailing list GUIDs" }
                        });
                    }
                }

                if (request.AdditionalRecipients != null)
                {
                    foreach (string adress in request.AdditionalRecipients)
                    {
                        recipients.Add(new MailRecipient(null, "", "", null, adress));
                    }
                }

                if (recipients.Count == 0)
                {
                    return BadRequest(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { $"Invalid request: Cannot send mail without recipients." }
                    });
                }

                if (request.Template == null && request.AdditionalText == null)
                {
                    return BadRequest(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { $"Invalid request: Cannot send mail without a body. Set at least the template or additionalText property." }
                    });
                }

                var response = await SendMails(request, recipients);
                return response;
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
            StringBuilder stringBuilder = new StringBuilder();

            if (request.Template != null)
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
                else
                {
                    stringBuilder.Append(template.Content);
                }
            }

            if (request.AdditionalText != null && request.AdditionalText.Length > 0)
            {
                stringBuilder.Append(request.AdditionalText);
            }

            List<(Guid guid, string name)> attachments = new();
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
                        attachments.Add((guid, model.Name));
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
                string body = mailService.GetMailHtml(stringBuilder.ToString(), attachments);
                mailService.SendMail(request.Subject, body, recipients);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(new SendMailSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { $"Failed to send E-mail: {e.Message}" }
                });
            }

            return Ok(new SendMailSingleResponse()
            {
                Success = true
            });
        }
    }
}
