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
/// 定义分库设置根。
/// </summary>
public class ShardingDatabaseSettingRoot : ISettingRoot
{
    /// <summary>
    /// 分库设置集合。
    /// </summary>
    public List<ShardingDatabaseSetting> Shardeds { get; set; } = new();
}
