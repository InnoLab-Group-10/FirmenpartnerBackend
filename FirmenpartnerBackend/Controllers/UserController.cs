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

        public UserController(UserManager<ApplicationUser> userManager, IAuthTokenService authTokenService, IResetPasswordService resetPasswordService)
        {
            this.userManager = userManager;
            this.authTokenService = authTokenService;
            this.resetPasswordService = resetPasswordService;
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
                        Email = user.Email,
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
