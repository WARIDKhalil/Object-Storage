using Domain.Shared;

namespace Domain.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        public Task<T> GetByIdAsync(Guid id);
        public Task<T> AddAsync(T entity);
        public Task DeleteByIdAsync(Guid id);
    }
}
