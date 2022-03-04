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
using Librame.Extensions.IdGenerators;

namespace Librame.Extensions.Data;

class InternalIdGeneratorFactory : IIdGeneratorFactory
{
    private readonly Dictionary<TypeNamedKey, IObjectIdGenerator> _idGenerators = new();


    public InternalIdGeneratorFactory(
        IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
        IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor)
    {
        if (_idGenerators.Count < 1)
        {
            var idOptions = dataOptionsMonitor.CurrentValue.IdGeneration;
            var clock = coreOptionsMonitor.CurrentValue.Clock;
            var locker = coreOptionsMonitor.CurrentValue.Locker;
            var combIdType = typeof(CombIdGenerator);

            // Base: IdentificationGenerator（Sqlite 使用与 MySQL 数据库相同的排序方式）
            _idGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdGenerators.ForMySql)),
                CombIdGenerators.ForMySql(clock, locker));

            _idGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdGenerators.ForOracle)),
                CombIdGenerators.ForOracle(clock, locker));

            _idGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdGenerators.ForSqlServer)),
                CombIdGenerators.ForSqlServer(clock, locker));

            _idGenerators.Add(new TypeNamedKey<CombSnowflakeIdGenerator>(),
                new CombSnowflakeIdGenerator(clock, locker, idOptions));

            _idGenerators.Add(new TypeNamedKey<MongoIdGenerator>(),
                new MongoIdGenerator(clock));

            _idGenerators.Add(new TypeNamedKey<SnowflakeIdGenerator>(),
                new SnowflakeIdGenerator(clock, locker, idOptions));

            if (dataOptionsMonitor.CurrentValue.IdGenerators.Count > 0)
            {
                foreach (var idGenerator in dataOptionsMonitor.CurrentValue.IdGenerators)
                {
                    _idGenerators.Add(idGenerator.Key, idGenerator.Value);
                }
            }
        }
    }


    public IIdGenerator<TId> GetIdGenerator<TId>(TypeNamedKey key)
        where TId : IEquatable<TId>
        => (IIdGenerator<TId>)GetIdGenerator(key);

    public IObjectIdGenerator GetIdGenerator(TypeNamedKey key)
        => _idGenerators[key];

}
