using AutoMapper;
using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirmenpartnerBackend.Controllers
{
    public abstract class GenericController<TModel, TBaseResponse, TSingleResponse, TMultiResponse, TCreateRequest> : ControllerBase where TModel : BaseModel, new()
                                                                                                                        where TBaseResponse : BaseSingleResponse, new()
                                                                                                                        where TSingleResponse : BaseSingleResponse, IBaseResponse, new()
                                                                                                                        where TMultiResponse : BaseMultiResponse<TBaseResponse>, IBaseResponse, new()

    {
        protected ApiDbContext dbContext;
        protected IMapper mapper;

        public GenericController(ApiDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        protected abstract DbSet<TModel> GetDbSet();

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Get([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                TModel? model = await GetDbSet().FindAsync(id);

                if (model == null)
                {
                    return NotFound(new TSingleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    TSingleResponse response = mapper.Map<TSingleResponse>(model);
                    response.Success = true;

                    return Ok(response);
                }

            }

            return BadRequest(new TSingleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> GetAll()
        {
            List<TBaseResponse> results = await GetDbSet().Select(e => mapper.Map<TBaseResponse>(e)).ToListAsync();
            return Ok(new TMultiResponse()
            {
                Success = true,
                Results = results
            });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Post([FromBody] TCreateRequest user)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Put([FromRoute] Guid id)
        {
            return Ok();
        }
    }
}
