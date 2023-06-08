#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Auditing;

sealed internal class InternalAuditingTracker : IAuditingTracker<EntityEntry>
{
    private readonly AuditOptions _options;


    public InternalAuditingTracker(IOptionsMonitor<DataExtensionOptions> optionsMonitor)
    {
        _options = optionsMonitor.CurrentValue.Audit;
    }


    public IEnumerable<EntityEntry>? TrackDatasForState(IDbContext context)
    {
        if (context is IDbContextDependencies dependencies)
        {

#pragma warning disable EF1001 // Internal EF Core API usage.

            return dependencies.StateManager
                .GetEntriesForState(_options.AddedState, _options.ModifiedState, _options.DeletedState, _options.UnchangedState)
                .Select(static s => new EntityEntry(s));

#pragma warning restore EF1001 // Internal EF Core API usage.

        }

        return null;
    }

}
