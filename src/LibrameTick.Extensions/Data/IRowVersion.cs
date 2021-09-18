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
/// 定义行版本接口。
/// </summary>
public interface IRowVersion
{
    /// <summary>
    /// 行版本。
    /// </summary>
    byte[] RowVersion { get; set; }
}
