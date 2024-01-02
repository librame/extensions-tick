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
/// 定义实现 <see cref="BaseSettingProvider{TSetting}"/> 的 JSON 文件配置提供程序。
/// </summary>
/// <typeparam name="TSetting">指定的设置类型。</typeparam>
public class JsonSettingProvider<TSetting> : BaseSettingProvider<TSetting>
    where TSetting : class, ISetting
{
    /// <summary>
    /// 构造一个 <see cref="JsonSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    public JsonSettingProvider(IOptionsMonitorCache<TSetting> cache,
        Action<IConfigurationBuilder> setupSettingBuilder)
        : base(cache, setupSettingBuilder)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="JsonSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    public JsonSettingProvider(IOptionsMonitorCache<TSetting> cache, string? settingName,
        Action<IConfigurationBuilder> setupSettingBuilder)
        : base(cache, settingName, setupSettingBuilder)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="JsonSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    /// <param name="postAction">给定的后置配置 <typeparamref name="TSetting"/> 动作。</param>
    public JsonSettingProvider(IOptionsMonitorCache<TSetting> cache,
        Action<IConfigurationBuilder> setupSettingBuilder, Action<TSetting>? postAction)
        : base(cache, setupSettingBuilder, postAction)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="JsonSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="cache">给定的 <see cref="IOptionsMonitorCache{TSetting}"/>。</param>
    /// <param name="settingName">给定的设置名称。</param>
    /// <param name="setupSettingBuilder">给定的安装 <see cref="IConfigurationBuilder"/> 动作。</param>
    /// <param name="postAction">给定的后置配置 <typeparamref name="TSetting"/> 动作。</param>
    public JsonSettingProvider(IOptionsMonitorCache<TSetting> cache, string? settingName,
        Action<IConfigurationBuilder> setupSettingBuilder, Action<TSetting>? postAction)
        : base(cache, settingName, setupSettingBuilder, postAction)
    {
    }


    /// <summary>
    /// 保存变化。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public override TSetting SaveChanges()
    {
        if (SettingBuilder.TryGetSingleFileConfigurationSourceWatcher(out var result))
        {
            (FileConfigurationSource source, IFileProvider provider, FileSystemWatcher? watcher) = result;

            watcher!.EnableRaisingEvents = false;

            source.SaveJsonFile(provider, CurrentSetting, forceOverwrite: true);

            watcher!.EnableRaisingEvents = true;
        }

        return CurrentSetting;
    }

}
