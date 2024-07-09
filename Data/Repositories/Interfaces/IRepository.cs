using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T Create(T item);
        void Update(T item, params string[] propertiesToIgnore);
        void Delete(T item);
        Task<bool> SaveChangesAsync();
    }
}
