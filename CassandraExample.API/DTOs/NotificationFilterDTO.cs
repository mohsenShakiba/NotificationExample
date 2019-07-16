using CassandraExample.Domain.AggregatesModel.TokenAggregate;

namespace CassandraExample.API.DTOs
{
    public class NotificationFilterDTO
    {
        public float? LtVersion { get; set; }
        public float? GtVersion { get; set; }
        public string Device { get; set; }
        
        public DeviceTypes? DeviceType
        {
            get
            {
                switch (Device.ToLower())
                {
                    case "android": return DeviceTypes.Android;
                    case "ios": return DeviceTypes.Android;
                }
                return null;
            }
        }
    }
}