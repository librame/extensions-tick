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
/// 定义二进制序列化成员排序的自定义特性。
/// </summary>
/// <param name="id">给定的序号标识。</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = true, AllowMultiple = false)]
public class BinaryOrderAttribute(int id) : Attribute
{
    /// <summary>
    /// 获取或设置序号标识。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int Id { get; init; } = id;
}
