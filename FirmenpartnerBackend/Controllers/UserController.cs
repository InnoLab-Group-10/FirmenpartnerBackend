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
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthTokenService authTokenService;
        private readonly IResetPasswordService resetPasswordService;

        public UserController(UserManager<IdentityUser> userManager, IAuthTokenService authTokenService, IResetPasswordService resetPasswordService)
        {
            this.userManager = userManager;
            this.authTokenService = authTokenService;
            this.resetPasswordService = resetPasswordService;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(AuthResponse), 400)]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? user = await userManager.FindByIdAsync(id);

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
                    return Ok(new GetUserResponse()
                    {
                        Success = true,
                        Errors = new List<string>(),
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email
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
        [ProducesResponseType(typeof(AuthResponse), 400)]
        public async Task<IActionResult> GetCurrentUser()
        {
            string id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
            return await GetUser(id);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(AuthResponse), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? existingUser = await userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new AuthResponse()
                    {
                        Success = false,
                        Errors = new List<string>() { "A user with the given e-mail address already exists." }
                    });
                }

                IdentityUser? newUser = new IdentityUser() { Email = user.Email, UserName = user.Name };
                IdentityResult? isCreated = await userManager.CreateAsync(newUser, user.Password);
                if (isCreated.Succeeded)
                {
                    AuthResult result = await authTokenService.GenerateToken(newUser);

                    return Ok(new AuthResponse()
                    {
                        Success = true,
                        Token = result.Token,
                        RefreshToken = result.RefreshToken
                    });
                }

                return new JsonResult(new AuthResponse()
                {
                    Success = false,
                    Errors = isCreated.Errors.Select(x => x.Description).ToList()
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
                IdentityUser? targetUser = await userManager.FindByIdAsync(userId);

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
                        Errors = new List<string>()
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
                IdentityUser? targetUser = await userManager.FindByEmailAsync(request.Email);

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
                IdentityUser? targetUser = await userManager.FindByEmailAsync(request.Email);

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
