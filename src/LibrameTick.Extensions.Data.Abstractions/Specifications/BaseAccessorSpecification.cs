#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Specifications;

///// <summary>
///// 定义实现 <see cref="IAccessorSpecification"/> 的基础存取器规约（默认按优先级进行升序排列）。
///// </summary>
//public class BaseAccessorSpecification : BaseSpecification<IAccessor>, IAccessorSpecification
//{
//    /// <summary>
//    /// 构造一个默认 <see cref="BaseAccessorSpecification"/>。
//    /// </summary>
//    public BaseAccessorSpecification()
//        : base()
//    {
//        // 默认按优先级升序排列
//        SetOrderBy(a => a.Priority);
//    }

//    /// <summary>
//    /// 使用指定的判断依据构造一个 <see cref="BaseAccessorSpecification"/>。
//    /// </summary>
//    /// <param name="criterion">给定的判断依据。</param>
//    public BaseAccessorSpecification(Func<IAccessor, bool> criterion)
//        : base(criterion)
//    {
//        // 默认按优先级升序排列
//        SetOrderBy(a => a.Priority);
//    }


//    /// <summary>
//    /// 规约访问模式。
//    /// </summary>
//    public AccessMode? Access { get; private set; }

//    /// <summary>
//    /// 规约分组。
//    /// </summary>
//    public int? Group { get; private set; }

//    /// <summary>
//    /// 调度器选项。
//    /// </summary>
//    public DispatcherOptions? DispatcherOptions { get; private set; }

//    /// <summary>
//    /// 规约冗余模式（默认未启用）。
//    /// </summary>
//    public RedundancyMode Redundancy { get; private set; }

//    /// <summary>
//    /// 规约冗余存取器方法。
//    /// </summary>
//    public Func<IEnumerable<IAccessor>, RedundancyMode, DispatcherOptions, IAccessor> RedundancyAccessorFunc { get; private set; }
//        = RedundableAccessorsExtensions.GetRedundableAccessors;


//    /// <summary>
//    /// 评估可枚举存取器。
//    /// </summary>
//    /// <param name="enumerable">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
//    /// <returns>返回 <see cref="IEnumerable{IAccessor}"/>。</returns>
//    public override IEnumerable<IAccessor> Evaluate(IEnumerable<IAccessor> enumerable)
//    {
//        // 优先分组
//        if (Group is not null)
//            enumerable = enumerable.Where(p => p.AccessorDescriptor?.Group == Group);

//        // 计算访问模式（区分读写）
//        if (Access is not null)
//        {
//            // 使用位移运算
//            enumerable = enumerable.Where(p =>
//                (Access & p.AccessorDescriptor?.Access) == p.AccessorDescriptor?.Access);
//        }

//        return base.Evaluate(enumerable);
//    }

//    /// <summary>
//    /// 出具可枚举存取器。
//    /// </summary>
//    /// <param name="enumerable">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
//    /// <returns>返回 <see cref="IAccessor"/>。</returns>
//    public override IAccessor Issue(IEnumerable<IAccessor> enumerable)
//    {
//        // 优先出具自定义提供程序
//        if (Provider is not null)
//            return Provider(enumerable);

//        // 使用冗余存取器
//        if (enumerable.NonEnumeratedCount() > 1)
//            return RedundancyAccessorFunc(enumerable, Redundancy, DispatcherOptions ?? new());

//        return enumerable.First();
//    }


//    /// <summary>
//    /// 设置规约访问模式。
//    /// </summary>
//    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
//    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
//    public IAccessorSpecification SetAccess(AccessMode access)
//    {
//        Access = access;
//        return this;
//    }

//    /// <summary>
//    /// 设置规约分组。
//    /// </summary>
//    /// <param name="group">给定的分组。</param>
//    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
//    public IAccessorSpecification SetGroup(int group)
//    {
//        Group = group;
//        return this;
//    }

//    /// <summary>
//    /// 设置调度器选项。
//    /// </summary>
//    /// <param name="options">给定的 <see cref="DispatcherOptions"/>。</param>
//    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
//    public IAccessorSpecification SetDispatcherOptions(DispatcherOptions options)
//    {
//        DispatcherOptions = options;
//        return this;
//    }

//    /// <summary>
//    /// 如果调度器选项为空则设置。
//    /// </summary>
//    /// <param name="options">给定的 <see cref="DispatcherOptions"/>。</param>
//    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
//    public IAccessorSpecification SetDispatcherOptionsIfNull(DispatcherOptions options)
//    {
//        if (DispatcherOptions is null)
//            DispatcherOptions = options;

//        return this;
//    }

//    /// <summary>
//    /// 设置规约冗余模式。
//    /// </summary>
//    /// <param name="redundancy">给定的冗余模式。</param>
//    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
//    public IAccessorSpecification SetRedundancy(RedundancyMode redundancy)
//    {
//        Redundancy = redundancy;
//        return this;
//    }

//    /// <summary>
//    /// 设置规约冗余存取器方法。
//    /// </summary>
//    /// <param name="func">给定的冗余存取器方法。</param>
//    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
//    public IAccessorSpecification SetRedundancyAccessorFunc(
//        Func<IEnumerable<IAccessor>, RedundancyMode, DispatcherOptions, IAccessor> func)
//    {
//        RedundancyAccessorFunc = func;
//        return this;
//    }

//}
