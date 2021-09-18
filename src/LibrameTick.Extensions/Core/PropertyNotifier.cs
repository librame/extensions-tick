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
/// 定义实现 <see cref="IPropertyNotifier"/> 的属性通知器。
/// </summary>
public sealed class PropertyNotifier : IPropertyNotifier
{
    private readonly ConcurrentDictionary<PropertyNoticeNamedKey, object> _propertyValues;
    private readonly ConcurrentDictionary<PropertyNoticeNamedKey, Func<object>> _propertyFuncs;

    private readonly TypeNamedKey _baseKey;


    /// <summary>
    /// 构造一个 <see cref="PropertyNotifier"/>。
    /// </summary>
    /// <param name="source">给定的属性源。</param>
    /// <param name="sourceAliase">给定的属性源别名（可选）。</param>
    public PropertyNotifier(object source, string? sourceAliase = null)
        : this(source, sourceAliase, parentNotifier: null, propertyValues: null, propertyFuncs: null)
    {
    }

    private PropertyNotifier(object source, string? sourceAliase,
        IPropertyNotifier? parentNotifier,
        ConcurrentDictionary<PropertyNoticeNamedKey, object>? propertyValues,
        ConcurrentDictionary<PropertyNoticeNamedKey, Func<object>>? propertyFuncs)
    {
        _propertyValues = propertyValues ?? new ConcurrentDictionary<PropertyNoticeNamedKey, object>();
        _propertyFuncs = propertyFuncs ?? new ConcurrentDictionary<PropertyNoticeNamedKey, Func<object>>();

        _baseKey = new TypeNamedKey(source.GetType(), sourceAliase);

        Source = source;
        SourceAliase = sourceAliase;
        ParentNotifier = parentNotifier;
    }


    /// <summary>
    /// 属性源。
    /// </summary>
    public object Source { get; init; }

    /// <summary>
    /// 属性源别名。
    /// </summary>
    public string? SourceAliase { get; init; }


    /// <summary>
    /// 父级属性通知器。
    /// </summary>
    public IPropertyNotifier? ParentNotifier { get; init; }


    /// <summary>
    /// 属性改变时事件。
    /// </summary>
    public event PropertyChangingEventHandler? PropertyChanging;

    /// <summary>
    /// 属性改变后事件。
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;


    /// <summary>
    /// 构建属性通知键。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <returns>返回 <see cref="PropertyNoticeNamedKey"/>。</returns>
    public PropertyNoticeNamedKey BuildKey(string propertyName)
        => new PropertyNoticeNamedKey(propertyName, _baseKey);


    private void HandleNotice(string propertyName, object? changingValue, object? oldValue,
        bool isUpdate, Action action)
    {
        // 调用属性改变时事件处理程序
        PropertyChanging?.Invoke(this,
            new NotifyPropertyChangingEventArgs(propertyName, changingValue, oldValue, isUpdate));

        action.Invoke();

        // 调用属性改变后事件处理程序
        PropertyChanged?.Invoke(this,
            new NotifyPropertyChangedEventArgs(propertyName, changingValue, oldValue, isUpdate));
    }


    /// <summary>
    /// 添加或更新属性值。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="propertyValue">给定的属性值对象。</param>
    /// <param name="addedOrUpdatedAction">成功添加或更新后的动作（可选）。</param>
    /// <returns>返回属性值对象。</returns>
    public object AddOrUpdate(string propertyName, object propertyValue, Action<object>? addedOrUpdatedAction = null)
    {
        if (propertyValue is null)
            throw new ArgumentException($"The value of the property '{propertyName}' to be added cannot be null.");

        var key = BuildKey(propertyName);

        if (_propertyValues.ContainsKey(key))
        {
            var oldValue = _propertyValues[key];

            HandleNotice(propertyName, propertyValue, oldValue, isUpdate: true, () =>
            {
                _propertyValues[key] = propertyValue;

                addedOrUpdatedAction?.Invoke(propertyValue);
            });
        }
        else
        {
            HandleNotice(propertyName, propertyValue, oldValue: null, isUpdate: false, () =>
            {
                _propertyValues.TryAdd(key, propertyValue);

                addedOrUpdatedAction?.Invoke(propertyValue);
            });
        }

        return propertyValue;
    }

