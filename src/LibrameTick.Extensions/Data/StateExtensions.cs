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
/// <see cref="IState{TStatus}"/> 静态扩展。
/// </summary>
public static class StateExtensions
{

    /// <summary>
    /// 设置状态。
    /// </summary>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    /// <param name="state">给定的 <see cref="IState{TStatus}"/>。</param>
    /// <param name="newStatusFactory">给定的新 <typeparamref name="TStatus"/> 工厂方法。</param>
    /// <returns>返回 <typeparamref name="TStatus"/>（兼容整数、单双精度的排序字段）。</returns>
    public static TStatus SetStatus<TStatus>(this IState<TStatus> state,
        Func<TStatus, TStatus> newStatusFactory)
        where TStatus : struct
        => state.Status = newStatusFactory.Invoke(state.Status);

    /// <summary>
    /// 异步设置状态。
    /// </summary>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    /// <param name="state">给定的 <see cref="IState{TStatus}"/>。</param>
    /// <param name="newStatusFactory">给定的新 <typeparamref name="TStatus"/> 工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TStatus"/>（兼容整数、单双精度的排序字段）的异步操作。</returns>
    public static ValueTask<TStatus> SetStatusAsync<TStatus>(this IState<TStatus> state,
        Func<TStatus, TStatus> newStatusFactory, CancellationToken cancellationToken = default)
        where TStatus : struct
        => cancellationToken.RunValueTask(() => state.Status = newStatusFactory.Invoke(state.Status));


    /// <summary>
    /// 设置对象状态。
    /// </summary>
    /// <param name="state">给定的 <see cref="IObjectState"/>。</param>
    /// <param name="newStatusFactory">给定的新对象状态工厂方法。</param>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    public static object SetObjectStatusAsync(this IObjectState state,
        Func<object, object> newStatusFactory)
    {
        var currentStatus = state.GetObjectStatus();
        return state.SetObjectStatus(newStatusFactory.Invoke(currentStatus));
    }

    /// <summary>
    /// 异步设置对象状态。
    /// </summary>
    /// <param name="state">给定的 <see cref="IObjectState"/>。</param>
    /// <param name="newStatusFactory">给定的新对象状态工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    public static async ValueTask<object> SetObjectStatusAsync(this IObjectState state,
        Func<object, object> newStatusFactory, CancellationToken cancellationToken = default)
    {
        var currentStatus = await state.GetObjectStatusAsync(cancellationToken).ConfigureAwaitWithoutContext();
        return await state.SetObjectStatusAsync(newStatusFactory.Invoke(currentStatus), cancellationToken)
            .ConfigureAwaitWithoutContext();
    }

}
