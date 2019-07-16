using System.Threading.Tasks;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;

namespace CassandraExample.Domain.Abstractions
{
    public interface IFireBaseService
    {
        Task<bool> SendAsync(Notification notification, App app, Token token);
    }
}