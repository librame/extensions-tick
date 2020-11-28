#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Options
{
    /// <summary>
    /// 扩展选项接口。
    /// </summary>
    public interface IExtensionOptions
    {
        /// <summary>
        /// 扩展名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 目录集合。
        /// </summary>
        DirectoriesOptions Directories { get; }
    }
}
