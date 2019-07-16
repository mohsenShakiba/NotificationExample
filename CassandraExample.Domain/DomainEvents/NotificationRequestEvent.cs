using System.Collections.Generic;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;

namespace CassandraExample.Domain.DomainEvents
{
    public class NotificationRequestEvent
    {
        public Notification Notification { get; set; }
        public IEnumerable<Token> DeviceTokens { get; set; }
    }
}