using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TestTask_Core3.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : class
    {
        private readonly TestDBContext repositoryContext;

        public RepositoryBase(TestDBContext repositoryContext)
        {
            this.repositoryContext = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }

        public IQueryable<T> FindAll()
        {
            return repositoryContext.Set<T>()
                .AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return repositoryContext.Set<T>()
                .Where(expression);
        }

        public void Create(T entity)
        {
            repositoryContext.Set<T>().Add(entity);
            repositoryContext.SaveChanges();
        }

        public void Update(T entity)
        {
            repositoryContext.Set<T>().Update(entity);
            repositoryContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            repositoryContext.Set<T>().Remove(entity);
            repositoryContext.SaveChanges();
        }

        public void Save()
        {
            var res = repositoryContext.SaveChanges();
        }
    }
}
