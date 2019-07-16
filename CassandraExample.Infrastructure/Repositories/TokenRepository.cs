using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra.Data.Linq;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using CassandraExample.Infrastructure.Data;

namespace CassandraExample.Infrastructure.Repositories
{
    public class TokenRepository: ITokenRepository
    {
        private readonly PushContext pushContext;


        public TokenRepository(PushContext pushContext)
        {
            this.pushContext = pushContext;
        }


        public async Task<IEnumerable<Token>> FindLatestAsync(int appId, string phoneNumber, int limit, float? ltVersion, float? gtVersion,
            DeviceType? deviceType)
        {
            var table = await pushContext.TokenTable
                .Where(t => t.UserPhoneNumber == phoneNumber && t.AppId == appId)
                .Take(limit)
                .ExecuteAsync();
            
            return table
                .Where(t => t.Version >= (gtVersion ?? int.MinValue))
                .Where(t => t.Version <= (ltVersion ?? int.MaxValue));
        }

        public async Task<Token> FindAsync(string token, string phoneNumber)
        {
            var tokens = await pushContext.TokenTable
                .Where(t => t.UserPhoneNumber == phoneNumber)
                .ExecuteAsync();
            return tokens.FirstOrDefault(t => t.TokenString == token);
        }

        public Task<IEnumerable<Token>> FindAllAsync(string phoneNumber)
        {
            var query = pushContext.TokenTable.Where(t => t.UserPhoneNumber == phoneNumber);
            return query.ExecuteAsync();
        }

        public Task InsertAsync(Token token)
        {
            return pushContext.TokenTable.Insert(token).ExecuteAsync();
        }

        public Task UpdateAsync(Token token)
        {
            return pushContext.TokenTable
                .Where(t => t.UserPhoneNumber == token.UserPhoneNumber)
                .Where(t => t.AppId == token.AppId)
                .Where(t => t.Id == token.Id)
                .Select(t => new Token()
                {
                    TokenString = token.TokenString,
                })
                .Update().ExecuteAsync();
        }
    }
}