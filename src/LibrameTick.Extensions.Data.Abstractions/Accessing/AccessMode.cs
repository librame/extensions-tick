#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义用于对数据源存取器的读、写或读/写的常量访问模式（可组合使用）。
/// </summary>
[Flags]
public enum AccessMode
{
    /// <summary>
    /// 对数据源存取器的读访问。可以从数据源存取器中读取数据。结合 Write 进行读写访问。
    /// </summary>
    Read = 1,

    /// <summary>
    /// 对数据源存取器的写访问。可以从数据源存取器中写入数据。结合 Read 进行读写访问。
    /// </summary>
    Write = 2,

    /// <summary>
    /// 对数据源存取器的读/写访问。数据可以写入数据源存取器，也可以从数据源存取器中读取数据。
    /// </summary>
    ReadWrite = 4
}
