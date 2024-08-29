#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义用于控制当前引用是否启用的接口。
/// </summary>
public interface IReferenceEnablement
{
    /// <summary>
    /// 获取或设置当前是否已启用此引用。
    /// </summary>
    bool IsReferenceEnabled { get; set; }
}
