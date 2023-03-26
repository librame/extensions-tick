﻿#region License

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
/// 定义基于文化信息的分片策略。
/// </summary>
/// <remarks>
/// 文化信息的分片策略支持的参数（区分大小写）包括：%c（名称）。
/// </remarks>
public class CultureInfoShardingStrategy : AbstractShardingStrategy<CultureInfo>
{
    /// <summary>
    /// 构造一个 <see cref="CultureInfoShardingStrategy"/>。
    /// </summary>
    public CultureInfoShardingStrategy()
        : base()
    {
        AddParameter("c", uic => uic.Name.Replace('-', '_'));
    }


    /// <summary>
    /// 重写默认值为当前文化信息 <see cref="CultureInfo.CurrentCulture"/>。
    /// </summary>
    public override Lazy<CultureInfo> DefaultValue
        => new Lazy<CultureInfo>(CultureInfo.CurrentCulture);

}
