#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制序列化成员类型映射的自定义特性。
/// </summary>
/// <remarks>
/// 默认为成员标注此特性可为自定义类型元素启用序列化支持（支持列表中的自定义类型元素）；如果要为字典集合启用，则在标注特性的基础上，再分别设置要为 <see cref="ForKey"/> 或 <see cref="ForValue"/> 启用序列化支持。
/// </remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = true, AllowMultiple = true)]
public sealed class BinaryMappingAttribute() : Attribute
{
    /// <summary>
    /// 为键启用此特性。
    /// </summary>
    public bool ForKey { get; set; }

    /// <summary>
    /// 为值启用此特性。
    /// </summary>
    public bool ForValue { get; set; }
}
