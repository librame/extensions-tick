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
/// 定义一个实现 <see cref="ICloneable"/> 的泛型可克隆接口。
/// </summary>
/// <typeparam name="TClone">指定的克隆类型。</typeparam>
public interface ICloneable<TClone> : ICloneable
{
    /// <summary>
    /// 创建一个泛型克隆对象（默认支持包含静态在内的所有字段和属性成员集合）。
    /// </summary>
    /// <returns>返回 <typeparamref name="TClone"/>。</returns>
    TClone CloneAs();
}
