using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using MediatR;

namespace CassandraExample.API.Application.Queries
{
    public class UserTokensQueryHandler : IRequestHandler<UserTokensQuery, IEnumerable<Token>>
    {
        private readonly ITokenRepository tokenRepository;

        public UserTokensQueryHandler(ITokenRepository tokenRepository)
        {
            this.tokenRepository = tokenRepository;
        }

        public Task<IEnumerable<Token>> Handle(UserTokensQuery request, CancellationToken cancellationToken)
        {
            return tokenRepository.FindAllAsync(request.PhoneNumber);
        }
    }
}