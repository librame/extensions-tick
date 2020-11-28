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
    /// 抽象扩展选项接口（抽象实现 <see cref="IExtensionOptions"/>）。
    /// </summary>
    public abstract class AbstractExtensionOptions : IExtensionOptions
    {
        public AbstractExtensionOptions(string name, DirectoriesOptions directories)
        {
            Name = name;
            Directories = directories;
        }


        /// <summary>
        /// 扩展名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 目录集合。
        /// </summary>
        public DirectoriesOptions Directories { get; }
    }
}
