#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IExtensionInfo"/>、<see cref="IOptions"/> 的扩展选项接口。
/// </summary>
public interface IExtensionOptions : IExtensionInfo, IOptions
{
    /// <summary>
    /// 目录选项。
    /// </summary>
    DirectoryOptions Directories { get; }

    /// <summary>
    /// 替换服务字典集合。
    /// </summary>
    IDictionary<Type, Type> ReplacedServices { get; }

    /// <summary>
    /// 服务特征集合。
    /// </summary>
    ServiceCharacteristicCollection ServiceCharacteristics { get; }

    /// <summary>
    /// 父级选项。
    /// </summary>
    IExtensionOptions? ParentOptions { get; }
}
