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

namespace Librame.Extensions.Specifications;

///// <summary>
///// 定义一个实现 <see cref="IEntitySpecification{T}"/> 并按创建时间降序排列的实体规约。
///// </summary>
///// <typeparam name="T">指定的类型。</typeparam>
///// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
//public class OrderByCreationTimeEntitySpecification<T, TCreatedTime> : BaseEntitySpecification<T>
//    where T : class, ICreationTime<TCreatedTime>
//    where TCreatedTime : struct
//{
//    /// <summary>
//    /// 构造一个 <see cref="OrderByCreationTimeEntitySpecification{T, TCreatedTime}"/>。
//    /// </summary>
//    public OrderByCreationTimeEntitySpecification()
//        : base()
//    {
//        SetOrderByDescending(s => s.CreatedTime);
//    }

//    /// <summary>
//    /// 使用规约条件构造一个 <see cref="OrderByCreationTimeEntitySpecification{T, TCreatedTime}"/> 实例。
//    /// </summary>
//    /// <param name="criterion">给定的判断依据表达式。</param>
//    public OrderByCreationTimeEntitySpecification(Expression<Func<T, bool>> criterion)
//        : base(criterion)
//    {
//        SetOrderByDescending(s => s.CreatedTime);
//    }

//}
