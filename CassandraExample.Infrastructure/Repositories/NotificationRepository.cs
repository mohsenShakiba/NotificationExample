using System.Collections.Generic;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Data.Linq;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Infrastructure.Data;

namespace CassandraExample.Infrastructure.Repositories
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly PushContext pushContext;

        public NotificationRepository(PushContext pushContext)
        {
            this.pushContext = pushContext;
        }
        
        public Task InsertAsync(Notification notification)
        {
            return pushContext.NotificationTable.Insert(notification).ExecuteAsync();
        }

        public Task UpdateAsync(Notification notification)
        {
            return pushContext.NotificationTable
                .Where(n => n.UserPhoneNumber == notification.UserPhoneNumber)
                .Where(n => n.AppId == notification.AppId)
                .Where(n => n.Id == notification.Id)
                .Select(_ => new Notification()
                {
                    Status = notification.Status
                })
                .Update().ExecuteAsync();
        }

        public async Task<IEnumerable<Notification>> FindAsync(int appId, string phoneNumber, int limit, int offset)
        {
            return await pushContext.NotificationTable
                .Where(n => n.UserPhoneNumber == phoneNumber)
                .Where(n => n.AppId == appId)
                .Take(limit)
                .ExecuteAsync();
        }

        public async Task<Notification> FindAsync(TimeUuid timeUuid)
        {
            return await pushContext.NotificationTable
                .FirstOrDefault(n => n.Id == timeUuid)
                .ExecuteAsync();
        }
    }
}