    /// <summary>
    /// 添加或更新属性值。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="propertyValueFunc">给定的属性值对象方法（默认初始会执行一次，以便支持事件参数集合调用）。</param>
    /// <param name="isInitializeValue">是否初始化属性值（如果使用初始化，则表示立即执行方法，并将执行结果缓存；反之则在每次获取属性值时再执行方法。可选；默认不初始化）。</param>
    /// <param name="addedOrUpdatedAction">成功添加或更新后的动作（可选）。</param>
    /// <returns>返回属性值对象（如果已初始化属性值）或值对象方法。</returns>
    public object AddOrUpdate(string propertyName, Func<object> propertyValueFunc, bool isInitializeValue = false,
        Action<object>? addedOrUpdatedAction = null)
    {
        if (propertyValueFunc is null)
            throw new ArgumentException($"The value func of the property '{propertyName}' to be added cannot be null.");

        var key = BuildKey(propertyName);

        if (!isInitializeValue)
        {
            if (_propertyFuncs.ContainsKey(key))
            {
                var oldValueFunc = _propertyFuncs[key];

                HandleNotice(propertyName, propertyValueFunc, oldValueFunc, isUpdate: true, () =>
                {
                    _propertyFuncs[key] = propertyValueFunc;

                    addedOrUpdatedAction?.Invoke(propertyValueFunc);
                });
            }
            else
            {
                HandleNotice(propertyName, propertyValueFunc, oldValue: null, isUpdate: false, () =>
                {
                    _propertyFuncs.TryAdd(key, propertyValueFunc);

                    addedOrUpdatedAction?.Invoke(propertyValueFunc);
                });
            }

            return propertyValueFunc;
        }

        return AddOrUpdate(propertyName, propertyValueFunc.Invoke());
    }


    /// <summary>
    /// 获取或添加属性值。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="addPropertyValue">给定的默认值。</param>
    /// <param name="addedAction">成功添加后的动作（可选）。</param>
    /// <param name="gotAction">成功获取后的动作（可选）。</param>
    /// <returns>返回属性值实例。</returns>
    public TValue GetOrAdd<TValue>(string propertyName, TValue addPropertyValue,
        Action<TValue>? addedAction = null, Action<TValue>? gotAction = null)
    {
        var key = BuildKey(propertyName);

        if (_propertyValues.ContainsKey(key))
        {
            var currentValue = (TValue)_propertyValues[key];
            gotAction?.Invoke(currentValue);

            return currentValue;
        }

        if (_propertyFuncs.ContainsKey(key))
        {
            var currentValue = (TValue)_propertyFuncs[key].Invoke();
            gotAction?.Invoke(currentValue);

            return currentValue;
        }

        if (addPropertyValue is null)
            throw new ArgumentException($"The value of the property '{propertyName}' to be added cannot be null.");

        _propertyValues.TryAdd(key, addPropertyValue);

        addedAction?.Invoke(addPropertyValue);

        return addPropertyValue;
    }

    /// <summary>
    /// 获取或添加属性值。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="addPropertyValue">给定的默认值。</param>
    /// <param name="addedAction">成功添加后的动作（可选）。</param>
    /// <param name="gotAction">成功获取后的动作（可选）。</param>
    /// <returns>返回属性值对象。</returns>
    public object GetOrAdd(string propertyName, object addPropertyValue,
        Action<object>? addedAction = null, Action<object>? gotAction = null)
    {
        var key = BuildKey(propertyName);

        if (_propertyValues.ContainsKey(key))
        {
            var currentValue = _propertyValues[key];
            gotAction?.Invoke(currentValue);

            return currentValue;
        }

        if (_propertyFuncs.ContainsKey(key))
        {
            var currentValue = _propertyFuncs[key].Invoke();
            gotAction?.Invoke(currentValue);

            return currentValue;
        }

        if (addPropertyValue is null)
            throw new ArgumentException($"The value of the property '{propertyName}' to be added cannot be null.");

        _propertyValues.TryAdd(key, addPropertyValue);

        addedAction?.Invoke(addPropertyValue);

        return addPropertyValue;
    }


    /// <summary>
    /// 尝试获取属性值。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="propertyValue">输出可能存在的属性值。</param>
    /// <returns>返回是否已移除的布尔值。</returns>
    public bool TryGetValue(string propertyName, [MaybeNullWhen(false)] out object propertyValue)
    {
        var key = BuildKey(propertyName);

        if (_propertyValues.TryGetValue(key, out propertyValue))
            return true;

        if (_propertyFuncs.TryGetValue(key, out var factory))
        {
            propertyValue = factory.Invoke();
            return true;
        }

        return false;
    }


    /// <summary>
    /// 尝试移除属性值。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="propertyValue">输出可能存在的属性值。</param>
    /// <returns>返回是否已移除的布尔值。</returns>
    public bool TryRemoveValue(string propertyName, [MaybeNullWhen(false)] out object propertyValue)
    {
        var key = BuildKey(propertyName);

        if (_propertyValues.TryRemove(key, out propertyValue))
            return true;

        if (_propertyFuncs.TryRemove(key, out var factory))
        {
            propertyValue = factory.Invoke();
            return true;
        }

        return false;
    }


    /// <summary>
    /// 使用新属性源创建一个 <see cref="IPropertyNotifier"/>。
    /// </summary>
    /// <param name="newSource">给定的新属性源。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    /// <returns>返回 <see cref="IPropertyNotifier"/>。</returns>
    public IPropertyNotifier WithSource(object newSource, string? sourceAliase = null)
        => new PropertyNotifier(newSource, sourceAliase, this, _propertyValues, _propertyFuncs);

}
