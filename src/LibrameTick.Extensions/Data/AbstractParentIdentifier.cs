#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Resources;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义抽象实现 <see cref="IParentIdentifier{TId}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
[NotMapped]
public abstract class AbstractParentIdentifier<TId> : AbstractIdentifier<TId>, IParentIdentifier<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// 父标识。
    /// </summary>
    [Display(Name = nameof(ParentId), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
    public virtual TId? ParentId { get; set; }


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(ParentId)}={ParentId}";

}
