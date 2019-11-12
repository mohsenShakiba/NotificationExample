using System.Collections.Generic;
using CassandraExample.API.DTOs;
using MediatR;

namespace CassandraExample.API.Application.Queries
{
    public class UserNotificationsQuery : IRequest<IEnumerable<NotificationDTO>>
    {
        public string PhoneNumber { get; set; }
        public string AppName { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}