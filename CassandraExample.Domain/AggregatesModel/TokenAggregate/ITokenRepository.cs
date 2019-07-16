using System.Collections.Generic;
using System.Threading.Tasks;

namespace CassandraExample.Domain.AggregatesModel.TokenAggregate
{
    public interface ITokenRepository
    {
        Task<IEnumerable<Token>> FindLatestAsync(int appId, string phoneNumber, int limit, float? ltVersion, float? gtVersion, DeviceType? deviceType);
        Task<Token> FindAsync(string token, string phoneNumber);
        Task<IEnumerable<Token>> FindAllAsync(string phoneNumber);
        Task InsertAsync(Token token);
        Task UpdateAsync(Token token);
    }
}