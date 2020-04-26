using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Net;
using TestTask_Core3.Services;

namespace WebService1.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/token")]
    [ApiVersionNeutral]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService jwtTokenService;

        public TokenController(IJwtTokenService jwtTokenService)
        {
            this.jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        /// <summary>
        /// Generate sample token
        /// </summary>       
        /// <returns>Generated token</returns>        
        [AllowAnonymous]
        [HttpGet("generate")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Generated token")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public IActionResult GenerateToken()
        {
            var token = jwtTokenService.GenerateToken();
            return Ok(token);
        }

        /// <summary>
        /// Validate sample token
        /// </summary>
        /// <param name="token">Token for validation</param>
        /// <returns>Token validation status</returns>        
        [HttpPost("validate")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Token validation status")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Bad request for missing or invalid parameter")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public IActionResult ValidateToken([FromBody] TokenKey token)
        {
            if (string.IsNullOrEmpty(token.Token))
            {
                return BadRequest();
            }
            var isValid = jwtTokenService.ValidateToken(token.Token);

            return Ok(new { isValid });
        }
        public class TokenKey
        {
            public string Token { get; set; }
        }
    }
}
