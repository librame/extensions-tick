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
/// 定义抽象实现 <see cref="ISettingProvider{TSetting}"/> 的设置提供程序。
/// </summary>
/// <typeparam name="TSetting">指定的设置类型。</typeparam>
public abstract class AbstractSettingProvider<TSetting> : ISettingProvider<TSetting>
    where TSetting : class, ISetting
{
    private readonly IConfigurationBuilder _settingBuilder;
    private readonly IOptionsMonitor<TSetting> _optionsMonitor;


    /// <summary>
    /// 构造一个 <see cref="AbstractSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="settingBuilder">给定的 <see cref="IConfigurationBuilder"/>。</param>
    /// <param name="postAction">给定的后置配置 <typeparamref name="TSetting"/> 动作。</param>
    /// <param name="setupOptions">给定的安装 <see cref="SettingOptions"/> 动作。</param>
    protected AbstractSettingProvider(IOptionsMonitorCache<TSetting> cache,
        string? settingName, IConfigurationBuilder settingBuilder,
        Action<TSetting>? postAction, Action<SettingOptions>? setupOptions)
    {
        var options = new SettingOptions();
        setupOptions?.Invoke(options);

        var configurationRoot = settingBuilder.Build();
        options.SetupConfigurationRoot?.Invoke(configurationRoot);

        var configureOptions = options.GetConfigureOptions<TSetting>(settingName, configurationRoot);
        var postConfigureOptions = SettingOptions.GetPostConfigureOptions(settingName, postAction);

        var factory = new OptionsFactory<TSetting>(configureOptions, postConfigureOptions);
        var sources = SettingOptions.GetOptionsChangeTokenSources<TSetting>(settingName, configurationRoot);

        _optionsMonitor = new OptionsMonitor<TSetting>(factory, sources, cache);
        _settingBuilder = settingBuilder;
    }


    /// <summary>
    /// 设置构建器。
    /// </summary>
    protected IConfigurationBuilder SettingBuilder
        => _settingBuilder;

    /// <summary>
    /// 当前设置。
    /// </summary>
    public TSetting CurrentSetting
        => _optionsMonitor.CurrentValue;


    /// <summary>
    /// 保存变化（默认直接返回当前设置）。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public virtual TSetting SaveChanges()
        => CurrentSetting;

}
