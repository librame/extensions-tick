#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Storing;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;

namespace Librame.Extensions.Data.Accessing
{
    // EFCore 的拦截器既支持使用 IInterceptor 进行集合注册，也支持在 DbContextOptionsBuilder.AddInterceptors(IInterceptor[]) 注册。
    class InternalAccessorSaveChangesInterceptor : ISaveChangesInterceptor
    {
        private readonly DataExtensionOptions _dataOptions;
        private readonly IAuditingManager _auditingManager;

        private IReadOnlyList<Audit>? _audits;


        public InternalAccessorSaveChangesInterceptor(DataExtensionOptions dataOptions,
            IAuditingManager auditingManager)
        {
            _dataOptions = dataOptions;
            _auditingManager = auditingManager;
        }


        public InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context == null)
                throw new ArgumentNullException($"{nameof(eventData)}.{eventData.Context}");

#pragma warning disable EF1001 // Internal EF Core API usage.

            var entityEntries = ((IDbContextDependencies)eventData.Context!).StateManager
                .GetEntriesForState(_dataOptions.Auditing.AddedState, _dataOptions.Auditing.ModifiedState,
                    _dataOptions.Auditing.DeletedState, _dataOptions.Auditing.UnchangedState);

            _audits = _auditingManager.GetAudits(entityEntries.Select(s => new EntityEntry(s)));

#pragma warning restore EF1001 // Internal EF Core API usage.

            if (_audits.Count > 0 && _dataOptions.Auditing.SaveAudits
                && eventData.Context is IDataAccessor accessor)
            {
                accessor.Audits.AddRange(_audits);
            }

            return result;
        }

        public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
            => cancellationToken.RunValueTask(() => SavingChanges(eventData, result));


        public int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            if (_audits != null)
                _dataOptions.Auditing.NotificationAction?.Invoke(_audits);

            return result;
        }

        public ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
            int result, CancellationToken cancellationToken = default)
            => cancellationToken.RunValueTask(() => SavedChanges(eventData, result));


        public void SaveChangesFailed(DbContextErrorEventData eventData)
        {
        }

        public Task SaveChangesFailedAsync(DbContextErrorEventData eventData,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

    }
}
