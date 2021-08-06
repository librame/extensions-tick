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
using Librame.Extensions.Data.Accessors;
using Librame.Extensions.Data.Stores;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public class DataExtensionOptions : AbstractExtensionOptions<DataExtensionOptions>
    {
        private readonly List<IObjectIdentificationGenerator> _idGenerators
            = new List<IObjectIdentificationGenerator>();


        /// <summary>
        /// 构造一个 <see cref="DataExtensionOptions"/>。
        /// </summary>
        /// <param name="parentOptions">给定的父级 <see cref="IExtensionOptions"/>。</param>
        public DataExtensionOptions(IExtensionOptions parentOptions)
            : base(parentOptions, parentOptions.Directories)
        {
            CoreOptions = parentOptions.GetRequiredOptions<CoreExtensionOptions>();

            Access = new AccessOptions(this);

            // IdGenerators
            AddIdGenerator(new MongoIdentificationGenerator(CoreOptions.Clock));
            AddIdGenerator(new SnowflakeIdentificationGenerator(CoreOptions.MachineId,
                CoreOptions.DataCenterId, CoreOptions.Clock));
            // 异构数据源数据同步功能的标识必须使用统一的生成方案
            AddIdGenerator(CombIdentificationGenerator.ForSqlServer(CoreOptions.Clock));

            ServiceCharacteristics.AddSingleton<IIdentificationGeneratorFactory>();

            // Accessors
            ServiceCharacteristics.AddScope<IAccessorAggregator>();
            ServiceCharacteristics.AddScope<IAccessorManager>();
            ServiceCharacteristics.AddScope<IAccessorResolver>();
            ServiceCharacteristics.AddScope<IAccessorSlicer>();

            ServiceCharacteristics.AddScope<IAccessorMigrator>();
            ServiceCharacteristics.AddScope<IAccessorSeeder>();
            ServiceCharacteristics.AddScope<IAccessorInitializer>();

            // Stores
            ServiceCharacteristics.AddScope(typeof(IStore<>));
        }


        /// <summary>
        /// 核心扩展选项。
        /// </summary>
        public CoreExtensionOptions CoreOptions { get; init; }


        /// <summary>
        /// 访问选项。
        /// </summary>
        public AccessOptions Access { get; init; }

        /// <summary>
        /// 标识生成器列表集合（默认已集成 <see cref="string"/> “MongoDB”、<see cref="long"/> “雪花”、<see cref="Guid"/> “COMB for SQL Server” 等标识类型的生成器）。
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<IObjectIdentificationGenerator> IdGenerators
            => _idGenerators;


        /// <summary>
        /// 添加实现 <see cref="IIdentificationGenerator{TId}"/> 的标识生成器（推荐从 <see cref="AbstractIdentificationGenerator{TId}"/> 派生）。
        /// </summary>
        /// <param name="idGenerator">给定的 <see cref="IIdentificationGenerator{TId}"/>。</param>
        public void AddIdGenerator<TId>(IIdentificationGenerator<TId> idGenerator)
            => _idGenerators.Add(idGenerator);

    }
}
