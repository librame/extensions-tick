#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Crypto;

/// <summary>
/// 定义表示此属性已加密的特性。
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class EncryptedAttribute : Attribute
{
}
