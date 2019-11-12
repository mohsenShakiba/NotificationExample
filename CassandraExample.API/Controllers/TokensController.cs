using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CassandraExample.API.Application.Commands;
using CassandraExample.API.Application.Queries;
using CassandraExample.API.DTOs;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CassandraExample.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class TokensController : Controller
    {
        private readonly IMediator mediator;

        public TokensController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> AddToken([FromBody] RegisterTokenCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                await mediator.Send(command);
                return Ok();
            }
            catch (InvalidAppException)
            {
                return BadRequest("InvalidApp");
            }
        }

        [HttpGet("{phoneNumber}")]
        public async Task<IActionResult> GetTokens([Required] [FromRoute] string phoneNumber)
        {
            if (!ModelState.IsValid) return BadRequest();

            var query = new UserTokensQuery
            {
                PhoneNumber = phoneNumber
            };
            
            var tokens = await mediator.Send(query);
            return Ok(tokens);
        }
        
    }
}