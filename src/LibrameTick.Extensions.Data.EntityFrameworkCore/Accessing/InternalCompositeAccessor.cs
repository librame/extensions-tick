#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Collections;
using Librame.Extensions.Data.Specification;
using System.Linq.Expressions;

namespace Librame.Extensions.Data.Accessing
{
    sealed class InternalCompositeAccessor : IAccessor
    {
        private readonly IAccessor[] _accessors;
        private readonly AccessorInteraction _interaction;


        public InternalCompositeAccessor(IEnumerable<IAccessor> accessors,
            AccessorInteraction interaction)
        {
            _accessors = accessors.ToArray();
            _interaction = interaction;
        }


        #region Private

        private void BatchingAccessors(Action<IAccessor> action)
        {
            foreach (var accessor in _accessors)
            {
                action.Invoke(accessor);
            }
        }

        private TResult? BatchingAccessors<TResult>(Func<IAccessor, TResult> func)
        {
            var result = default(TResult);

            foreach (var accessor in _accessors)
            {
                result = func.Invoke(accessor);
            }

            return result;
        }


        private void ChainingAccessorsByException(Action<IAccessor> action, int accessorIndex = 0)
        {
            try
            {
                action.Invoke(_accessors[accessorIndex]);
            }
            catch (Exception)
            {
                if (accessorIndex < _accessors.Length - 1)
                    ChainingAccessorsByException(action, accessorIndex++); // 链式处理

                throw; // 所有访问器均出错时则抛出异常
            }
        }

        private TResult ChainingAccessorsByException<TResult>(Func<IAccessor, TResult> func, int accessorIndex = 0)
        {
            try
            {
                return func.Invoke(_accessors[accessorIndex]);
            }
            catch (Exception)
            {
                if (accessorIndex < _accessors.Length - 1)
                    return ChainingAccessorsByException(func, accessorIndex++); // 链式处理

                throw; // 所有访问器均出错时则抛出异常
            }
        }

        #endregion


        public Type AccessorType
            => typeof(InternalCompositeAccessor);


        #region Exists

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate,
            bool checkLocal = true)
            where TEntity : class
            => ChainingAccessorsByException(a => a.Exists(predicate));

