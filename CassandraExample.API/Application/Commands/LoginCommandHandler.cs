using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CassandraExample.API.DTOs;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace CassandraExample.API.Application.Commands
{
    public class LoginCommandHandler: IRequestHandler<LoginCommand, LoginResultDTO>
    {
        private readonly IConfiguration configuration;

        public LoginCommandHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public async Task<LoginResultDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            
            var disco = await client.GetDiscoveryDocumentAsync(
                new DiscoveryDocumentRequest
                {
                    Address = configuration.GetValue<string>("IdentityServerUrl"),
                    Policy =
                    {
                        RequireHttps = false
                    }
                }, cancellationToken: cancellationToken);
            
            if (disco.IsError)
                return new LoginResultDTO() {Success = false};
            
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = request.UserId,
                ClientSecret = request.Password,
                Scope = "push"
            }, cancellationToken: cancellationToken);

            if (tokenResponse.IsError)
                return new LoginResultDTO() {Success = false};
            
            return new LoginResultDTO()
            {
                Token = tokenResponse.AccessToken,
                Success = true
            };
        }
    }
}