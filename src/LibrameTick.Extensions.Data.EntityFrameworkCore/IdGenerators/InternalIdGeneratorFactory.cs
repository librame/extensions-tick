#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.IdGeneration;

internal sealed class InternalIdGeneratorFactory : IIdGeneratorFactory
{
    private readonly Dictionary<TypeNamedKey, IObjectIdGenerator> _allIdGenerators = [];


    public InternalIdGeneratorFactory(
        IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
        IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor)
    {
        if (_allIdGenerators.Count < 1)
        {
            var clock = coreOptionsMonitor.CurrentValue.Clock;
            var dataId = dataOptionsMonitor.CurrentValue.DataIdGeneration;

            var combIdType = typeof(CombIdGenerator);

            // Base: IdentificationGenerator（Sqlite 使用与 MySQL 数据库相同的排序方式）
            _allIdGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdGenerators.ForMySql)),
                CombIdGenerators.ForMySql(dataId.IdGeneration, clock));

            _allIdGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdGenerators.ForOracle)),
                CombIdGenerators.ForOracle(dataId.IdGeneration, clock));

            _allIdGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdGenerators.ForSqlServer)),
                CombIdGenerators.ForSqlServer(dataId.IdGeneration, clock));

            _allIdGenerators.Add(new TypeNamedKey<CombSnowflakeIdGenerator>(),
                new CombSnowflakeIdGenerator(dataId.IdGeneration, clock));

            _allIdGenerators.Add(new TypeNamedKey<MongoIdGenerator>(),
                new MongoIdGenerator(dataId.Mongo, dataId.IdGeneration));

            _allIdGenerators.Add(new TypeNamedKey<SnowflakeIdGenerator>(),
                new SnowflakeIdGenerator(dataId.Snowflake, dataId.IdGeneration, clock));

            if (dataId.CustomIdGenerators.Count > 0)
            {
                foreach (var custom in dataId.CustomIdGenerators)
                {
                    _allIdGenerators.Add(custom.Key, custom.Value);
                }
            }
        }
    }


    public IIdGenerator<TId> GetIdGenerator<TId>(TypeNamedKey key)
        where TId : IEquatable<TId>
        => (IIdGenerator<TId>)GetIdGenerator(key);

    public IObjectIdGenerator GetIdGenerator(TypeNamedKey key)
        => _allIdGenerators[key];

}
