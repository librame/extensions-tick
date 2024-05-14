#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

///// <summary>
///// 定义一个实现 <see cref="IPriorable{Single}"/> 的可优先接口。
///// </summary>
//public interface IPriorable : IPriorable<float>
//{
//}


///// <summary>
///// 定义一个实现 <see cref="IComparable{T}"/> 的可优先接口。
///// </summary>
///// <typeparam name="T">指定的优先级类型。</typeparam>
//public interface IPriorable<T> : IComparable<IPriorable<T>>
//    where T : IComparable<T>
//{
//    /// <summary>
//    /// 获取优先级。
//    /// </summary>
//    /// <returns>返回 <typeparamref name="T"/>。</returns>
//    T GetPriority();

//    /// <summary>
//    /// 设置优先级。
//    /// </summary>
//    /// <param name="newPriority">给定的新优先级。</param>
//    /// <returns>返回 <typeparamref name="T"/>。</returns>
//    T SetPriority(T newPriority);

//    /// <summary>
//    /// 比较实例。
//    /// </summary>
//    /// <param name="other">给定的 <see cref="IPriorable{T}"/>。</param>
//    /// <returns>返回整数。</returns>
//    int IComparable<IPriorable<T>>.CompareTo(IPriorable<T>? other)
//        => other?.GetPriority().CompareTo(GetPriority()) ?? 0;

//}
