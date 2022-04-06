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
using Librame.Extensions.Data.Specifications;
using Librame.Extensions.Equilizers;

namespace Librame.Extensions.Data.Accessing;

sealed class InternalCompositeAccessor : AbstractSortable, IAccessor
{
    private readonly IEquilizer<IAccessor> _equilizer;


    public InternalCompositeAccessor(IEnumerable<IAccessor> accessors)
    {
        _equilizer = new TransactionEquilizer<IAccessor>(accessors);
    }


    public string AccessorId
        => _equilizer.Invoke(a => a.AccessorId).JoinString(',');

    public Type AccessorType
        => typeof(InternalCompositeAccessor);

    public AccessorDescriptor? AccessorDescriptor
        => null;


    #region IShardable

    public IShardingManager ShardingManager
        => _equilizer.First().ShardingManager;

    #endregion


    #region IConnectable<IAccessor>

    public string? CurrentConnectionString
        => _equilizer.Invoke(a => a.CurrentConnectionString).JoinString(',');

    public Action<IAccessor>? ChangingAction
    {
        get => _equilizer.Invoke(a => a.ChangingAction).First();
        set => _equilizer.Invoke(a => a.ChangingAction = value);
    }

    public Action<IAccessor>? ChangedAction
    {
        get => _equilizer.Invoke(a => a.ChangedAction).First();
        set => _equilizer.Invoke(a => a.ChangedAction = value);
    }


    public IAccessor ChangeConnection(string newConnectionString)
        => _equilizer.Invoke(a => a.ChangeConnection(newConnectionString)).First();

    public bool TryCreateDatabase()
        => _equilizer.Invoke(a => a.TryCreateDatabase()).First();

    public Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
        => _equilizer.Invoke(a => a.TryCreateDatabaseAsync(cancellationToken)).First();

    #endregion


    #region ISaveChangeable

    public int SaveChanges()
        => SaveChanges(acceptAllChangesOnSuccess: true);

    public int SaveChanges(bool acceptAllChangesOnSuccess)
        => _equilizer.Invoke(a => a.SaveChanges(acceptAllChangesOnSuccess)).First();


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

