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
/// <see cref="IExtensionOptions"/> 与 <see cref="IConfiguration"/> 静态扩展。
/// </summary>
public static class ExtensionOptionsConfigurationExtensions
{

    /// <summary>
    /// 尝试从 JSON 文件中加载扩展选项。
    /// </summary>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    /// <returns>返回是否成功加载的布尔值。</returns>
    public static bool TryLoadOptionsFromJson(this IExtensionOptions options)
        => options.TryLoadOptionsFromJson(out _);

    /// <summary>
    /// 尝试从 JSON 文件中加载扩展选项。
    /// </summary>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    /// <param name="jsonPath">输出 JSON 文件路径字符串。</param>
    /// <returns>返回是否成功加载的布尔值。</returns>
    public static bool TryLoadOptionsFromJson(this IExtensionOptions options,
        [MaybeNullWhen(false)] out string jsonPath)
    {
        jsonPath = options.BuildJsonPath();
        if (jsonPath.FileExists())
        {
            var root = new ConfigurationBuilder()
                .AddJsonFile(jsonPath) // default(optional: false, reloadOnChange: false)
                .Build();

            // 默认从配置根对象的第一个子配置部分加载
            var section = root.GetChildren().FirstOrDefault();
            if (section is not null)
            {
                options.LoadOptions(section);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 从配置实例中加载扩展选项。
    /// </summary>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
    public static void LoadOptions(this IExtensionOptions options, IConfiguration configuration)
        => configuration.Bind(options);

}
