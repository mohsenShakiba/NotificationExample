using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;

namespace CassandraExample.Infrastructure.Mappings
{
    public class PushMappings: Cassandra.Mapping.Mappings
    {
        public PushMappings()
        {
            For<Token>()
                .TableName("tokens")
                .PartitionKey(t => t.UserPhoneNumber)
                .ClusteringKey(t => t.AppId)
                .ClusteringKey(t => t.Id)
                .Column(t => t.Id, cm => cm.WithName("id"))
                .Column(t => t.UserPhoneNumber, cm => cm.WithName("phone"))
                .Column(t => t.Version, cm => cm.WithName("version"))
                .Column(t => t.TokenString, cm => cm.WithName("t_string").WithDbType<string>())
                .Column(t => t.AppId, cm => cm.WithName("app_id"))
                .Column(t => t.DeviceType, cm => cm.WithName("type").WithDbType<int>());

            For<App>()
                .TableName("apps")
                .PartitionKey(t => t.Name)
                .ClusteringKey(t => t.Id)
                .Column(t => t.Id, cm => cm.WithName("id"))
                .Column(t => t.Name, cm => cm.WithName("name"))
                .Column(t => t.Json, cm => cm.WithName("json"));

            For<Notification>()
                .TableName("notifications")
                .PartitionKey(t => t.UserPhoneNumber, t => t.AppId)
                .ClusteringKey(t => t.Id)
                .Column(t => t.Id, cm => cm.WithName("id"))
                .Column(t => t.Title, cm => cm.WithName("title"))
                .Column(t => t.Message, cm => cm.WithName("message"))
                .Column(t => t.AppId, cm => cm.WithName("app_id"))
                .Column(t => t.UserPhoneNumber, cm => cm.WithName("phone"))
                .Column(t => t.BadgeCount, cm => cm.Ignore())
                .Column(t => t.Status, cm => cm.WithName("status").WithDbType<int>());

        }
    }
}