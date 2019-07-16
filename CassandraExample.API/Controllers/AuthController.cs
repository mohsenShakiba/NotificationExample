using System.Net.Http;
using System.Threading.Tasks;
using CassandraExample.API.DTOs;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CassandraExample.API.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetClient([FromBody] AuthDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(
                new DiscoveryDocumentRequest
                {
                    Address = configuration.GetValue<string>("IdentityServerUrl"),
                    Policy =
                    {
                        RequireHttps = false
                    }
                }
            );
            if (disco.IsError) return BadRequest(disco.Error);
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = dto.UserId,
                ClientSecret = dto.Password,
                Scope = "push"
            });

            if (tokenResponse.IsError) return BadRequest(tokenResponse.ErrorDescription);
            return Ok(tokenResponse.Json);
        }
    }
}