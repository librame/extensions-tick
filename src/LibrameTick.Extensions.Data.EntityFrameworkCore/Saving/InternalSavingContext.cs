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

internal sealed class InternalSavingContext : ISavingContext<BaseDbContext, EntityEntry>
{
    private readonly ConcurrentDictionary<Type, ISavingBehavior<BaseDbContext, EntityEntry>> _behaviors;


    public InternalSavingContext(BaseDbContext dbContext, IReadOnlyCollection<EntityEntry> changeEntries)
    {
        _behaviors = new ConcurrentDictionary<Type, ISavingBehavior<BaseDbContext, EntityEntry>>();

        DbContext = dbContext;
        ChangeEntries = changeEntries;
    }


    public BaseDbContext DbContext { get; init; }

    public IReadOnlyCollection<EntityEntry> ChangeEntries { get; init; }


    public TBehavior AddOrUpdateBehavior<TBehavior>(TBehavior behavior)
        where TBehavior : ISavingBehavior<BaseDbContext, EntityEntry>
    {
        AddOrUpdateBehavior(typeof(TBehavior), behavior);
        return behavior;
    }

    public ISavingBehavior<BaseDbContext, EntityEntry> AddOrUpdateBehavior(Type behaviorType,
        ISavingBehavior<BaseDbContext, EntityEntry> behavior)
        => _behaviors.AddOrUpdate(behaviorType, key => behavior, (key, oldValue) => behavior);


    public bool TryGetBehavior<TBehavior>([MaybeNullWhen(false)] out TBehavior result)
        where TBehavior : ISavingBehavior<BaseDbContext, EntityEntry>
    {
        var exists = _behaviors.TryGetValue(typeof(TBehavior), out var behavior);
        result = behavior.As<TBehavior>();

        return exists;
    }

    public bool TryGetBehavior(Type behaviorType,
        [MaybeNullWhen(false)] out ISavingBehavior<BaseDbContext, EntityEntry> result)
        => _behaviors.TryGetValue(behaviorType, out result);

}
