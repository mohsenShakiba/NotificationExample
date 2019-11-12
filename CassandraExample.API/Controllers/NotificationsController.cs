using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CassandraExample.API.Application.Commands;
using CassandraExample.API.Application.Queries;
using CassandraExample.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CassandraExample.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly IMediator mediator;

        public NotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SendBatchNotification([FromBody] SendNotificationCommand command)
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

        [HttpGet("{appName}/{phoneNumber}")]
        public async Task<IActionResult> GetNotifications([FromRoute, Required] string appName, [FromRoute, Required] string phoneNumber,
            int limit = 20, int offset = 0)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var query = new UserNotificationsQuery
            {
                PhoneNumber = phoneNumber,
                AppName = appName,
                Limit = limit,
                Offset = offset
            };
            try
            {
                var result = await mediator.Send(query);
                return Ok(result);
            }
            catch (InvalidAppException)
            {
                return BadRequest("InvalidApp");
            }
        }
    }
}