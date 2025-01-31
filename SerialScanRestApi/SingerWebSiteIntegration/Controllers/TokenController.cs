using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SingerWebSiteIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingerWebSiteIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenManager _tokenManager;
        public TokenController(IJwtTokenManager jwtTokenManager)
        {
            _tokenManager = jwtTokenManager;
        }
        [AllowAnonymous]
        [HttpPost("request")]

        // public IActionResult Authenticate([FromHeader]UserCredential Credential)
        public IActionResult Authenticate([FromHeader] string Username, [FromHeader] string Password)
        {
            UserCredential credential = new UserCredential();
            credential.Username = Username;
            credential.Password = Password;
            var token = _tokenManager.Authentication(Username, Password);
            if (string.IsNullOrEmpty(token))
            {
                ReturnMessage token1 = new ReturnMessage()
                {
                    status = "UNSUCCESS",
                    message = "Invalid Username Password",
                    token = "",
                    expiresIn = "0"
                };

                // return Unauthorized();


                return Unauthorized(JsonConvert.SerializeObject(token1));
                // return Ok(token);
            }
            else
            {
                ReturnMessage token1 = new ReturnMessage()
                {
                    status = "SUCCESS",
                    message = "Token Generated",
                    token = token,
                    expiresIn = "24 hours"
                };
                return Ok(token1);

            }
        }
    }
}
