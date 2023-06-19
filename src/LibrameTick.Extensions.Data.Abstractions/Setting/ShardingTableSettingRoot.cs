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
    public List<ShardingTableSetting> Tables { get; set; } = new();


    /// <summary>
    /// 尝试获取指定实体类型的分表设置。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="result">输出 <see cref="ShardingTableSetting"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool TryGetTable(Type entityType,
        [MaybeNullWhen(false)] out ShardingTableSetting result)
    {
        result = Tables.SingleOrDefault(p => p.SourceType == entityType);
        return result is not null;
    }

}
