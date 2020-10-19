using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Sql
{
    public class DatabaseRepository<TEntity> : IDatabaseRepository<TEntity> where TEntity : Base
    {
        private readonly DbSet<TEntity> dbSet;
        protected DatabaseSimulatorContext Context { get; }

        public DatabaseRepository(DatabaseSimulatorContext managementContext)
        {
            Context = managementContext;
            dbSet = Context.Set<TEntity>();
        }

        public async Task<TEntity> RefreshEntity(TEntity entity)
        {
            await Context.Entry(entity).ReloadAsync();
            return entity;
        }

        public virtual Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable = Find();

            foreach (var expression in includes)
            {
                queryable = queryable.Include(expression);
            }

            return queryable.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual Task<List<TEntity>> GetByIdsAsync(IEnumerable<int> ids, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable = Find();

            foreach (var expression in includes)
            {
                queryable = queryable.Include(expression);
            }

            return queryable.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual IQueryable<TEntity> Find()
        {
            return dbSet.AsQueryable();
        }

        public async Task<List<T>> LoadChildEntities<T>(TEntity entity, Expression<Func<TEntity, IEnumerable<T>>> collectionExpression) where T : class
        {
            await Context.Entry(entity).Collection(collectionExpression).LoadAsync();

            return collectionExpression.Compile()(entity)?.ToList() ?? new List<T>();
        }

        public virtual TEntity Add(TEntity entity)
        {
            var timestamp = DateTime.UtcNow;
            //entity.CreatedOn = timestamp;
            //entity.ModifiedOn = timestamp;
            dbSet.Add(entity);
            return entity;
        }

        public virtual void AddRange(List<TEntity> entities)
        {
            var timestamp = DateTime.UtcNow;
            foreach (var entity in entities)
            {
                //entity.CreatedOn = timestamp;
                //entity.ModifiedOn = timestamp;
            }
            dbSet.AddRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            Context.Remove(entity);
        }

        public void DeleteRange(List<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public virtual async Task SaveAsync()
        {
            var timestamp = DateTime.UtcNow;
            var changedEntriesCopy = Context.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            foreach (var entry in changedEntriesCopy)
            {
                //(entry.Entity as Base).ModifiedOn = timestamp;
            }

            await Context.SaveChangesAsync();
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = Context.ChangeTracker.Entries().Where(e => e.State != EntityState.Detached).ToList();
            foreach (var entry in changedEntriesCopy)
            {
                entry.State = EntityState.Detached;
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
