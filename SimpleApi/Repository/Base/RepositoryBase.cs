using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SimpleApi.Repository.Base
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public RepositoryBase(DbContext context)
        {
            Context = context;
        }

        public virtual TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AsNoTracking().Any(predicate);
        }

        public virtual bool Add(TEntity entity)
        {
            try { Context.Set<TEntity>().Add(entity); }
            catch (Exception e) { return false; }
            return true;
        }
        public virtual bool Update(TEntity entity)
        {
            try { Context.Set<TEntity>().Update(entity); }
            catch (Exception e){ return false; }
            return true;
        }

        public virtual bool AddRange(IEnumerable<TEntity> entities)
        {
            try { Context.Set<TEntity>().AddRange(entities); }
            catch { return false; }
            return true;
        }

        public virtual bool Remove(TEntity entity)
        {
            try { Context.Set<TEntity>().Remove(entity); }
            catch (Exception e) { return false; }
            return true;
        }

        public virtual bool RemoveRange(IEnumerable<TEntity> entities)
        {
            try { Context.Set<TEntity>().RemoveRange(entities); }
            catch (Exception e) { return true; }
            return false;
        }

        public virtual int Save()
        {
            return Context.SaveChanges();
        }
    }
}
