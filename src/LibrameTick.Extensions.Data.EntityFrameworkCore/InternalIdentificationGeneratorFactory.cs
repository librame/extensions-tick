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

namespace Librame.Extensions.Data;

class InternalIdentificationGeneratorFactory : IIdentificationGeneratorFactory
{
    private readonly Dictionary<TypeNamedKey, IObjectIdentificationGenerator> _idGenerators = new();


    public InternalIdentificationGeneratorFactory(IOptionsMonitor<DataExtensionOptions> dataOptions,
        IOptionsMonitor<CoreExtensionOptions> coreOptions)
    {
        if (_idGenerators.Count < 1)
        {
            var idOptions = dataOptions.CurrentValue.IdGeneration;
            var clock = coreOptions.CurrentValue.Clock;
            var locker = coreOptions.CurrentValue.Locker;
            var combIdType = typeof(CombIdentificationGenerator);

            // Base: IdentificationGenerator（Sqlite 使用与 MySQL 数据库相同的排序方式）
            _idGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdentificationGenerators.ForMySql)),
                CombIdentificationGenerators.ForMySql(clock, locker));

            _idGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdentificationGenerators.ForOracle)),
                CombIdentificationGenerators.ForOracle(clock, locker));

            _idGenerators.Add(new TypeNamedKey(combIdType, nameof(CombIdentificationGenerators.ForSqlServer)),
                CombIdentificationGenerators.ForSqlServer(clock, locker));

            _idGenerators.Add(new TypeNamedKey<CombSnowflakeIdentificationGenerator>(),
                new CombSnowflakeIdentificationGenerator(clock, locker, idOptions));

            _idGenerators.Add(new TypeNamedKey<MongoIdentificationGenerator>(),
                new MongoIdentificationGenerator(clock));

            _idGenerators.Add(new TypeNamedKey<SnowflakeIdentificationGenerator>(),
                new SnowflakeIdentificationGenerator(clock, locker, idOptions));

            if (dataOptions.CurrentValue.IdGenerators.Count > 0)
            {
                foreach (var idGenerator in dataOptions.CurrentValue.IdGenerators)
                {
                    _idGenerators.Add(idGenerator.Key, idGenerator.Value);
                }
            }
        }
    }


    public IIdentificationGenerator<TId> GetIdGenerator<TId>(TypeNamedKey key)
        where TId : IEquatable<TId>
        => (IIdentificationGenerator<TId>)GetIdGenerator(key);

    public IObjectIdentificationGenerator GetIdGenerator(TypeNamedKey key)
        => _idGenerators[key];

}
