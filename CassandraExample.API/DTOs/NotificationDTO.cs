using CassandraExample.Domain.AggregatesModel.NotificationAggregate;

namespace CassandraExample.API.DTOs
{
    public class NotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public long DateCreated { get; set; }

        public NotificationDTO(Notification notification)
        {
            Title = notification.Title;
            Message = notification.Message;
            DateCreated = notification.Id.GetDate().ToUnixTimeSeconds();
        }
        
    }
}