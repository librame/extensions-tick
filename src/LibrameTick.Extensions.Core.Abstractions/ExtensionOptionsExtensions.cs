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
    /// 构建扩展选项的 JSON 文件路径（默认以扩展选项程序集名称为文件名）。
    /// </summary>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回字符串。</returns>
    public static string BuildJsonPath(this IExtensionOptions options)
        => $"{options.GetType().GetAssemblyName()}.json".SetBasePath(options.Directories.ConfigDirectory);


    /// <summary>
    /// 查找指定目标扩展选项（支持链式查找父级扩展选项）。
    /// </summary>
    /// <typeparam name="TTargetOptions">指定的目标扩展选项类型。</typeparam>
    /// <param name="lastOptions">给定配置的最后一个 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回 <typeparamref name="TTargetOptions"/>。</returns>
    public static TTargetOptions? FindOptions<TTargetOptions>(this IExtensionOptions lastOptions)
        where TTargetOptions : IExtensionOptions
    {
        if (!(lastOptions is TTargetOptions targetOptions))
        {
            if (lastOptions.ParentOptions is not null)
                return FindOptions<TTargetOptions>(lastOptions.ParentOptions);

            return default;
        }

        return targetOptions;
    }

    /// <summary>
    /// 获取必需的目标扩展选项（通过 <see cref="FindOptions{TTargetOptions}(IExtensionOptions)"/> 实现，如果未找到则抛出异常）。
    /// </summary>
    /// <typeparam name="TTargetOptions">指定的目标扩展选项类型。</typeparam>
    /// <param name="lastOptions">给定配置的最后一个 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回 <typeparamref name="TTargetOptions"/>。</returns>
    public static TTargetOptions GetRequiredOptions<TTargetOptions>(this IExtensionOptions lastOptions)
        where TTargetOptions : IExtensionOptions
    {
        var targetOptions = lastOptions.FindOptions<TTargetOptions>();
        if (targetOptions is null)
            throw new ArgumentException($"Target options instance '{typeof(TTargetOptions)}' not found from current options '{lastOptions.GetType()}'.");

        return targetOptions;
    }


    /// <summary>
    /// 将当前扩展选项（含父级扩展选项）另存为 JSON 文件。
    /// </summary>
    /// <param name="lastOptions">给定配置的最后一个 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回 <see cref="Dictionary{String, IExtensionOptions}"/>。</returns>
    public static Dictionary<string, IExtensionOptions> SaveOptionsAsJson(this IExtensionOptions lastOptions)
    {
        var allOptions = new Dictionary<string, IExtensionOptions>();

        SaveOptions(lastOptions, allOptions);

        return allOptions;

        static string SaveOptions(IExtensionOptions current, Dictionary<string, IExtensionOptions> dictionary)
        {
            var jsonPath = current.BuildJsonPath();

            jsonPath.WriteJson(current);
            dictionary.Add(jsonPath, current);

            if (current.ParentOptions is not null)
                SaveOptions(current.ParentOptions, dictionary);

            return jsonPath;
        }
    }

}
