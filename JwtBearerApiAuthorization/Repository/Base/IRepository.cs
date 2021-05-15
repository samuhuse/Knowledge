using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JwtBearerApiAuthorization.Repository.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        bool Exists(Expression<Func<TEntity, bool>> predicate);

        bool Add(TEntity entity);
        bool AddRange(IEnumerable<TEntity> entities);

        bool Update(TEntity entity);

        bool Remove(TEntity entity);
        bool RemoveRange(IEnumerable<TEntity> entities);

        int Save();
    }
}
