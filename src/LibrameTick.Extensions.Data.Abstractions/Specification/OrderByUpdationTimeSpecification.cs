#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Specification;

/// <summary>
/// 定义一个实现 <see cref="IUpdationTime{TUpdatedTime}"/> 接口类型的降序排列规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public class OrderByUpdationTimeSpecification<T, TUpdatedTime> : BaseSpecification<T>
    where T : class, IUpdationTime<TUpdatedTime>
    where TUpdatedTime : struct
{
    /// <summary>
    /// 构造一个 <see cref="OrderByUpdationTimeSpecification{T, TUpdatedTime}"/>。
    /// </summary>
    public OrderByUpdationTimeSpecification()
    {
        SetOrderByDescending(s => s.UpdatedTime);
    }

}
