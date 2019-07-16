using Cassandra;

namespace CassandraExample.Domain.AggregatesModel.TokenAggregate
{
    public class Token
    {
        public TimeUuid Id { get; set; }
        public string UserPhoneNumber { get; set; }
        public int AppId { get; set; }
        public float Version { get; set; }
        public string TokenString { get; set; }
        public DeviceTypes DeviceTypes { get; set; }

        public void UpdateId()
        {
            Id = TimeUuid.NewId();
        }
    }
}