using System.Threading.Tasks;
using Cassandra.Data.Linq;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Infrastructure.Data;

namespace CassandraExample.Infrastructure.Repositories
{
    public class AppRepository: IAppRepository
    {
        private readonly PushContext pushContext;

        public AppRepository(PushContext pushContext)
        {
            this.pushContext = pushContext;
        }
        
        public async Task<App> FindAsync(string name)
        {
            return await pushContext.AppTable.FirstOrDefault(a => a.Name == name).ExecuteAsync();
        }

        public Task<App> FindByIdAsync(int id)
        {
            return pushContext.Mapper.FirstAsync<App>("SELECT * FROM app_mv WHERE id = ?", id);
        }

    }
}