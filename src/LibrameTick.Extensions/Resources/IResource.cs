﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Resources;

/// <summary>
/// 定义一个资源接口。
/// </summary>
public interface IResource
{
    /// <summary>
    /// 资源名称。
    /// </summary>
    string ResourceName { get; }

    /// <summary>
    /// 资源类型。
    /// </summary>
    Type ResourceType { get; }
}
