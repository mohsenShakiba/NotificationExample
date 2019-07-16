namespace CassandraExample.Domain.AggregatesModel.SeedWork
{
    public interface IRepository<T> where T: IAggregateRoot
    {
    }
}