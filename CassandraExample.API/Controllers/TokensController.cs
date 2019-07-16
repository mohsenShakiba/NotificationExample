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
        private readonly IAppRepository appRepository;
        private readonly IMediator mediator;

        public TokensController(IMediator mediator, IAppRepository appRepository)
        {
            this.mediator = mediator;
            this.appRepository = appRepository;
        }


        [HttpPost]
        public async Task<IActionResult> AddToken([FromBody] RegisterTokenDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var command = new RegisterTokenCommand
            {
                PhoneNumber = dto.PhoneNumber,
                AppName = dto.AppName,
                ProjectName = dto.ProjectName,
                Token = dto.Token,
                Version = dto.Version,
                DeviceTypes = dto.DeviceTypes
            };
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

        [HttpGet("app/{name}")]
        public async Task<IActionResult> GetProject([Required] [FromRoute] string name)
        {
            var app = await appRepository.FindAsync(name);
            return Ok(app);
        }
    }
}