        public Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
            bool checkLocal = true, CancellationToken cancellationToken = default)
            where TEntity : class
            => ChainingAccessorsByException(a => a.ExistsAsync(predicate, checkLocal, cancellationToken));

        #endregion


        #region Find

        public TEntity? Find<TEntity>(params object?[]? keyValues)
            where TEntity : class
            => ChainingAccessorsByException(a => a.Find<TEntity>(keyValues));

        public object? Find(Type entityType, params object?[]? keyValues)
            => ChainingAccessorsByException(a => a.Find(entityType, keyValues));

        public ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindAsync<TEntity>(keyValues, cancellationToken));

        public ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues)
            => ChainingAccessorsByException(a => a.FindAsync(entityType, keyValues));

        public ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken)
            => ChainingAccessorsByException(a => a.FindAsync(entityType, keyValues, cancellationToken));

        public ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindAsync<TEntity>(keyValues));


        public IList<TEntity> FindListWithSpecification<TEntity>(ISpecification<TEntity>? specification = null)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindListWithSpecification(specification));

        public Task<IList<TEntity>> FindListWithSpecificationAsync<TEntity>(ISpecification<TEntity>? specification = null,
            CancellationToken cancellationToken = default)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindListWithSpecificationAsync(specification, cancellationToken));


        public IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindPagingList(pageAction));

        public Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindPagingListAsync(pageAction, cancellationToken));


        public IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
            ISpecification<TEntity>? specification = null)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindPagingListWithSpecification(pageAction, specification));

        public Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
            ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
            where TEntity : class
            => ChainingAccessorsByException(a => a.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

        #endregion


        #region GetQueryable

        public IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression)
            => ChainingAccessorsByException(a => a.FromExpression(expression));


        public IQueryable<TEntity> GetQueryable<TEntity>()
            where TEntity : class
            => ChainingAccessorsByException(a => a.GetQueryable<TEntity>());

        public IQueryable<TEntity> GetQueryable<TEntity>(string name)
            where TEntity : class
            => ChainingAccessorsByException(a => a.GetQueryable<TEntity>(name));

        #endregion


        #region Add

        public TEntity AddIfNotExists<TEntity>(TEntity entity,
            Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
            where TEntity : class
            => BatchingAccessors(a => a.AddIfNotExists(entity, predicate, checkLocal))!;
        
        public object Add(object entity)
            => BatchingAccessors(a => a.Add(entity))!;

        public TEntity Add<TEntity>(TEntity entity)
            where TEntity : class
            => BatchingAccessors(a => a.Add(entity))!;


        public void AddRange(IEnumerable<object> entities)
            => BatchingAccessors(a => a.AddRange(entities));

        public void AddRange(params object[] entities)
            => BatchingAccessors(a => a.AddRange(entities));

        public Task AddRangeAsync(IEnumerable<object> entities,
            CancellationToken cancellationToken = default)
            => BatchingAccessors(a => a.AddRangeAsync(entities, cancellationToken))!;

        public Task AddRangeAsync(params object[] entities)
            => BatchingAccessors(a => a.AddRangeAsync(entities))!;

        #endregion


        #region Attach

        public object Attach(object entity)
            => BatchingAccessors(a => a.Attach(entity))!;

        public TEntity Attach<TEntity>(TEntity entity)
            where TEntity : class
            => BatchingAccessors(a => a.Attach(entity))!;


        public void AttachRange(params object[] entities)
            => BatchingAccessors(a => a.AttachRange(entities));

        public void AttachRange(IEnumerable<object> entities)
            => BatchingAccessors(a => a.AttachRange(entities));

        #endregion


        #region Remove

        public object Remove(object entity)
            => BatchingAccessors(a => a.Remove(entity))!;

        public TEntity Remove<TEntity>(TEntity entity)
            where TEntity : class
            => BatchingAccessors(a => a.Remove(entity))!;


        public void RemoveRange(params object[] entities)
            => BatchingAccessors(a => a.RemoveRange(entities));

        public void RemoveRange(IEnumerable<object> entities)
            => BatchingAccessors(a => a.RemoveRange(entities));

        #endregion


        #region Update

        public object Update(object entity)
            => BatchingAccessors(a => a.Update(entity))!;

        public TEntity Update<TEntity>(TEntity entity)
            where TEntity : class
            => BatchingAccessors(a => a.Update(entity))!;


        public void UpdateRange(params object[] entities)
            => BatchingAccessors(a => a.UpdateRange(entities));

        public void UpdateRange(IEnumerable<object> entities)
            => BatchingAccessors(a => a.UpdateRange(entities));

        #endregion


        #region SaveChanges

        public int SaveChanges()
            => SaveChanges(acceptAllChangesOnSuccess: true);

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (_interaction == AccessorInteraction.Read)
                throw new NotSupportedException($"{nameof(IAccessor)} in read interaction, the {nameof(SaveChanges)} is not supported.");

            return BatchingAccessors(a => a.SaveChanges(acceptAllChangesOnSuccess));
        }


        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            if (_interaction == AccessorInteraction.Read)
                throw new NotSupportedException($"{nameof(IAccessor)} in read interaction, the {nameof(SaveChangesAsync)} is not supported.");

            return BatchingAccessors(a => a.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken))!;
        }

        #endregion


        #region ISortable

        public float Priority
            => 0;


        public int CompareTo(ISortable? other)
            => Priority.CompareTo(other?.Priority ?? 0);

        #endregion


        #region IDisposable

        public void Dispose()
            => BatchingAccessors(a => a.Dispose());

        #endregion


        #region IAsyncDisposable

        public ValueTask DisposeAsync()
            => BatchingAccessors(a => a.DisposeAsync());

        #endregion

    }
}
