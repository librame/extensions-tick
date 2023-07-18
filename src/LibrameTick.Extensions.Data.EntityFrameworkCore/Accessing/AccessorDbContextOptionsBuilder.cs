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
using Librame.Extensions.Crypto;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Device;
using Librame.Extensions.Dispatchers;

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
        _relationalOptionsExtension = parentBuilder.Options.Extensions
            .WhereAre<IDbContextOptionsExtension, RelationalOptionsExtension>(p => p.Info.IsDatabaseProvider)?.FirstOrDefault();

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
    /// <remarks>
    /// <para>给存取器命名，名称必需唯一，通常在使用命名规约存取器获取指定存取器进行存取操作时有效。</para>
    /// </remarks>
    /// <param name="name">给定的所属群组。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithName(string name)
        => WithOption(e => e.WithName(name));

    /// <summary>
    /// 配置所属群组。
    /// </summary>
    /// <remarks>
    /// <para>将多个存取器划分为一组，同组中可设置不同的访问模式与调度模式。</para>
    /// </remarks>
    /// <param name="group">给定的所属群组。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithGroup(int group)
        => WithOption(e => e.WithGroup(group));

    /// <summary>
    /// 配置所属分区。
    /// </summary>
    /// <remarks>
    /// <para>当调度模式为 <see cref="DispatchingMode.Striping"/> 时有效，通常从 1、2... 开始编号（总数不超过同群组存取器个数），实体则按特定算法取存取器个数的模进行存取操作。</para>
    /// </remarks>
    /// <param name="partition">给定的所属分区。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithPartition(int partition)
        => WithOption(e => e.WithPartition(partition));

    /// <summary>
    /// 配置访问模式（默认为 <see cref="AccessMode.ReadWrite"/>）。
    /// </summary>
    /// <remarks>
    /// <para>将同组的多个存取器划分为相同或不同的访问模式，可分别实现读、写、读/写等同读写或读写分离模式。</para>
    /// </remarks>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithAccess(AccessMode access)
        => WithOption(e => e.WithAccess(access));

    /// <summary>
    /// 配置调度模式（默认为 <see cref="DispatchingMode.Mirroring"/>）。
    /// </summary>
    /// <remarks>
    /// <para>将同组的多个存取器设定为不同的调度模式，即可调用对应的调度器分别实现默认、镜像、条带（即分片）、复合等存取操作。</para>
    /// </remarks>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithDispatching(DispatchingMode mode)
        => WithOption(e => e.WithDispatching(mode));

    /// <summary>
    /// 配置存取器优先级（默认为 <see cref="float.Tau"/>，但会优先使用存取器实现的 <see cref="IPriorable{T}.GetPriority()"/> 的属性值）。
    /// </summary>
    /// <remarks>
    /// <para>为同组的多个存取器设定不同的优先级，配合调度模式可实现优先或负载使用存取器进行存取操作。</para>
    /// </remarks>
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
    /// 分片规则特性。
    /// </summary>
    /// <typeparam name="TStrategy">指定的分片策略类型。</typeparam>
    /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="configureAction">给定的分片命名特性配置动作（可选）。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithSharding<TStrategy>(string suffixFormatter,
        Action<ShardingAttribute>? configureAction = null)
        => WithSharding(suffixFormatter, configureAction, typeof(TStrategy));

    /// <summary>
    /// 分片规则特性。
    /// </summary>
    /// <remarks>
    /// <para>如果要使用分库，此为必选项，可设置分库的规则。</para>
    /// </remarks>
    /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithSharding(string suffixFormatter, params Type[] strategyTypes)
        => WithSharding(suffixFormatter, configureAction: null, strategyTypes);

    /// <summary>
    /// 分片规则特性。
    /// </summary>
    /// <remarks>
    /// <para>如果要使用分库，此为必选项，可设置分库的规则。</para>
    /// </remarks>
    /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="configureAction">给定的分片命名特性配置动作。</param>
    /// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithSharding(string suffixFormatter,
        Action<ShardingAttribute>? configureAction, params Type[] strategyTypes)
    {
        var attribute = ShardingAttribute.ParseFromConnectionString(ParentBuilder.Options.ContextType,
            suffixFormatter, strategyTypes, _relationalOptionsExtension?.ConnectionString);

        configureAction?.Invoke(attribute);

        return WithSharding(attribute);
    }

    /// <summary>
    /// 分片规则特性。
    /// </summary>
    /// <remarks>
    /// <para>如果要使用分库，此为必选项，可设置分库的规则。</para>
    /// </remarks>
    /// <param name="sharding">给定的 <see cref="ShardingAttribute"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithSharding(ShardingAttribute sharding)
        => WithOption(e => e.WithSharding(sharding));


    /// <summary>
    /// 配置本机负载器。
    /// </summary>
    /// <remarks>
    /// <para>当调度模式为 <see cref="DispatchingMode.Mirroring"/> 时有效。</para>
    /// </remarks>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithLocalhostLoader()
        => WithLoader(nameof(DeviceLoadOptions.Localhost));

    /// <summary>
    /// 配置负载器。
    /// </summary>
    /// <remarks>
    /// <para>当调度模式为 <see cref="DispatchingMode.Mirroring"/> 时有效，可配置获取服务器负载信息的主机名（本机用 <see cref="DeviceLoadOptions.Localhost"/> 表示，远程主机需返回 <see cref="DeviceUsageDescriptor"/> 的 JSON 形式），详情参见 <see cref="AccessorDeviceLoader"/>。</para>
    /// </remarks>
    /// <param name="host">给定的负载器主机。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    public virtual AccessorDbContextOptionsBuilder WithLoader(string host)
        => WithOption(e => e.WithLoader(host));


    ///// <summary>
    ///// 配置存取器类型（通常不需要修改）。
    ///// </summary>
    ///// <param name="accessorType">给定的存取器类型。</param>
    ///// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
    //public virtual AccessorDbContextOptionsBuilder WithAccessorType(Type accessorType)
    //    => WithOption(e => e.WithAccessorType(accessorType));


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
