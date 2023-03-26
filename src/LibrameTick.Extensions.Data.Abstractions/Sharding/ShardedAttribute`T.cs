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
/// 定义用于分片且指定分片策略的命名特性（构成方式为：BaseName+Suffix，可用于分库、分表操作）。
/// </summary>
/// <typeparam name="TStrategy">指定的分片策略类型。</typeparam>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ShardedAttribute<TStrategy> : ShardedAttribute
{
    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute{TStrategy}"/>。
    /// </summary>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    public ShardedAttribute(string suffix)
        : base(suffix, typeof(TStrategy), baseName: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ShardedAttribute{TStrategy}"/>。
    /// </summary>
    /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
    /// <param name="baseName">给定的基础名称（可空；针对分表默认使用实体名称复数，针对数据库默认使用数据库名）。</param>
    public ShardedAttribute(string suffix, string? baseName)
        : base(suffix, typeof(TStrategy), baseName)
    {
    }

}
