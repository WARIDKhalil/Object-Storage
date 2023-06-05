using Domain.Contracts;
using Domain.Shared;
using Infrastructure.Database;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected IMongoCollection<T> _collection;

        public Repository(MongoContext context) 
        {
            _collection = context.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _collection.Find<T>(x => x.Id == id).FirstOrDefaultAsync<T>();
        }
    }
}
