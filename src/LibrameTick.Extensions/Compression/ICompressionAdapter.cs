#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义压缩适配器接口。
/// </summary>
public interface ICompressionAdapter
{
    /// <summary>
    /// 获取被适配文件扩展名。
    /// </summary>
    /// <value>
    /// 返回文件扩展名字符串。
    /// </value>
    string? BeAdaptedFileExtension { get; }

    /// <summary>
    /// 获取被适配类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    Type BeAdaptedType { get; }

    /// <summary>
    /// 获取被压缩类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    Type BeCompressedType { get; }

    /// <summary>
    /// 获取适配器类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    Type AdapterType { get; }

    /// <summary>
    /// 获取适配器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    string AdapterName { get; }
}
