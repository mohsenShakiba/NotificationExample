using CassandraExample.Domain.AggregatesModel.NotificationAggregate;

namespace CassandraExample.Domain.DomainEvents
{
    public class NotificationResultEvent
    {
        public Notification Notification { get; set; }
    }
}