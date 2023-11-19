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
/// 定义一个可处理 <see cref="IDataContext"/> 的分片初始化器接口。
/// </summary>
public interface IShardingInitializer
{
    /// <summary>
    /// 初始化数据上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    void Initialize(IDataContext context);
}
