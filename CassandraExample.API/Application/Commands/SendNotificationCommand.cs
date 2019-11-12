using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CassandraExample.API.DTOs;
using MediatR;

namespace CassandraExample.API.Application.Commands
{
    public class SendNotificationCommand : IRequest
    {
        [Required]
        public string Title { get; set; }
        public string Message { get; set; }
        [Required]
        public IEnumerable<string> PhoneNumbers { get; set; }
        [Required]
        public string AppName { get; set; }
        public string ProjectName { get; set; }
        public NotificationFilterDTO Filter { get; set; }
    }
}