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
/// 定义二进制序列化忽略成员的自定义特性。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = false, AllowMultiple = false)]
public sealed class BinaryIgnoreAttribute : Attribute
{
}
