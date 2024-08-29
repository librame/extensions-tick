#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义对象优先级接口。
/// </summary>
public interface IObjectPrioritization
{
    /// <summary>
    /// 优先级类型。
    /// </summary>
    Type PriorityType { get; }


    /// <summary>
    /// 获取对象优先级。
    /// </summary>
    /// <returns>返回优先级（兼容整数、单双精度的优先级字段）。</returns>
    object GetObjectPriority();

    /// <summary>
    /// 异步获取对象优先级。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含优先级（兼容整数、单双精度的优先级字段）的异步操作。</returns>
    ValueTask<object> GetObjectPriorityAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象优先级。
    /// </summary>
    /// <param name="newPriority">给定的新优先级对象。</param>
    /// <returns>返回优先（兼容整数、单双精度的优先级字段）。</returns>
    object SetObjectPriority(object newPriority);

    /// <summary>
    /// 异步设置对象优先级。
    /// </summary>
    /// <param name="newPriority">给定的新优先级对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含优先（兼容整数、单双精度的优先级字段）的异步操作。</returns>
    ValueTask<object> SetObjectPriorityAsync(object newPriority, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象优先级。
    /// </summary>
    /// <param name="newPriorityFactory">给定的新优先级对象工厂方法。</param>
    /// <returns>返回优先（兼容整数、单双精度的优先级字段）。</returns>
    object SetObjectPriority(Func<object, object> newPriorityFactory)
    {
        var currentPriority = GetObjectPriority();

        return SetObjectPriority(newPriorityFactory(currentPriority));
    }

    /// <summary>
    /// 异步设置对象优先级。
    /// </summary>
    /// <param name="newPriorityFactory">给定的新优先级对象工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含优先（兼容整数、单双精度的优先级字段）的异步操作。</returns>
    async ValueTask<object> SetObjectPriorityAsync(Func<object, object> newPriorityFactory,
        CancellationToken cancellationToken = default)
    {
        var currentPriority = await GetObjectPriorityAsync(cancellationToken).ConfigureAwait(false);

        return await SetObjectPriorityAsync(newPriorityFactory(currentPriority), cancellationToken).ConfigureAwait(false);
    }

}
