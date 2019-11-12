using System.Net.Http;
using System.Threading.Tasks;
using CassandraExample.API.Application.Commands;
using CassandraExample.API.DTOs;
using IdentityModel.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CassandraExample.API.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] LoginCommand command)
        {
            if (!ModelState.IsValid)  return BadRequest(ModelState);

            var result = await mediator.Send(command);
            
            return Ok(result);
        }
    }
}