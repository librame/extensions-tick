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
/// <see cref="IExtensionOptions"/> 静态扩展。
/// </summary>
public static class ExtensionOptionsExtensions
{

    /// <summary>
    /// 获取扩展选项的 JSON 文件名（不包含文件路径；默认以扩展选项程序集名称为文件名）。
    /// </summary>
    /// <param name="optionsType">给定的扩展选项类型。</param>
    /// <returns>返回字符串。</returns>
    public static string GetJsonFileName(this Type optionsType)
        => $"{optionsType.GetAssemblyName()}.json";

    /// <summary>
    /// 构建扩展选项的 JSON 文件路径（默认以扩展选项的配置目录为基础路径，以 <see cref="GetJsonFileName(Type)"/> 为文件名）。
    /// </summary>
    /// <param name="optionsType">给定的扩展选项类型。</param>
    /// <param name="directories">给定的 <see cref="IRegisterableDirectories"/>（可选；默认以 <see cref="Registration.GetRegisterableDirectories()"/> 的配置目录为基础路径）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string BuildJsonFilePath(this Type optionsType, IRegisterableDirectories? directories = null)
        => optionsType.GetJsonFileName().SetBasePath((directories ?? Registration.GetRegisterableDirectories()).ConfigDirectory);

    /// <summary>
    /// 构建扩展选项的 JSON 文件路径（默认以扩展选项的配置目录为基础路径，以 <see cref="GetJsonFileName(Type)"/> 为文件名）。
    /// </summary>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回路径字符串。</returns>
    public static string BuildJsonFilePath(this IExtensionOptions options)
        => options.GetType().GetJsonFileName().SetBasePath(options.Directories.ConfigDirectory);


    /// <summary>
    /// 将扩展选项保存为 JSON 文件。
    /// </summary>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回保存的路径字符串。</returns>
    public static string SaveOptionsAsJson(this IExtensionOptions options)
    {
        var jsonPath = options.BuildJsonFilePath();
        jsonPath.SerializeJsonFile(options);

        return jsonPath;
    }

}
