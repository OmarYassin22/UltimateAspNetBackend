using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryBase<T>(RepositoryContext context) : IRepositoryBase<T> where T : class
    {

        public IQueryable<T> FindAll(bool trackChanges)
       => trackChanges ? context.Set<T>() : context.Set<T>().AsNoTracking();

        public IQueryable<T> FindByConditions(Expression<Func<T, bool>> condition, bool trachChanges)
       => trachChanges ? context.Set<T>().Where(condition) : context.Set<T>().Where(condition).AsNoTracking();
        public void Create(T entity)
       => context.Add(entity);
        public void Update(T entity)
       => context.Update(entity);
        public void Delete(T entity)
       => context.Remove(entity);

        public async Task saveAsync()
       => await context.SaveChangesAsync();

    }
}
