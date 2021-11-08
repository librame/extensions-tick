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
using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Specification;

namespace Librame.Extensions.Data.Accessing;

sealed class InternalCompositeAccessor : AbstractSortable, IAccessor
{
    private readonly IAccessor[] _accessors;


    public InternalCompositeAccessor(IEnumerable<IAccessor> accessors)
    {
        _accessors = accessors.ToArray();
    }


    public string AccessorId
        => _accessors.Joining(a => a.AccessorId);

    public Type AccessorType
        => typeof(InternalCompositeAccessor);

    public AccessorDescriptor? AccessorDescriptor
        => _accessors.ChainingByException(a => a.AccessorDescriptor);


    #region IShardable

    public IShardingManager ShardingManager
        => _accessors.ChainingByException(a => a.ShardingManager);

    #endregion


    #region IConnectable<IAccessor>

    public string? CurrentConnectionString
        => _accessors.Joining(a => a.CurrentConnectionString);

    public Action<IAccessor>? ChangingAction
    {
        get => _accessors.ChainingByException(a => a.ChangingAction);
        set => _accessors.BatchingFirst(a => a.ChangingAction = value);
    }

    public Action<IAccessor>? ChangedAction
    {
        get => _accessors.ChainingByException(a => a.ChangedAction);
        set => _accessors.BatchingFirst(a => a.ChangedAction = value);
    }


    public IAccessor ChangeConnection(string newConnectionString)
        => _accessors.Batching(a => a.ChangeConnection(newConnectionString)).First();

    public bool TryCreateDatabase()
        => _accessors.BatchingFirstWithTransaction(a => a.TryCreateDatabase());

    public Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
        => _accessors.BatchingFirstWithTransaction(a => a.TryCreateDatabaseAsync(cancellationToken));

    #endregion


    #region ISaveChangeable

    public int SaveChanges()
        => SaveChanges(acceptAllChangesOnSuccess: true);

    public int SaveChanges(bool acceptAllChangesOnSuccess)
        => _accessors.BatchingFirstWithTransaction(a => a.SaveChanges(acceptAllChangesOnSuccess));


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

    public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
        => _accessors.BatchingFirstWithTransaction(a => a.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken))!;

    #endregion


    #region IDisposable

    public void Dispose()
        => _accessors.Batching(a => a.Dispose());

    #endregion


    #region IAsyncDisposable

    public ValueTask DisposeAsync()
        => _accessors.BatchingFirst(a => a.DisposeAsync());

    #endregion


    #region Exists

    public bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true)
        where TEntity : class
        => _accessors.ChainingByException(a => a.Exists(predicate));

    public Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class
        => _accessors.ChainingByException(a => a.ExistsAsync(predicate, checkLocal, cancellationToken));

    #endregion


    #region Find

    public TEntity? Find<TEntity>(params object?[]? keyValues)
        where TEntity : class
        => _accessors.ChainingByException(a => a.Find<TEntity>(keyValues));

    public object? Find(Type entityType, params object?[]? keyValues)
        => _accessors.ChainingByException(a => a.Find(entityType, keyValues));

    public ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindAsync<TEntity>(keyValues, cancellationToken));

    public ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues)
        => _accessors.ChainingByException(a => a.FindAsync(entityType, keyValues));

    public ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken)
        => _accessors.ChainingByException(a => a.FindAsync(entityType, keyValues, cancellationToken));

    public ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindAsync<TEntity>(keyValues));


    public IList<TEntity> FindListWithSpecification<TEntity>(IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindListWithSpecification(specification));

    public Task<IList<TEntity>> FindListWithSpecificationAsync<TEntity>(IEntitySpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindListWithSpecificationAsync(specification, cancellationToken));


    public IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindPagingList(pageAction));

    public Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindPagingListAsync(pageAction, cancellationToken));


    public IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindPagingListWithSpecification(pageAction, specification));

    public Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
        => _accessors.ChainingByException(a => a.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

    #endregion


    #region GetQueryable

    public IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression)
        => _accessors.ChainingByException(a => a.FromExpression(expression));


    public IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class
        => _accessors.ChainingByException(a => a.GetQueryable<TEntity>());

    public IQueryable<TEntity> GetQueryable<TEntity>(string name)
        where TEntity : class
        => _accessors.ChainingByException(a => a.GetQueryable<TEntity>(name));

    #endregion


    #region Add

    public TEntity AddIfNotExists<TEntity>(TEntity entity,
        Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class
        => _accessors.BatchingFirstWithTransaction(a => a.AddIfNotExists(entity, predicate, checkLocal))!;
        
    public object Add(object entity)
        => _accessors.BatchingFirstWithTransaction(a => a.Add(entity))!;

    public TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
        => _accessors.BatchingFirstWithTransaction(a => a.Add(entity))!;


    public void AddRange(IEnumerable<object> entities)
        => _accessors.BatchingWithTransaction(a => a.AddRange(entities));

    public void AddRange(params object[] entities)
        => _accessors.BatchingWithTransaction(a => a.AddRange(entities));

    public Task AddRangeAsync(IEnumerable<object> entities,
        CancellationToken cancellationToken = default)
        => _accessors.BatchingFirstWithTransaction(a => a.AddRangeAsync(entities, cancellationToken))!;

    public Task AddRangeAsync(params object[] entities)
        => _accessors.BatchingFirstWithTransaction(a => a.AddRangeAsync(entities))!;

    #endregion


    #region Attach

    public object Attach(object entity)
        => _accessors.BatchingFirstWithTransaction(a => a.Attach(entity))!;

    public TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
        => _accessors.BatchingFirstWithTransaction(a => a.Attach(entity))!;


    public void AttachRange(params object[] entities)
        => _accessors.BatchingWithTransaction(a => a.AttachRange(entities));

    public void AttachRange(IEnumerable<object> entities)
        => _accessors.BatchingWithTransaction(a => a.AttachRange(entities));

    #endregion


    #region Remove

    public object Remove(object entity)
        => _accessors.BatchingFirstWithTransaction(a => a.Remove(entity))!;

    public TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
        => _accessors.BatchingFirstWithTransaction(a => a.Remove(entity))!;


    public void RemoveRange(params object[] entities)
        => _accessors.BatchingWithTransaction(a => a.RemoveRange(entities));

    public void RemoveRange(IEnumerable<object> entities)
        => _accessors.BatchingWithTransaction(a => a.RemoveRange(entities));

    #endregion


    #region Update

    public object Update(object entity)
        => _accessors.BatchingFirstWithTransaction(a => a.Update(entity))!;

    public TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
        => _accessors.BatchingFirstWithTransaction(a => a.Update(entity))!;


    public void UpdateRange(params object[] entities)
        => _accessors.BatchingWithTransaction(a => a.UpdateRange(entities));

    public void UpdateRange(IEnumerable<object> entities)
        => _accessors.BatchingWithTransaction(a => a.UpdateRange(entities));

    #endregion

}
