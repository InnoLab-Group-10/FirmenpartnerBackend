﻿using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Models.Request;
using FirmenpartnerBackend.Models.Response;
using FirmenpartnerBackend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirmenpartnerBackend.Controllers
{
    [Route("/api/session")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthTokenService authTokenService;

        public SessionController(UserManager<IdentityUser> userManager, IAuthTokenService authTokenService)
        {
            this.userManager = userManager;
            this.authTokenService = authTokenService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(AuthResponse), 400)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser? existingUser = await userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new AuthResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                                        "Invalid authentication request."
                                    }
                    });
                }

                bool isCorrect = await userManager.CheckPasswordAsync(existingUser, user.Password);

                if (isCorrect)
                {
                    AuthResult result = await authTokenService.GenerateToken(existingUser);

                    return Ok(new AuthResponse()
                    {
                        Success = true,
                        Token = result.Token,
                        RefreshToken = result.RefreshToken,
                    });
                }
                else
                {
                    // We dont want to give to much information on why the request has failed for security reasons
                    return BadRequest(new AuthResponse()
                    {
                        Success = false,
                        Errors = new List<string>(){
                                         "Invalid authentication request."
                                    }
                    });
                }
            }

            return BadRequest(new AuthResponse()
            {
                Success = false,
                Errors = new List<string>(){
                                        "Invalid request."
                                    }
            });
        }

        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(typeof(AuthResponse), 400)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                AuthResult? res = await authTokenService.RefreshToken(tokenRequest);

                if (res == null)
                {
                    return BadRequest(new AuthResponse()
                    {
                        Errors = new List<string>() {
                    "Invalid token."
                },
                        Success = false
                    });
                }

                return Ok(res);
            }

            return BadRequest(new AuthResponse()
            {
                Errors = new List<string>() {
                "Invalid request."
            },
                Success = false
            });
        }
    }
}
