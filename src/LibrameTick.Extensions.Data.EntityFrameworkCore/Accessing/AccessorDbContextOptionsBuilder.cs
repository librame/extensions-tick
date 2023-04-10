﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Cryptography;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义存取器的 <see cref="DbContextOptionsBuilder"/>。
/// </summary>
public class AccessorDbContextOptionsBuilder
{
    //private readonly CoreOptionsExtension? _coreOptionsExtension;
    private readonly RelationalOptionsExtension? _relationalOptionsExtension;


    /// <summary>
    /// 构造一个 <see cref="AccessorDbContextOptionsBuilder"/>。
    /// </summary>
    /// <param name="parentBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    public AccessorDbContextOptionsBuilder(DbContextOptionsBuilder parentBuilder)
    {
        ParentBuilder = parentBuilder;

        //_coreOptionsExtension = parentBuilder.Options.FindExtension<CoreOptionsExtension>();
        _relationalOptionsExtension = parentBuilder.Options.LikeExtensions<RelationalOptionsExtension>()?.FirstOrDefault();

        //if (_coreOptionsExtension?.MaxPoolSize > 0)
        //    WithOption(e => e.WithPooling(true));
    }


    /// <summary>
    /// 父级 <see cref="DbContextOptionsBuilder"/>。
    /// </summary>
    protected virtual DbContextOptionsBuilder ParentBuilder { get; }


    /// <summary>
    /// 配置存取器名称。
    /// </summary>
    /// <param name="name">给定的所属群组。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithName(string name)
        => WithOption(e => e.WithName(name));

    /// <summary>
    /// 配置存取器所属群组（默认为 0，表示多个存取器划分为一组，同组意味着具有相同的增、删、改等操作；如果不需要改变，可不调用此方法）。
    /// </summary>
    /// <param name="group">给定的所属群组。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithGroup(int group)
        => WithOption(e => e.WithGroup(group));

    /// <summary>
    /// 配置存取器交互方式（默认为读/写；如果不需要改变，可不调用此方法）。
    /// </summary>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithAccess(AccessMode access)
        => WithOption(e => e.WithAccess(access));

    /// <summary>
    /// 配置存取器优先级（默认使用 <see cref="IAccessor"/> 定义的优先级属性值；如果不需要改变，可不调用此方法）。
    /// </summary>
    /// <param name="priority">给定的存取器优先级（数值越小越优先）。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithPriority(float priority)
        => WithOption(e => e.WithPriority(priority));

    /// <summary>
    /// 配置算法选项（默认为空表示使用核心模块的算法选项，详情参见 <see cref="CoreExtensionOptions.Algorithm"/>）。
    /// </summary>
    /// <param name="algorithm">给定的 <see cref="AlgorithmOptions"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithAlgorithm(AlgorithmOptions algorithm)
        => WithOption(e => e.WithAlgorithm(algorithm));


    /// <summary>
    /// 配置分片特性。
    /// </summary>
    /// <typeparam name="TStrategy">指定的分库策略类型。</typeparam>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="configureAction">给定的分片命名特性配置动作（可选）。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithSharding<TStrategy>(string suffix,
        Action<ShardedAttribute>? configureAction = null)
        where TStrategy : IShardingStrategy
        => WithSharding(typeof(TStrategy), suffix, configureAction);

    /// <summary>
    /// 配置分片特性。
    /// </summary>
    /// <param name="strategyType">给定的策略类型。</param>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="configureAction">给定的分片命名特性配置动作（可选）。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithSharding(Type strategyType,
        string suffix, Action<ShardedAttribute>? configureAction = null)
    {
        var attribute = ShardedAttribute.ParseFromConnectionString(strategyType,
            suffix, _relationalOptionsExtension?.ConnectionString);

        configureAction?.Invoke(attribute);

        return WithSharding(attribute);
    }

    /// <summary>
    /// 配置分片特性。
    /// </summary>
    /// <param name="sharded">给定的 <see cref="ShardedAttribute"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithSharding(ShardedAttribute sharded)
        => WithOption(e => e.WithSharding(sharded));


    /// <summary>
    /// 配置存取器服务类型（通常不需要修改）。
    /// </summary>
    /// <param name="serviceType">给定的存取器服务类型。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithServiceType(Type serviceType)
        => WithOption(e => e.WithServiceType(serviceType));


    /// <summary>
    /// 使用指定的函数更新选项扩展。
    /// </summary>
    /// <param name="withFunc">给定的要更新选项的函数。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    protected virtual AccessorDbContextOptionsBuilder WithOption(
        Func<AccessorDbContextOptionsExtension, AccessorDbContextOptionsExtension> withFunc)
    {
        ParentBuilder.AddOrUpdateExtension(withFunc);
        return this;
    }

}
