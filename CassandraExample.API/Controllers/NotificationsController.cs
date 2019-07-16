using System.Threading.Tasks;
using CassandraExample.API.Application.Commands;
using CassandraExample.API.Application.Queries;
using CassandraExample.API.DTOs;
using CassandraExample.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CassandraExample.API.Controllers
{
//    [Authorize]
    [Route("api/v1/[controller]")]
    public class NotificationsController : Controller
    {
        private readonly IMediator mediator;

        public NotificationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SendBatchNotification([FromBody] SendBatchNotificationDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var command = new SendNotificationCommand
            {
                PhoneNumbers = dto.PhoneNumbers,
                Title = dto.Title,
                Message = dto.Message,
                AppName = dto.AppName,
                ProjectName = dto.ProjectName,
                Filter = dto.Filter
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

        [HttpGet("{appName}/{phoneNumber}")]
        public async Task<IActionResult> GetNotifications([FromRoute] string appName, [FromRoute] string phoneNumber,
            int limit = 20, int offset = 0)
        {
            var query = new UserNotificationsQuery
            {
                PhoneNumber = phoneNumber,
                AppName = appName
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