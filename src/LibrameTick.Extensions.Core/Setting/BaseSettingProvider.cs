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
/// 定义实现 <see cref="AbstractSettingProvider{TSetting}"/> 的基础设置提供程序。
/// </summary>
/// <typeparam name="TSetting">指定的设置类型。</typeparam>
public class BaseSettingProvider<TSetting> : AbstractSettingProvider<TSetting>
    where TSetting : class, ISetting
{
    /// <summary>
    /// 构造一个 <see cref="BaseSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    public BaseSettingProvider(IOptionsMonitorCache<TSetting> cache,
        Action<IConfigurationBuilder> setupSettingBuilder)
        : this(cache, settingName: null, setupSettingBuilder, postAction: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="BaseSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    public BaseSettingProvider(IOptionsMonitorCache<TSetting> cache, string? settingName,
        Action<IConfigurationBuilder> setupSettingBuilder)
        : this(cache, settingName, setupSettingBuilder, postAction: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="BaseSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    /// <param name="postAction">给定的后置配置 <typeparamref name="TSetting"/> 动作。</param>
    public BaseSettingProvider(IOptionsMonitorCache<TSetting> cache,
        Action<IConfigurationBuilder> setupSettingBuilder, Action<TSetting>? postAction)
        : this(cache, settingName: null, setupSettingBuilder, postAction)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="BaseSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    /// <param name="postAction">给定的后置配置 <typeparamref name="TSetting"/> 动作。</param>
    public BaseSettingProvider(IOptionsMonitorCache<TSetting> cache, string? settingName,
        Action<IConfigurationBuilder> setupSettingBuilder, Action<TSetting>? postAction)
        : base(cache, settingName, InitialSettingBuilder(setupSettingBuilder), postAction, ConfigureSettingOptions)
    {
    }


    private static void ConfigureSettingOptions(SettingOptions options)
    {
        options.SetupConfigurationRoot = config => config.EnableTemplate(); // 启用配置模板功能
    }

    private static ConfigurationBuilder InitialSettingBuilder(Action<IConfigurationBuilder> setupBuilder)
    {
        var builder = new ConfigurationBuilder();

        setupBuilder(builder);

        return builder;
    }

}