    public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
        => _equilizer.Invoke(a => a.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)).First();

    #endregion


    #region IDisposable

    public void Dispose()
        => _equilizer.Invoke(a => a.Dispose());

    #endregion


    #region IAsyncDisposable

    public ValueTask DisposeAsync()
        => _equilizer.Invoke(a => a.DisposeAsync()).First();

    #endregion


    #region ExecuteCommand

    public bool ExecuteSuccess(string sql, DbParameter[]? parameters = null)
        => _equilizer.Invoke(a => a.ExecuteSuccess(sql, parameters)).First();

    public object? ExecuteScalar(string sql, DbParameter[]? parameters = null)
        => _equilizer.Invoke(a => a.ExecuteScalar(sql, parameters)).First();

    public IList<TEntity>? ExecuteList<TEntity>(string sql, DbParameter[]? parameters = null)
        where TEntity : class
        => _equilizer.Invoke(a => a.ExecuteList<TEntity>(sql, parameters)).First();


    public Task<bool> ExecuteSuccessAsync(string sql, DbParameter[]? parameters = null,
        CancellationToken cancellationToken = default)
        => _equilizer.Invoke(a => a.ExecuteSuccessAsync(sql, parameters, cancellationToken)).First();

    public Task<object?> ExecuteScalarAsync(string sql, DbParameter[]? parameters = null,
        CancellationToken cancellationToken = default)
        => _equilizer.Invoke(a => a.ExecuteScalarAsync(sql, parameters, cancellationToken)).First();

    public Task<List<TEntity>?> ExecuteListAsync<TEntity>(string sql, DbParameter[]? parameters = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => _equilizer.Invoke(a => a.ExecuteListAsync<TEntity>(sql, parameters, cancellationToken)).First();

    #endregion


    #region Exists

    public bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true)
        where TEntity : class
        => _equilizer.Invoke(a => a.Exists(predicate)).First();

    public Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class
        => _equilizer.Invoke(a => a.ExistsAsync(predicate, checkLocal, cancellationToken)).First();

    #endregion


    #region Find

    public TEntity? Find<TEntity>(params object?[]? keyValues)
        where TEntity : class
        => _equilizer.Invoke(a => a.Find<TEntity>(keyValues)).First();

    public object? Find(Type entityType, params object?[]? keyValues)
        => _equilizer.Invoke(a => a.Find(entityType, keyValues));

    public ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindAsync<TEntity>(keyValues, cancellationToken)).First();

    public ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues)
        => _equilizer.Invoke(a => a.FindAsync(entityType, keyValues)).First();

    public ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken)
        => _equilizer.Invoke(a => a.FindAsync(entityType, keyValues, cancellationToken)).First();

    public ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindAsync<TEntity>(keyValues)).First();


    public IList<TEntity> FindListWithSpecification<TEntity>(IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindListWithSpecification(specification)).First();

    public Task<IList<TEntity>> FindListWithSpecificationAsync<TEntity>(IEntitySpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindListWithSpecificationAsync(specification, cancellationToken)).First();


    public IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindPagingList(pageAction)).First();

    public Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindPagingListAsync(pageAction, cancellationToken)).First();


    public IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindPagingListWithSpecification(pageAction, specification)).First();

    public Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
        => _equilizer.Invoke(a => a.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken)).First();

    #endregion


    #region GetQueryable

    public IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression)
        => _equilizer.Invoke(a => a.FromExpression(expression)).First();


    public IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class
        => _equilizer.Invoke(a => a.GetQueryable<TEntity>()).First();

    public IQueryable<TEntity> GetQueryable<TEntity>(string name)
        where TEntity : class
        => _equilizer.Invoke(a => a.GetQueryable<TEntity>(name)).First();

    #endregion


    #region Add

    public TEntity AddIfNotExists<TEntity>(TEntity entity,
        Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class
        => _equilizer.Invoke(a => a.AddIfNotExists(entity, predicate, checkLocal)).First();
        
    public object Add(object entity)
        => _equilizer.Invoke(a => a.Add(entity))!;

    public TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
        => _equilizer.Invoke(a => a.Add(entity)).First();


    public void AddRange(IEnumerable<object> entities)
        => _equilizer.Invoke(a => a.AddRange(entities));

    public void AddRange(params object[] entities)
        => _equilizer.Invoke(a => a.AddRange(entities));

    public Task AddRangeAsync(IEnumerable<object> entities,
        CancellationToken cancellationToken = default)
        => _equilizer.Invoke(a => a.AddRangeAsync(entities, cancellationToken)).First();

    public Task AddRangeAsync(params object[] entities)
        => _equilizer.Invoke(a => a.AddRangeAsync(entities)).First();

    #endregion


    #region Attach

    public object Attach(object entity)
        => _equilizer.Invoke(a => a.Attach(entity))!;

    public TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
        => _equilizer.Invoke(a => a.Attach(entity)).First();


    public void AttachRange(params object[] entities)
        => _equilizer.Invoke(a => a.AttachRange(entities));

    public void AttachRange(IEnumerable<object> entities)
        => _equilizer.Invoke(a => a.AttachRange(entities));

    #endregion


    #region Remove

    public object Remove(object entity)
        => _equilizer.Invoke(a => a.Remove(entity))!;

    public TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
        => _equilizer.Invoke(a => a.Remove(entity)).First();


    public void RemoveRange(params object[] entities)
        => _equilizer.Invoke(a => a.RemoveRange(entities));

    public void RemoveRange(IEnumerable<object> entities)
        => _equilizer.Invoke(a => a.RemoveRange(entities));

    #endregion


    #region Update

    public object Update(object entity)
        => _equilizer.Invoke(a => a.Update(entity))!;

    public TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
        => _equilizer.Invoke(a => a.Update(entity)).First();


    public void UpdateRange(params object[] entities)
        => _equilizer.Invoke(a => a.UpdateRange(entities));

    public void UpdateRange(IEnumerable<object> entities)
        => _equilizer.Invoke(a => a.UpdateRange(entities));

    #endregion

}
