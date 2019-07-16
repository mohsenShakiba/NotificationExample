using CassandraExample.Domain.AggregatesModel.SeedWork;

namespace CassandraExample.Domain.AggregatesModel.AppAggregate
{
    public class App: IAggregateRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Json { get; set; }
    }
}