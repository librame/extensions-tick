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

/// <summary>
/// 定义一个继承 <see cref="ICloneable"/> 的泛型可克隆接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public interface ICloneable<TSource> : ICloneable
{
    /// <summary>
    /// 创建一个泛型克隆对象（默认支持包含静态在内的所有字段和属性成员集合）。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSource"/>。</returns>
    TSource CloneAs();
}
