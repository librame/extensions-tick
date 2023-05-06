#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Setting;

internal class ColumnJsonFileSettingProvider : AbstractFileSettingProvider<ShardingColumnSetting>
{
    /// <summary>
    /// 构造一个 <see cref="ColumnJsonFileSettingProvider"/>。
    /// </summary>
    /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
    /// <param name="options">给定的 <see cref="IOptionsMonitor{DataExtensionOptions}"/>。</param>
    public ColumnJsonFileSettingProvider(ILoggerFactory loggerFactory, IOptionsMonitor<DataExtensionOptions> options)
        : base(loggerFactory, GetFileName(options.CurrentValue))
    {
    }


    private static string GetFileName(DataExtensionOptions options)
        => options.Setting.ShardingColumnFileName.SetBasePath(options.ShardingDirectory);


    /// <summary>
    /// 加载设置。
    /// </summary>
    /// <returns>返回 <see cref="ShardingColumnSetting"/>。</returns>
    public override ShardingColumnSetting Load()
    {
        var setting = FilePath.DeserializeJsonFile<ShardingColumnSetting>();
        if (setting is null)
            throw new NotSupportedException($"Unsupported {nameof(ShardingColumnSetting)} file format.");

        return setting;
    }

    /// <summary>
    /// 保存设置。
    /// </summary>
    /// <param name="setting">给定的 <see cref="ShardingColumnSetting"/>。</param>
    /// <returns>返回 <see cref="ShardingColumnSetting"/>。</returns>
    public override ShardingColumnSetting Save(ShardingColumnSetting setting)
    {
        FilePath.SerializeJsonFile(setting);

        return setting;
    }

}
