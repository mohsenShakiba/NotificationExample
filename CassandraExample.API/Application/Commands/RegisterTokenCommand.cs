using System.Threading;
using System.Threading.Tasks;
using Cassandra;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using CassandraExample.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CassandraExample.API.Application.Commands
{
    public class RegisterTokenCommand : IRequest
    {
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public float Version { get; set; }
        public string AppName { get; set; }
        public string ProjectName { get; set; }
        public DeviceTypes DeviceTypes { get; set; }
    }

    public class RegisterTokenCommandHandler : IRequestHandler<RegisterTokenCommand>
    {
        private readonly IAppRepository appRepository;
        private readonly ILogger<RegisterTokenCommandHandler> logger;
        private readonly IMediator mediator;
        private readonly ITokenRepository tokenRepository;

        public RegisterTokenCommandHandler(ITokenRepository tokenRepository, IAppRepository appRepository,
            IMediator mediator, ILogger<RegisterTokenCommandHandler> logger)
        {
            this.tokenRepository = tokenRepository;
            this.appRepository = appRepository;
            this.mediator = mediator;
            this.logger = logger;
        }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="InvalidAppException"></exception>
        /// <exception cref="InvalidProjectException"></exception>
        public async Task<Unit> Handle(RegisterTokenCommand request, CancellationToken cancellationToken)
        {
            // find appId
            var app = await appRepository.FindAsync(request.AppName);
            // make sure app exists
            if (app == null)
            {
                logger.LogError("app with the name {} was not found", request.AppName);
                throw new InvalidAppException();
            }

            // log
            logger.LogInformation("TOKEN_INSERTED");
            // check if token exists
            var token = await tokenRepository.FindAsync(request.Token, request.PhoneNumber);
            // insert token
            if (token == null)
            {
                logger.LogInformation("registered token for project {} with phone {}", request.ProjectName,
                    request.PhoneNumber);
                // create token 
                token = new Token
                {
                    Id = TimeUuid.NewId(),
                    AppId = app.Id,
                    DeviceTypes = request.DeviceTypes,
                    TokenString = request.Token,
                    UserPhoneNumber = request.PhoneNumber,
                    Version = request.Version
                };
                // update user
                await tokenRepository.InsertAsync(token);
            }
            // update 
            else
            {
                logger.LogInformation("registering token not required for project {} with phone {}, it already exists",
                    request.ProjectName, request.PhoneNumber);
                token.UpdateId();
                await tokenRepository.UpdateAsync(token);
            }
            return Unit.Value;
        }
    }
}