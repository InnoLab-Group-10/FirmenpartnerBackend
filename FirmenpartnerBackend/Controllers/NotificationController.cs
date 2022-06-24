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

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/notification")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class NotificationController : GenericController<Notification, NotificationBaseResponse, NotificationSingleResponse, NotificationMultiResponse, NotificationRequest>
    {
        public NotificationController(ApiDbContext dbContext, IMapper mapper, CsvConfiguration csvConfiguration, FileUploadConfig fileUploadConfig) : base(dbContext, mapper, csvConfiguration, fileUploadConfig)
        {
        }

        [HttpGet]
        [Route("user/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetForUser([FromRoute] Guid id)
        {
            List<Notification> models = await GetDbSet().Where(n => n.RecipientId == id.ToString()).ToListAsync();

            if (models.Count == 0) return NoContent();

            foreach (Notification model in models)
            {
                LoadRelated(dbContext.Entry(model));
            }

            List<NotificationBaseResponse> results = models.Select(e => mapper.Map<NotificationBaseResponse>(e)).ToList();
            return Ok(new NotificationMultiResponse()
            {
                Success = true,
                Results = results
            });
        }

        protected override DbSet<Notification> GetDbSet()
        {
            return dbContext.Notifications;
        }
    }
}
