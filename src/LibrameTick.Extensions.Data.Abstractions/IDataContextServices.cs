#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个 <see cref="IDataContext"/> 服务集合接口。
/// </summary>
public interface IDataContextServices
{
    /// <summary>
    /// 分片上下文。
    /// </summary>
    IShardingContext ShardingContext { get; }


    /// <summary>
    /// 初始连接字符串。
    /// </summary>
    string? InitialConnectionString { get; }

    /// <summary>
    /// 初始数据库名称。
    /// </summary>
    string? InitialDatabaseName { get; }

    /// <summary>
    /// 根据初始连接字符串创建的分片描述符。
    /// </summary>
    ShardingDescriptor? InitialShardingDescriptor { get; }


    /// <summary>
    /// 获取指定数据库上下文服务实例。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <returns>返回 <typeparamref name="TService"/>。</returns>
    TService GetContextService<TService>()
        where TService : class;
}
