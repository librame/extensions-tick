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
/// 定义实现 <see cref="IOptions"/> 的设置选项。
/// </summary>
public class ShardingSettingOptions : IOptions
{
    /// <summary>
    /// 分栏文件名。
    /// </summary>
    public string ShardingColumnFileName { get; set; } = "setting_columns.json";

    /// <summary>
    /// 分库文件名。
    /// </summary>
    public string ShardingDatabaseFileName { get; set; } = "setting_databases.json";

    /// <summary>
    /// 分表文件名。
    /// </summary>
    public string ShardingTableFileName { get; set; } = "setting_tables.json";
}
