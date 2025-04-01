// DnDAdventure.Infrastructure/Repositories/InMemoryRepository.cs
using DnDAdventure.Core.Repositories;
using System.Collections.Concurrent;

namespace DnDAdventure.Infrastructure.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        private readonly ConcurrentDictionary<Guid, T> _storage = new();
        private readonly Func<T, Guid> _idSelector;

        public InMemoryRepository(Func<T, Guid> idSelector)
        {
            _idSelector = idSelector;
        }

        public Task<T?> GetByIdAsync(Guid id)
        {
            _storage.TryGetValue(id, out var entity);
            return Task.FromResult(entity);
        }

        public Task<List<T>> GetAllAsync()
        {
            return Task.FromResult(_storage.Values.ToList());
        }

        public Task<T> AddAsync(T entity)
        {
            var id = _idSelector(entity);
            _storage[id] = entity;
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(T entity)
        {
            var id = _idSelector(entity);
            _storage[id] = entity;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _storage.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
}