using System.Threading.Tasks;
using CassandraExample.Domain.Abstractions;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Notification = CassandraExample.Domain.AggregatesModel.NotificationAggregate.Notification;

namespace CassandraExample.API.Application.Services
{
    public class FireBaseService : IFireBaseService
    {
        private readonly ILogger<FireBaseService> logger;

        public FireBaseService(ILogger<FireBaseService> logger, IAppRepository appRepository)
        {
            this.logger = logger;
        }

        public async Task<bool> SendAsync(Notification notification, App app, Token token)
        {
            // creating message
            var message = new Message
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = notification.Title,
                    Body = notification.Message
                },
                Token = token.TokenString
            };
            // sending message
            try
            {
                var fireBaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(app.Json)
                }, app.Name);
                var response = await FirebaseMessaging.GetMessaging(fireBaseApp).SendAsync(message);
                logger.LogInformation("Message sent to FireBase with result: {}", response);
                logger.LogInformation("FIREBASE_NOTIFICATION_SUCCESS");
                return true;
            }
            catch (FirebaseException)
            {
                logger.LogError("Message failed with FireBase exception token: {}", token.TokenString);
                logger.LogInformation("FIREBASE_NOTIFICATION_ERROR");
                return false;
            }
        }
    }
}