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
/// 定义抽象实现 <see cref="IExtensionInfo"/> 的扩展信息。
/// </summary>
public abstract class AbstractExtensionInfo : IExtensionInfo
{
    /// <summary>
    /// 扩展类型。
    /// </summary>
    [JsonIgnore]
    public virtual Type ExtensionType
        => GetType();

    /// <summary>
    /// 扩展名称。
    /// </summary>
    public virtual string ExtensionName
        => ExtensionType.Name;
}
