using CassandraExample.Domain.AggregatesModel.NotificationAggregate;

namespace CassandraExample.Domain.DomainEvents
{
    public class NotificationSendEvent
    {
        public Notification Notification { get; set; }
    }
}