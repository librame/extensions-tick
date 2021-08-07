#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Access
{
    /// <summary>
    /// 定义用于对数据源访问器的读、写或读/写模式的常量交互方式。
    /// </summary>
    [Flags]
    public enum AccessorInteraction
    {
        /// <summary>
        /// 对数据源访问器的读访问。可以从数据源访问器中读取数据。结合 Write 进行读写访问。
        /// </summary>
        Read = 1,

        /// <summary>
        /// 对数据源访问器的写访问。可以从数据源访问器中写入数据。结合 Read 进行读写访问。
        /// </summary>
        Write = 2,

        /// <summary>
        /// 对数据源访问器的读写访问。数据可以写入数据源访问器，也可以从数据源访问器中读取数据。
        /// </summary>
        ReadWrite = 4
    }
}
