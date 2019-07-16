using System;
using System.Collections.Generic;
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using CassandraExample.Domain.AggregatesModel.AppAggregate;
using CassandraExample.Domain.AggregatesModel.NotificationAggregate;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using Microsoft.Extensions.Configuration;

namespace CassandraExample.Infrastructure.Data
{
    public class PushContext
    {
        public readonly Table<Token> TokenTable;
        public readonly Table<App> AppTable;
        public readonly Table<Notification> NotificationTable;
        public readonly IMapper Mapper;
        
        
        public PushContext(IConfiguration configuration)
        {
            var cluster = Cluster.Builder()
                .AddContactPoints(configuration.GetConnectionString("Cassandra"))
                .WithDefaultKeyspace(configuration.GetSection("Cassandra")["KeySpace"])
                .Build();
            
            Console.WriteLine("created keyspace");
            
            var session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists(new Dictionary<string, string>(){{"class",  Cassandra.ReplicationStrategies.SimpleStrategy}, {"replication_factor", "1"}});
            Mapper = new Mapper(session);
            Execute(session);
            Console.WriteLine("created mapper");
     
            TokenTable = new Table<Token>(session);
            TokenTable.CreateIfNotExists();
            Console.WriteLine("created token");

            AppTable = new Table<App>(session);
            AppTable.CreateIfNotExists();
            Console.WriteLine("created app");

            NotificationTable = new Table<Notification>(session);
            NotificationTable.CreateIfNotExists();
        }
        
        void Execute(ISession session)
        {
            session.Execute(
                "CREATE MATERIALIZED VIEW IF NOT EXISTS app_mv AS SELECT id, name, json FROM apps " +
                "WHERE id IS NOT NULL AND name IS NOT NULL " +
                "PRIMARY KEY (id, name)"
            );
            
            session.Execute(
                "CREATE INDEX IF NOT EXISTS ON notifications (id)"
            );
        }
        
    }
    

}