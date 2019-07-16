using Cassandra;

namespace CassandraExample.Domain.AggregatesModel.NotificationAggregate
{
    public class Notification
    {
        public string UserPhoneNumber { get; set; }
        public int AppId { get; set; }
        public TimeUuid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int BadgeCount { get; set; }
        public NotificationStatus Status { get; set; }

        public Notification()
        {
        }

        public Notification(string userPhoneNumber, int appId, string title, string message)
        {
            UserPhoneNumber = userPhoneNumber;
            AppId = appId;
            Id = TimeUuid.NewId();
            Title = title;
            Message = message;
            Status = NotificationStatus.Waiting;
        }
        
        public void SetNotificationResult(bool success)
        {
            Status = success ? NotificationStatus.Sent : NotificationStatus.Error;
        }
    }
}