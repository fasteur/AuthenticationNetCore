using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace AuthenticationNetCore.Api.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected DataContext context;
        public GenericRepository(DataContext context)
        {
            this.context = context;
        }
        public virtual T Add(T entity)
        {
            return context
                .Add(entity)
                .Entity;
        }

        public async virtual Task<T> AddAsync(T entity)
        {
            var data =  await context.AddAsync(entity);
            return data.Entity;
        }

        public virtual List<T> All()
        {
            return context.Set<T>()
                .AsQueryable()
                .ToList();
        }

        public async virtual Task<List<T>> AllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public virtual List<T> Find(Expression<Func<T, bool>> predicate)
        {
            return context.Set<T>()
                .AsQueryable()
                .Where(predicate).ToList();
        }
        public async virtual Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>()
                .AsQueryable()
                .Where(predicate).ToListAsync();
        }

        public virtual T Get(Guid id)
        {
            return context.Find<T>(id);
        }

        public async virtual Task<T> GetAsync(Guid id)
        {
            return await context.FindAsync<T>(id);
        }

        public virtual List<T> Remove(T entity)
        {
            context.Remove<T>(entity);
            return context.Set<T>()
                .AsQueryable()
                .ToList();
        }

        public async virtual Task<List<T>> RemoveAsyncById(Guid id)
        {
            var entity = await context.FindAsync<T>(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
            return await context.Set<T>().ToListAsync();
        }

        public virtual void SaveChanges()
        {
            context.SaveChanges();
        }

        public async virtual Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public virtual T Update(T entity)
        {
            return context.Update(entity).Entity;
        }
    }
}