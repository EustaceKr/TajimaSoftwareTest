using Data.Context;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual T Create(T entity)
        {
            return _context.Set<T>().Add(entity).Entity;
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }
        
        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

    }
}
