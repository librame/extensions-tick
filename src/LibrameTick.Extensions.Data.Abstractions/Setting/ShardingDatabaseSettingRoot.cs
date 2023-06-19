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
    public List<ShardingDatabaseSetting> Databases { get; set; } = new();


    /// <summary>
    /// 尝试获取指定存取器类型的分库设置。
    /// </summary>
    /// <param name="accessorType">给定的存取器类型。</param>
    /// <param name="result">输出 <see cref="ShardingDatabaseSetting"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool TryGetDatabase(Type accessorType,
        [MaybeNullWhen(false)] out ShardingDatabaseSetting result)
    {
        result = Databases.SingleOrDefault(p => p.SourceType == accessorType);
        return result is not null;
    }

}
