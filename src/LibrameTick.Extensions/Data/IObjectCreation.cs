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
/// 定义联合 <see cref="IObjectCreationTime"/> 与 <see cref="IObjectCreator"/> 的创建接口。
/// </summary>
public interface IObjectCreation : IObjectCreationTime, IObjectCreator
{
}
