using AutoMapper;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using FirmenpartnerBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/mailsettings")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(401)]
    public class MailSettingsController : ControllerBase
    {
        private readonly ApiDbContext dbContext;
        private readonly IMailSettingsService mailSettingsService;
        private readonly IMapper mapper;

        public MailSettingsController(ApiDbContext dbContext, IMailSettingsService mailSettingsService, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mailSettingsService = mailSettingsService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("{key}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetSetting([FromRoute] string key)
        {
            try
            {
                MailSetting setting = await mailSettingsService.GetSetting(key);

                var response = mapper.Map<MailSettingSingleResponse>(setting);
                response.Success = true;

                return Ok(response);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new MailSettingSingleResponse()
                {
                    Success = false,
                    Errors = new() { e.Message }
                });
            }
        }

        [HttpPost]
        [Route("{key}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SetSetting([FromRoute] string key, [FromBody] MailSettingRequest body)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailSetting result = await mailSettingsService.SetSetting(key, body.Value);

                    var response = mapper.Map<MailSettingSingleResponse>(result);
                    response.Success = true;

                    return Ok(response);
                }
                catch (KeyNotFoundException e)
                {
                    return NotFound(new MailSettingSingleResponse()
                    {
                        Success = false,
                        Errors = new() { e.Message }
                    });
                }
            }
            else
            {
                return BadRequest(new MailSettingSingleResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid request." }
                });
            }
        }
    }
}
