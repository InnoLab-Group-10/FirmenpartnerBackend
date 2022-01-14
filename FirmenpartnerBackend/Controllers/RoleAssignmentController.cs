using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/role")]
    [ApiController]
    public class RoleAssignmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleAssignmentController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(GetAllRolesResponse), 200)]
        public async Task<IActionResult> GetRoles()
        {
            List<string> roles = await Task.Run(() => roleManager.Roles.Where(r => r.Name != ApplicationRoles.ROOT).Select(r => r.Name).ToList());
            return Ok(new GetAllRolesResponse()
            {
                Success = true,
                Roles = roles
            });
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(AssignRoleResponse), 200)]
        [ProducesResponseType(typeof(AssignRoleResponse), 400)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByIdAsync(request.UserId);
                IdentityRole? role = await roleManager.FindByNameAsync(request.Role);

                if (user == null)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No user with the provided ID exists." }
                    });
                }


                if (role == null)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No role with the provided ID exists." }
                    });
                }

                if (role.Name == ApplicationRoles.ROOT)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Cannot assign root role." }
                    });
                }

                bool alreadyAssigned = await userManager.IsInRoleAsync(user, role.Name);
                if (alreadyAssigned)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "The specified role is already assigned to the specified user." }
                    });
                }

                IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList()
                    });
                }

                return Ok(new AssignRoleResponse()
                {
                    Success = true
                });
            }

            return BadRequest(new AssignRoleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(AssignRoleResponse), 200)]
        [ProducesResponseType(typeof(AssignRoleResponse), 400)]
        public async Task<IActionResult> UnassignRole([FromBody] AssignRoleRequest request)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByIdAsync(request.UserId);
                IdentityRole? role = await roleManager.FindByNameAsync(request.Role);

                if (user == null)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No user with the provided ID exists." }
                    });
                }


                if (role == null)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No role with the provided ID exists." }
                    });
                }

                if (role.Name == ApplicationRoles.ROOT)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Cannot unassign root role." }
                    });
                }

                bool alreadyAssigned = await userManager.IsInRoleAsync(user, role.Name);
                if (!alreadyAssigned)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "The specified role is not assigned to the specified user." }
                    });
                }

                IdentityResult result = await userManager.RemoveFromRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    return BadRequest(new AssignRoleResponse()
                    {
                        Success = false,
                        Errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList()
                    });
                }

                return Ok(new AssignRoleResponse()
                {
                    Success = true
                });
            }

            return BadRequest(new AssignRoleResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }
    }
}
