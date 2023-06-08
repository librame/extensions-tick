#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Resources;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义抽象 <see cref="IRanking{TRank}"/>。
/// </summary>
/// <typeparam name="TRank">指定的排名类型（兼容整数、单双精度等结构体的排名字段）。</typeparam>
public abstract class AbstractRanking<TRank> : IRanking<TRank>
    where TRank : IComparable<TRank>, IEquatable<TRank>
{

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 排名。
    /// </summary>
    [Display(Name = nameof(Rank), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
    public virtual TRank Rank { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


}
