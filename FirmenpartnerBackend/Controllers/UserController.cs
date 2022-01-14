using AutoMapper;
using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using FirmenpartnerBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthTokenService authTokenService;
        private readonly IResetPasswordService resetPasswordService;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager, IAuthTokenService authTokenService, IResetPasswordService resetPasswordService, IMapper mapper)
        {
            this.userManager = userManager;
            this.authTokenService = authTokenService;
            this.resetPasswordService = resetPasswordService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(GetAllUsersResponse), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            List<string> users = userManager.Users.Select(u => u.Id).ToList();
            return Ok(new GetAllUsersResponse()
            {
                Success = true,
                Users = users
            });
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(AuthResponse), 400)]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return BadRequest(new GetUserResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No user with the given ID exists." }
                    });
                }
                else
                {
                    List<string> roles = (await userManager.GetRolesAsync(user)).ToList();

                    return Ok(new GetUserResponse()
                    {
                        Success = true,
                        Id = user.Id,
                        Username = user.UserName,
                        Prefix = user.Prefix,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Suffix = user.Suffix,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Roles = roles
                    });
                }

            }

            return BadRequest(new GetUserResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpGet]
        [Route("current")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        public async Task<IActionResult> GetCurrentUser()
        {
            string id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            return await GetUser(id);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(AuthResponse), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? existingUser = await userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new AuthResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "A user with the given e-mail address already exists." }
                    });
                }

                ApplicationUser? newUser = new ApplicationUser() { Email = user.Email, UserName = user.Name };
                IdentityResult? isCreated = await userManager.CreateAsync(newUser, user.Password);
                List<string> errors = new List<string>();

                if (isCreated.Succeeded)
                {
                    IdentityResult addedToRole = await userManager.AddToRoleAsync(newUser, ApplicationRoles.USER);

                    if (addedToRole.Succeeded)
                    {
                        AuthResult result = await authTokenService.GenerateToken(newUser);

                        if (result.Success)
                        {
                            return Ok(new AuthResponse()
                            {
                                Success = true,
                                Token = result.Token,
                                RefreshToken = result.RefreshToken
                            });
                        }
                        else
                        {
                            errors.AddRange(result.Errors);
                        }
                    }
                    else
                    {
                        errors.AddRange(addedToRole.Errors.Select(x => x.Description).ToList());
                    }
                }
                else
                {
                    errors.AddRange(isCreated.Errors.Select(x => x.Description).ToList());
                }

                return new JsonResult(new AuthResponse()
                {
                    Success = false,
                    Errors = errors
                }
                )
                { StatusCode = 500 };
            }

            return BadRequest(new AuthResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, [FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? trackedModel = await userManager.FindByIdAsync(id.ToString());

                if (trackedModel == null)
                {
                    return NotFound(new GetUserResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }
                else
                {
                    if (request.Username != null) await userManager.SetUserNameAsync(trackedModel, request.Username);
                    if (request.Username == "") await userManager.SetUserNameAsync(trackedModel, null);

                    if (request.Prefix != null) trackedModel.Prefix = request.Prefix;
                    if (request.Prefix == "") trackedModel.Prefix = null;

                    if (request.FirstName != null) trackedModel.FirstName = request.FirstName;
                    if (request.FirstName == "") trackedModel.FirstName = null;

                    if (request.LastName != null) trackedModel.LastName = request.LastName;
                    if (request.LastName == "") trackedModel.LastName = null;

                    if (request.Suffix != null) trackedModel.Suffix = request.Suffix;
                    if (request.Suffix == "") trackedModel.Suffix = null;

                    if (request.Notes != null) trackedModel.Notes = request.Notes;
                    if (request.Notes == "") trackedModel.Notes = null;

                    if (request.Phone != null) trackedModel.PhoneNumber = request.Phone;
                    if (request.Phone == "") trackedModel.PhoneNumber = null;

                    if (request.Email != null)
                    {
                        string mailChangeToken = await userManager.GenerateChangeEmailTokenAsync(trackedModel, request.Email);
                        await userManager.ChangeEmailAsync(trackedModel, request.Email, mailChangeToken);
                    }

                    await userManager.UpdateAsync(trackedModel);

                    List<string> roles = (await userManager.GetRolesAsync(trackedModel)).ToList();

                    return Ok(new GetUserResponse()
                    {
                        Success = true,
                        Id = trackedModel.Id,
                        Username = trackedModel.UserName,
                        Prefix = trackedModel.Prefix,
                        FirstName = trackedModel.FirstName,
                        LastName = trackedModel.LastName,
                        Suffix = trackedModel.Suffix,
                        Email = trackedModel.Email,
                        Phone = trackedModel.PhoneNumber,
                        Roles = roles
                    });
                }
            }
            else
            {
                return BadRequest(new GetUserResponse()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid request." }
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.ADMIN)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? model = await userManager.FindByIdAsync(id.ToString());

                if (model == null)
                {
                    return NotFound(new DeleteResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "No entity with the given ID exists." }
                    });
                }

                if (await userManager.IsInRoleAsync(model, ApplicationRoles.ROOT))
                {
                    return BadRequest(new DeleteResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "Cannot delete root user." }
                    });
                }

                await userManager.DeleteAsync(model);

                return Ok(new DeleteResponse()
                {
                    Success = true
                });
            }

            return BadRequest(new DeleteResponse()
            {
                Success = false,
                Errors = new List<string>() { "Invalid request." }
            });
        }

        [HttpPost]
        [Route("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(ChangePasswordResponse), 200)]
        [ProducesResponseType(typeof(ChangePasswordResponse), 400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                string userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                ApplicationUser? targetUser = await userManager.FindByIdAsync(userId);

                if (targetUser == null)
                {
                    return BadRequest(new ChangePasswordResponse()
                    {
                        Success = false,
                        Errors = new List<string> {
                            "Invalid request."
                        }
                    });
                }

                IdentityResult result = await userManager.ChangePasswordAsync(targetUser, request.OldPassword, request.NewPassword);

                if (result.Succeeded)
                {
                    return Ok(new ChangePasswordResponse()
                    {
                        Success = true,
                    });
                }
                else
                {
                    return BadRequest(new ChangePasswordResponse()
                    {
                        Success = false,
                        Errors = result.Errors.Select(e => $"{e.Code}: {e.Description}").ToList()
                    });
                }
            }

            return BadRequest(new ChangePasswordResponse()
            {
                Success = false,
                Errors = new List<string> {
                            "Invalid request."
                        }
            });
        }

        [HttpPost]
        [Route("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ChangePasswordResponse), 200)]
        [ProducesResponseType(typeof(ChangePasswordResponse), 400)]
        public async Task<IActionResult> RequestResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? targetUser = await userManager.FindByEmailAsync(request.Email);

                if (targetUser == null)
                {
                    return BadRequest(new ChangePasswordResponse()
                    {
                        Success = false,
                        Errors = new List<string> {
                            "Invalid request."
                        }
                    });
                }

                ChangePasswordResponse result = await resetPasswordService.RequestPasswordReset(targetUser);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest(new ChangePasswordResponse()
            {
                Success = false,
                Errors = new List<string> {
                            "Invalid request."
                        }
            });
        }

        [HttpPost]
        [Route("confirm-reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ChangePasswordResponse), 200)]
        [ProducesResponseType(typeof(ChangePasswordResponse), 400)]
        public async Task<IActionResult> FinalizeResetPassword([FromBody] FinalizeResetPasswordRequest request)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? targetUser = await userManager.FindByEmailAsync(request.Email);

                if (targetUser == null)
                {
                    return BadRequest(new ChangePasswordResponse()
                    {
                        Success = false,
                        Errors = new List<string> {
                            "Invalid request."
                        }
                    });
                }

                ChangePasswordResponse result = await resetPasswordService.FinalizePasswordReset(targetUser, request.ResetToken, request.NewPassword);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }

            return BadRequest(new ChangePasswordResponse()
            {
                Success = false,
                Errors = new List<string> {
                            "Invalid request."
                        }
            });
        }

    }
}
