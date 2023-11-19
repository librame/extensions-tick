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
/// 定义一个 <see cref="IDataContext"/> 数据上下文的依赖集合接口。
/// </summary>
public interface IDataContextDependencies
{
    /// <summary>
    /// 分片上下文。
    /// </summary>
    IShardingContext ShardingContext { get; }


    /// <summary>
    /// 获取指定服务实例。
    /// </summary>
    /// <typeparam name="TService">指定的服务类型。</typeparam>
    /// <returns>返回 <typeparamref name="TService"/>。</returns>
    TService GetScopeService<TService>()
        where TService : class;
}
