#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义文件设置根描述符。
/// </summary>
public class FileSettingRootDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="FileSettingRootDescriptor"/>。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="optional">文件是否可选。</param>
    /// <param name="reloadOnChange">文件变化是否重载。</param>
    public FileSettingRootDescriptor(string path, bool optional, bool reloadOnChange)
    {
        Path = path;
        Optional = optional;
        ReloadOnChange = reloadOnChange;
    }


    /// <summary>
    /// 文件路径。
    /// </summary>
    public string Path { get; init; }

    /// <summary>
    /// 是否可选。
    /// </summary>
    public bool Optional { get; init; }

    /// <summary>
    /// 文件变化是否重载。
    /// </summary>
    public bool ReloadOnChange { get; init; }
}
