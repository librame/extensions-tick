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
/// 定义二进制序列化成员类型泛型映射的自定义特性。
/// </summary>
/// <param name="version">给定的版本号。</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = false, AllowMultiple = false)]
public sealed class BinaryVersionAttribute(double version) : Attribute
{
    /// <summary>
    /// 版本号。
    /// </summary>
    public double Version { get; set; } = version;
}
