#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace Librame.Extensions.Setting;

///// <summary>
///// 定义实现 <see cref="ISettingProvider{ShardingDatabaseSetting}"/> 的分库 JSON 文件型选项提供程序。
///// </summary>
//public class DatabaseJsonFileSettingProvider : BaseSettingProvider<ShardingDatabaseSetting>
//{
//    /// <summary>
//    /// 构造一个 <see cref="DatabaseJsonFileSettingProvider"/>。
//    /// </summary>
//    /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
//    /// <param name="options">给定的 <see cref="IOptionsMonitor{DataExtensionOptions}"/>。</param>
//    public DatabaseJsonFileSettingProvider(ILoggerFactory loggerFactory, IOptionsMonitor<DataExtensionOptions> options)
//        : base(loggerFactory, GetFileName(options.CurrentValue))
//    {
//    }


//    private static string GetFileName(DataExtensionOptions options)
//        => options.Setting.ShardingDatabaseFileName.SetBasePath(options.ShardingDirectory);


//    /// <summary>
//    /// 加载设置。
//    /// </summary>
//    /// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
//    public override ShardingDatabaseSetting Load()
//    {
//        var setting = FilePath.DeserializeJsonFile<ShardingDatabaseSetting>();
//        if (setting is null)
//            throw new NotSupportedException($"Unsupported {nameof(ShardingDatabaseSetting)} file format.");

//        return setting;
//    }

//    /// <summary>
//    /// 保存设置。
//    /// </summary>
//    /// <param name="setting">给定的 <see cref="ShardingDatabaseSetting"/>。</param>
//    /// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
//    public override ShardingDatabaseSetting Save(ShardingDatabaseSetting setting)
//    {
//        FilePath.SerializeJsonFile(setting);

//        return setting;
//    }


//    /// <summary>
//    /// 生成设置。
//    /// </summary>
//    /// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
//    public override ShardingDatabaseSetting Generate()
//        => new();

//}
