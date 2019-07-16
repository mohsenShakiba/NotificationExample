using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CassandraExample.API.DTOs;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Domain.Exceptions;
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

    public class UserNotificationsQueryHandler : IRequestHandler<UserNotificationsQuery, IEnumerable<NotificationDTO>>
    {
        private readonly IAppRepository appRepository;
        private readonly INotificationRepository notificationRepository;

        public UserNotificationsQueryHandler(INotificationRepository notificationRepository,
            IAppRepository appRepository)
        {
            this.notificationRepository = notificationRepository;
            this.appRepository = appRepository;
        }

        public async Task<IEnumerable<NotificationDTO>> Handle(UserNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            var app = await appRepository.FindAsync(request.AppName);
            if (app == null) throw new InvalidAppException();
            var notifications =
                await notificationRepository.FindAsync(app.Id, request.PhoneNumber, request.Limit, request.Offset);
            return notifications.Select(n => new NotificationDTO(n));
        }
    }
}