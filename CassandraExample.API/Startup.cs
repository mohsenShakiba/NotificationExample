using Cassandra.Mapping;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using CassandraExample.Infrastructure.Data;
using CassandraExample.Infrastructure.Mappings;
using CassandraExample.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CassandraExample.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IAppRepository, AppRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddSingleton<PushContext>();

            services.AddMvc();
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            MappingConfiguration.Global.Define<PushMappings>();
        }
    }
}