using System.Collections.Generic;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using MediatR;

namespace CassandraExample.API.Application.Queries
{
    public class UserTokensQuery : IRequest<IEnumerable<Token>>
    {
        public string PhoneNumber { get; set; }
    }
}