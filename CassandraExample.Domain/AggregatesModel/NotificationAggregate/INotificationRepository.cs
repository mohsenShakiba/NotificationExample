using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra;

namespace CassandraExample.Domain.AggregatesModel.NotificationAggregate
{
    public interface INotificationRepository
    {
        Task InsertAsync(Notification notification);
        Task<IEnumerable<Notification>> FindAsync(int appId, string phoneNumber, int limit, int offset);
        Task<Notification> FindAsync(TimeUuid timeUuid);
    }
}