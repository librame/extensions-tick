#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

internal sealed class InternalShardingFinder : IShardingFinder
{
    private readonly ConcurrentDictionary<Type, IReadOnlyList<ShardingDescriptor>?> _cacheTables = new();


    private static DataContext GetRealDataContext(IDataContext context)
    {
        if (context is not DataContext dataContext)
        {
            // 不支持的数据库上下文类型，默认仅支持 DbContext 及其派生类型。
            throw new NotSupportedException($"Unsupported data context type '{context.ContextType}', only DbContext and its derived types are supported by default.");
        }

        return dataContext;
    }


    public IReadOnlyList<ShardingDescriptor>? FindTables(IDataContext context)
    {
        if (!_cacheTables.TryGetValue(context.ContextType, out var result))
        {
            var dbContext = GetRealDataContext(context);
            var descriptors = FindTableDescriptorsNonCachedFromEntities(dbContext);

            if (descriptors.Length > 0)
            {
                _cacheTables[context.ContextType] = result = descriptors;
            }
            else
            {
                _cacheTables[context.ContextType] = null;
            }
        }

        return result;
    }


    private static ShardingDescriptor[] FindTableDescriptorsNonCachedFromEntities(DataContext context)
    {
        var descrs = context.Model.GetEntityTypes()
            .SelectWithoutNull(p => p.GetShardingDescriptor(context.CurrentServices.ShardingContext.StrategyProvider))
            .ToArray();

        return descrs;
    }

}
