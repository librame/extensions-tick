#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义一个实现 <see cref="IOptions"/> 的设置选项。
/// </summary>
public class SettingOptions : IOptions
{
    /// <summary>
    /// 安装配置根。
    /// </summary>
    [JsonIgnore]
    public Action<IConfigurationRoot>? SetupConfigurationRoot { get; set; }

    /// <summary>
    /// 安装绑定器选项。
    /// </summary>
    [JsonIgnore]
    public Action<BinderOptions>? SetupBinderOptions { get; set; }


    /// <summary>
    /// 获取设置的选项变化令牌源集合。
    /// </summary>
    /// <typeparam name="TSetting">指定的设置类型。</typeparam>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="configurationRoot">给定的 <see cref="IConfigurationRoot"/>。</param>
    /// <returns>返回 <see cref="IOptionsChangeTokenSource{TSetting}"/> 集合。</returns>
    public static IEnumerable<IOptionsChangeTokenSource<TSetting>> GetOptionsChangeTokenSources<TSetting>(string? settingName,
        IConfigurationRoot configurationRoot)
        where TSetting : class, ISetting
    {
        yield return new ConfigurationChangeTokenSource<TSetting>(settingName, configurationRoot);
    }

    /// <summary>
    /// 获取设置的配置选项集合。
    /// </summary>
    /// <typeparam name="TSetting">指定的设置类型。</typeparam>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="configurationRoot">给定的 <see cref="IConfigurationRoot"/>。</param>
    /// <returns>返回 <see cref="IConfigureOptions{TSetting}"/> 集合。</returns>
    public IEnumerable<IConfigureOptions<TSetting>> GetConfigureOptions<TSetting>(string? settingName,
        IConfigurationRoot configurationRoot)
        where TSetting : class, ISetting
    {
        yield return new NamedConfigureFromConfigurationOptions<TSetting>(settingName,
            configurationRoot, SetupBinderOptions);
    }

    /// <summary>
    /// 获取设置的后置配置选项集合。
    /// </summary>
    /// <typeparam name="TSetting">指定的设置类型。</typeparam>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="postAction">给定的后置配置 <typeparamref name="TSetting"/> 动作。</param>
    /// <returns>返回 <see cref="IPostConfigureOptions{TSetting}"/> 集合。</returns>
    public static IEnumerable<IPostConfigureOptions<TSetting>> GetPostConfigureOptions<TSetting>(string? settingName,
        Action<TSetting>? postAction)
        where TSetting : class, ISetting
    {
        yield return new PostConfigureOptions<TSetting>(settingName, postAction);
    }

}
