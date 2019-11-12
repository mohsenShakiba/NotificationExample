using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CassandraExample.Domain.Abstractions;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using CassandraExample.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CassandraExample.API.Application.Commands
{
    public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
    {
        private readonly IFireBaseService fireBaseService;
        private readonly IAppRepository appRepository;
        private readonly ILogger<SendNotificationCommandHandler> logger;
        private readonly INotificationRepository notificationRepository;
        private readonly ITokenRepository tokenRepository;

        public SendNotificationCommandHandler(
            IFireBaseService fireBaseService,
            IAppRepository appRepository, INotificationRepository notificationRepository,
            ITokenRepository tokenRepository,
            ILogger<SendNotificationCommandHandler> logger)
        {
            this.fireBaseService = fireBaseService;
            this.appRepository = appRepository;
            this.notificationRepository = notificationRepository;
            this.tokenRepository = tokenRepository;
            this.logger = logger;
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAppException"></exception>
        /// <exception cref="InvalidProjectException"></exception>
        public async Task<Unit> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            // find appId
            var app = await appRepository.FindAsync(request.AppName);
            // make sure app exists
            if (app == null)
            {
                logger.LogError("app with the name {} was not found", request.AppName);
                throw new InvalidAppException();
            }
            // log event
            logger.LogInformation("SEND_NOTIFICATION");
            // insert user notifications
            var userNotifications =
                request.PhoneNumbers.Select(p => new Notification(p, app.Id, request.Title, request.Message));
            // get tokens
            foreach (var userNotification in userNotifications)
            {
                await notificationRepository.InsertAsync(userNotification);
                // get token 
                var tokens = await tokenRepository.FindLatestAsync(app.Id, userNotification.UserPhoneNumber, 3,
                    request.Filter?.LtVersion, request.Filter?.GtVersion, request.Filter?.DeviceType);
                // 
                logger.LogDebug("sending message to phone {} with id {}", userNotification.UserPhoneNumber,
                    userNotification.Id);
                // send fcm
                await SendFcmAsync(userNotification, app, tokens);
            }

            return Unit.Value;
        }

        private async Task SendFcmAsync(Notification userNotification, App app, IEnumerable<Token> tokens)
        {
            foreach (var token in tokens)
            {
                await fireBaseService.SendAsync(userNotification, app, token);
            }
        }
    }
}