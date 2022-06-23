#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个可拦截接口。
/// </summary>
public interface IInterceptable
{
    /// <summary>
    /// 源类型。
    /// </summary>
    Type? SourceType { get; set; }

    /// <summary>
    /// 源对象。
    /// </summary>
    object? Source { set; }

    /// <summary>
    /// 当前调用的拦截描述符。
    /// </summary>
    InterceptionDescriptor? CurrentDescriptor { get; }


    /// <summary>
    /// 移除指定拦截描述符。
    /// </summary>
    /// <param name="present">给定的 <see cref="InterceptionDescriptor"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool Remove(InterceptionDescriptor present);

    /// <summary>
    /// 移除指定拦截描述符。
    /// </summary>
    /// <param name="match">给定的断定 <see cref="InterceptionDescriptor"/> 方法。</param>
    /// <returns>返回移除的拦截描述符数。</returns>
    int RemoveWhere(Predicate<InterceptionDescriptor> match);


    /// <summary>
    /// 添加或更新拦截描述符集合。
    /// </summary>
    /// <param name="additives">给定的 <see cref="InterceptionDescriptor"/> 集合。</param>
    /// <param name="updateAction">给定的更新描述符方法。</param>
    /// <returns>返回 <see cref="IInterceptable"/>。</returns>
    IInterceptable AddOrUpdateRange(IEnumerable<InterceptionDescriptor> additives,
        Func<(InterceptionDescriptor additive, InterceptionDescriptor present), InterceptionDescriptor>? updateAction = null);

    /// <summary>
    /// 添加或更新拦截描述符。
    /// </summary>
    /// <param name="additive">给定的 <see cref="InterceptionDescriptor"/>。</param>
    /// <param name="updateAction">给定的更新描述符配置动作。</param>
    /// <returns>返回 <see cref="IInterceptable"/>。</returns>
    IInterceptable AddOrUpdate(InterceptionDescriptor additive,
        Func<(InterceptionDescriptor additive, InterceptionDescriptor present), InterceptionDescriptor>? updateAction = null);
}


/// <summary>
/// 定义一个实现 <see cref="IInterceptable"/> 的可拦截对象。
/// </summary>
public class Interceptable : DispatchProxy, IInterceptable
{
    private readonly HashSet<InterceptionDescriptor> _descriptors = new();

    private Type? _sourceType;


    /// <summary>
    /// 源类型。
    /// </summary>
    public Type? SourceType
    {
        get
        {
            if (_sourceType is null)
                _sourceType = Source?.GetType();

            return _sourceType;
        }
        set
        {
            _sourceType = value;
        }
    }

    /// <summary>
    /// 源对象。
    /// </summary>
    public object? Source { private get; set; }

    /// <summary>
    /// 当前调用的拦截描述符。
    /// </summary>
    public InterceptionDescriptor? CurrentDescriptor { get; private set; }


    /// <summary>
    /// 移除指定拦截描述符。
    /// </summary>
    /// <param name="present">给定的 <see cref="InterceptionDescriptor"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Remove(InterceptionDescriptor present)
        => _descriptors.Remove(present);

    /// <summary>
    /// 移除指定拦截描述符。
    /// </summary>
    /// <param name="match">给定的断定 <see cref="InterceptionDescriptor"/> 方法。</param>
    /// <returns>返回移除的拦截描述符数。</returns>
    public int RemoveWhere(Predicate<InterceptionDescriptor> match)
        => _descriptors.RemoveWhere(match);


    /// <summary>
    /// 添加或更新拦截描述符集合。
    /// </summary>
    /// <param name="additives">给定的 <see cref="InterceptionDescriptor"/> 集合。</param>
    /// <param name="updateAction">给定的更新描述符方法。</param>
    /// <returns>返回 <see cref="IInterceptable"/>。</returns>
    public IInterceptable AddOrUpdateRange(IEnumerable<InterceptionDescriptor> additives,
        Func<(InterceptionDescriptor additive, InterceptionDescriptor present), InterceptionDescriptor>? updateAction = null)
    {
        foreach (var add in additives)
        {
            AddOrUpdate(add, updateAction);
        }

        return this;
    }

    /// <summary>
    /// 添加或更新拦截描述符。
    /// </summary>
    /// <param name="additive">给定的 <see cref="InterceptionDescriptor"/>。</param>
    /// <param name="updateAction">给定的更新描述符方法。</param>
    /// <returns>返回 <see cref="IInterceptable"/>。</returns>
    public IInterceptable AddOrUpdate(InterceptionDescriptor additive,
        Func<(InterceptionDescriptor additive, InterceptionDescriptor present), InterceptionDescriptor>? updateAction = null)
    {
        if (_descriptors.TryGetValue(additive, out var present))
        {
            var updateDescriptor = updateAction?.Invoke((additive, present));
            _descriptors.Remove(present);

            if (updateDescriptor is not null)
                _descriptors.Add(updateDescriptor);
        }
        else
        {
            _descriptors.Add(additive);
        }

        return this;
    }


    /// <summary>
    /// 调用方法。
    /// </summary>
    /// <param name="targetMethod">给定的 <see cref="MethodInfo"/>。</param>
    /// <param name="args">给定的参数数组。</param>
    /// <returns>返回调用结果。</returns>
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod is null)
            return null;

        ArgumentNullException.ThrowIfNull(Source);

        CurrentDescriptor = GetDescriptor(targetMethod, args);
        if (CurrentDescriptor is null)
            return targetMethod.Invoke(Source, args);

        // 调用前置动作
        CurrentDescriptor.PreAction?.Invoke(Source, CurrentDescriptor);

        CurrentDescriptor.InvokeValue = targetMethod.Invoke(Source, args);

        // 调用后置动作
        CurrentDescriptor.PostAction?.Invoke(Source, CurrentDescriptor);

        return CurrentDescriptor.InvokeValue;
    }

    /// <summary>
    /// 获取方法信息的拦截描述符。
    /// </summary>
    /// <param name="targetMethod">给定的 <see cref="MethodInfo"/>。</param>
    /// <param name="args">给定的方法参数数组。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor"/>。</returns>
    private InterceptionDescriptor? GetDescriptor(MethodInfo? targetMethod, object?[]? args)
    {
        foreach (var descriptor in _descriptors)
        {
            // 支持拦截方法的指定参数值
            if (descriptor.Equals(targetMethod, args))
                return descriptor;
        }

        return null;
    }

}
