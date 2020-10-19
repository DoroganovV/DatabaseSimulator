using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Sql
{
    public interface IDatabaseRepository<TEntity> : IDisposable where TEntity : Base
    {
        Task<TEntity> RefreshEntity(TEntity entity);
        Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);
        Task<List<TEntity>> GetByIdsAsync(IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> Find();
        Task<List<T>> LoadChildEntities<T>(TEntity entity, Expression<Func<TEntity, IEnumerable<T>>> collectionExpression) where T : class;
        TEntity Add(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entities);
        Task SaveAsync();
        void DetachAllEntities();
    }
}
