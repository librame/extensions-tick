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

namespace Librame.Extensions.Data.Auditing;

sealed internal class InternalAuditingContext : IAuditingContext<EntityEntry, Audit>
{
    public InternalAuditingContext(IAuditingParser<EntityEntry, Audit> parser,
        IAuditingTracker<EntityEntry> tracker)
    {
        Parser = parser;
        Tracker = tracker;
    }


    public IAuditingParser<EntityEntry, Audit> Parser { get; }

    public IAuditingTracker<EntityEntry> Tracker { get; }


    public IEnumerable<Audit>? GetAudits(IDbContext context)
    {
        var sources = Tracker.TrackDatasForState(context);

        var audits = Parser.ParseEntities(sources);
        return audits;
    }



}
