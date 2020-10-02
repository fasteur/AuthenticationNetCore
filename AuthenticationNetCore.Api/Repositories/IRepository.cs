using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthenticationNetCore.Api.Repositories
{
    public interface IRepository<T>
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        T Get(Guid id);
        Task<T> GetAsync(Guid id);
        List<T> All();
        Task<List<T>> AllAsync();
        List<T> Find(Expression<Func<T, bool>> predicate);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        void SaveChanges();
        Task<int> SaveChangesAsync();
        List<T> Remove(T entity);
        Task<List<T>> RemoveAsyncById(Guid id);
    }
}