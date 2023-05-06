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

internal class TableJsonFileSettingProvider : AbstractFileSettingProvider<ShardingTableSetting>
{
    /// <summary>
    /// 构造一个 <see cref="TableJsonFileSettingProvider"/>。
    /// </summary>
    /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
    /// <param name="options">给定的 <see cref="IOptionsMonitor{DataExtensionOptions}"/>。</param>
    public TableJsonFileSettingProvider(ILoggerFactory loggerFactory, IOptionsMonitor<DataExtensionOptions> options)
        : base(loggerFactory, GetFileName(options.CurrentValue))
    {
    }


    private static string GetFileName(DataExtensionOptions options)
        => options.Setting.ShardingTableFileName.SetBasePath(options.ShardingDirectory);


    /// <summary>
    /// 加载设置。
    /// </summary>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public override ShardingTableSetting Load()
    {
        var setting = FilePath.DeserializeJsonFile<ShardingTableSetting>();
        if (setting is null)
            throw new NotSupportedException($"Unsupported {nameof(ShardingTableSetting)} file format.");

        return setting;
    }

    /// <summary>
    /// 保存设置。
    /// </summary>
    /// <param name="setting">给定的 <see cref="ShardingTableSetting"/>。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public override ShardingTableSetting Save(ShardingTableSetting setting)
    {
        FilePath.SerializeJsonFile(setting);

        return setting;
    }

}
