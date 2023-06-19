#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Saving;

internal sealed class InternalSaveChangesEventHandler : ISaveChangesEventHandler
{
    private ISavingContext<BaseDbContext, EntityEntry>? _savingContext;


    public void SavingChanges(IDbContext context, bool acceptAllChangesOnSuccess)
    {
        var entityEntries = ChangeEntityEntries(context);

        if (context is BaseDbContext dbContext && entityEntries is not null)
        {
            _savingContext = new InternalSavingContext(dbContext, entityEntries.AsReadOnlyCollection());

            foreach (var behavior in dbContext.DataOptions.SavingBehaviors)
            {
                behavior.Handle(_savingContext);
            }
        }
    }

    public void SavedChanges(IDbContext context, bool acceptAllChangesOnSuccess, int entitiesSavedCount)
    {
        if (_savingContext?.TryGetBehavior<InternalAuditingSavingBehavior>(out var result) == true
            && result.SavingAudits is not null)
        {
            _savingContext.DbContext.DataOptions.Audit.NotificationAction?.Invoke(result.SavingAudits);
        }
    }

    public void SaveChangesFailed(IDbContext context, bool acceptAllChangesOnSuccess, Exception exception)
    {
        //
    }


    private static IEnumerable<EntityEntry>? ChangeEntityEntries(IDbContext context)
    {
        if (context is IDbContextDependencies dependencies)
        {

#pragma warning disable EF1001 // Internal EF Core API usage.

            return dependencies.StateManager
                .GetEntriesForState(added: true, modified: true, deleted: true)
                .Select(static s => new EntityEntry(s));

#pragma warning restore EF1001 // Internal EF Core API usage.

        }

        return null;
    }

}
