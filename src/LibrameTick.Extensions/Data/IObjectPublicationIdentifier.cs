#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义联合 <see cref="IObjectCreation"/> 与 <see cref="IObjectIdentifier"/> 的对象发表标识符接口。
/// </summary>
public interface IObjectPublicationIdentifier : IObjectPublication, IObjectIdentifier
{
}
