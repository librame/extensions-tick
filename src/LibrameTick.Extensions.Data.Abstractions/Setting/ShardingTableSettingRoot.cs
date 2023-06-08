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
/// 定义分表设置根。
/// </summary>
public class ShardingTableSettingRoot : ISettingRoot
{
    /// <summary>
    /// 分表设置集合。
    /// </summary>
    public List<ShardingTableSetting> Shardeds { get; set; } = new();
}
