using System.Threading.Tasks;
using CassandraExample.Domain.AggregatesModel.SeedWork;

namespace CassandraExample.Domain.AggregatesModel.AppAggregate
{
    public interface IAppRepository: IRepository<App>
    {
        Task<App> FindAsync(string name);
    }
}