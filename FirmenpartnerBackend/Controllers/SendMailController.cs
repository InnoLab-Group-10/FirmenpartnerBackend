using AutoMapper;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public SendMailController(ApiDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToList([FromBody] SendMailRequest request)
        {
            return Ok(new SendMailSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Not implemented yet." }
            });

            if (ModelState.IsValid)
            {
                MailingList? mailingList = await dbContext.MailingLists.FindAsync(request.MailingList);
                MailTemplate? template = await dbContext.MailTemplates.FindAsync(request.Template);

                if (mailingList == null || template == null)
                {
                    return NotFound(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Invalid mailing list or mail template ID." }
                    });
                }

                try
                {
                    SentMail mail = null; // Call service to generate and send mail
                    // Save generated mail to the DB - still need to add DB table for this to ApiDbContext as well

                    SendMailSingleResponse response = mapper.Map<SendMailSingleResponse>(mail);
                    response.Success = true;
                    return Ok(response);
                }
                catch (Exception e)
                {
                    return UnprocessableEntity(new SendMailSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { e.Message }
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
    }
}
