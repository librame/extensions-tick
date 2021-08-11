#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;

namespace Librame.Extensions.Core.Storage
{
    /// <summary>
    /// 定义存储目录内容集合接口。
    /// </summary>
    public interface IStorageDirectoryContents : IEnumerable<IStorageFileInfo>, IDirectoryContents
    {
    }
}
