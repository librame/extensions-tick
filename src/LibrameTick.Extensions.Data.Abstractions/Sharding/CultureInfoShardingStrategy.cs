#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义基于文化信息的分片策略（策略前缀为 ci）。
/// </summary>
/// <remarks>
/// 文化信息的分片策略支持的参数（区分大小写）包括：
///     <para>%ci:nh（带连字符的名称，如：zh-CN）、</para>
///     <para>%ci:nu（带下划线的名称，如：zh_CN）、</para>
///     <para>%ci:n2li（带下划线的名称，如：zh）、</para>
///     <para>%ci:n3li（带下划线的名称，如：zho）、</para>
///     <para>%ci:n3lwl（带下划线的名称，如：chs）。</para>
///     <para>%ci:n3lwu（带下划线的名称，如：CHS）。</para>
/// 详情参考：https://learn.microsoft.com/zh-cn/dotnet/api/system.globalization.cultureinfo.name?view=net-8.0
/// </remarks>
public class CultureInfoShardingStrategy : AbstractShardingStrategy<CultureInfo>
{
    /// <summary>
    /// 构造一个 <see cref="CultureInfoShardingStrategy"/>。
    /// </summary>
    /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
    public CultureInfoShardingStrategy(Func<CultureInfo> defaultValueFactory)
        : base("ci", defaultValueFactory)
    {
        AddParameter("nh", static uic => uic.Name);
        AddParameter("nu", static uic => uic.Name.Replace('-', '_'));
        AddParameter("n2li", static uic => uic.TwoLetterISOLanguageName);
        AddParameter("n3li", static uic => uic.ThreeLetterISOLanguageName);
        AddParameter("n3lwl", static uic => uic.ThreeLetterWindowsLanguageName.ToLower());
        AddParameter("n3lwu", static uic => uic.ThreeLetterWindowsLanguageName);
    }

}
