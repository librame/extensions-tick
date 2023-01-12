#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

//namespace Librame.Extensions.Core;

///// <summary>
///// <see cref="IExtensionOptions"/> 与 <see cref="IConfiguration"/> 静态扩展。
///// </summary>
//public static class ExtensionOptionsConfigurationExtensions
//{

    ///// <summary>
    ///// 从 JSON 文件获取配置选项。
    ///// </summary>
    ///// <param name="optionsType">给定的扩展选项类型。</param>
    ///// <returns>返回 <see cref="IConfiguration"/>。</returns>
    //public static IConfiguration? GetConfigOptionsFromJson(this Type optionsType)
    //    => optionsType.BuildJsonFilePath().GetConfigOptionsFromJson();

    ///// <summary>
    ///// 从 JSON 文件获取配置选项。
    ///// </summary>
    ///// <param name="filePath">给定的 JSON 文件路径。</param>
    ///// <returns>返回 <see cref="IConfiguration"/>。</returns>
    //public static IConfiguration? GetConfigOptionsFromJson(this string filePath)
    //{
    //    if (filePath.FileExists())
    //    {
    //        var root = new ConfigurationBuilder()
    //            .AddJsonFile(filePath) // default(optional: false, reloadOnChange: false)
    //            .Build();

    //        // 默认从配置根对象的第一个子配置部分加载
    //        return root.GetChildren().FirstOrDefault();
    //    }

    //    return null;
    //}


    ///// <summary>
    ///// 尝试从 JSON 文件中加载扩展选项。
    ///// </summary>
    ///// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    ///// <returns>返回是否成功加载的布尔值。</returns>
    //public static bool TryLoadOptionsFromJson(this IExtensionOptions options)
    //    => options.TryLoadOptionsFromJson(out _);

    ///// <summary>
    ///// 尝试从 JSON 文件中加载扩展选项。
    ///// </summary>
    ///// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    ///// <param name="filePath">输出 JSON 文件路径字符串。</param>
    ///// <returns>返回是否成功加载的布尔值。</returns>
    //public static bool TryLoadOptionsFromJson(this IExtensionOptions options,
    //    [MaybeNullWhen(false)] out string filePath)
    //{
    //    filePath = options.BuildJsonFilePath();

    //    var configuration = filePath.GetConfigOptionsFromJson();
    //    if (configuration is not null)
    //    {
    //        configuration.Bind(options);
    //        return true;
    //    }

    //    return false;
    //}

//}
