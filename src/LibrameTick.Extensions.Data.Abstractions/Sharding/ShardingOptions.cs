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

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的分片选项。
/// </summary>
public class ShardingOptions : IOptions
{
    /// <summary>
    /// 默认 <see cref="CultureInfo"/> 工厂方法。
    /// </summary>
    public Func<CultureInfo> DefaultCultureInfoFactory { get; set; } = () => CultureInfo.CurrentUICulture;

    /// <summary>
    /// 默认 <see cref="DateTime"/> 工厂方法。
    /// </summary>
    public Func<DateTime> DefaultDateTimeFactory { get; set; } = () => DateTime.Now;

    /// <summary>
    /// 默认 <see cref="DateTimeOffset"/> 工厂方法。
    /// </summary>
    public Func<DateTimeOffset> DefaultDateTimeOffsetFactory { get; set; } = () => DateTimeOffset.Now;

    /// <summary>
    /// 默认分片程序集名称工厂方法。
    /// </summary>
    public Func<Type, string> DefaultShardingAssemblyNameFactory { get; set; }
        = contextType => $"{contextType.Assembly.GetName().Name}_Sharding_{DateTime.Now.Ticks}";


    /// <summary>
    /// 附加的分片策略集合（默认已集成 <see cref="CultureInfoShardingStrategy"/>、<see cref="DateTimeShardingStrategy"/>、<see cref="DateTimeOffsetShardingStrategy"/>、<see cref="ModShardingStrategy"/> 等分片策略）。
    /// </summary>
    [JsonIgnore]
    public List<IShardingStrategy> AttachStrategies { get; init; } = [];
}
