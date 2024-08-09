#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制成员信息。
/// </summary>
/// <remarks>
/// 使用成员信息与序列化选项构造一个 <see cref="BinaryMemberInfo"/> 实例。
/// </remarks>
/// <param name="info">给定的 <see cref="MemberInfo"/>。</param>
/// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
/// <param name="orderId">给定的顺序标识。</param>
public class BinaryMemberInfo(MemberInfo info, BinarySerializerOptions options, int orderId)
{
    private readonly FieldInfo? _fieldInfo = info as FieldInfo;
    private readonly PropertyInfo? _propertyInfo = info as PropertyInfo;


    /// <summary>
    /// 获取或设置声明类型级联标识。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int DeclaringTypeCascadeId { get; set; }

    /// <summary>
    /// 获取声明类型。
    /// </summary>
    /// <value>
    /// 返回类型实例。
    /// </value>
    public Type? DeclaringType { get; init; } = info.DeclaringType;

    /// <summary>
    /// 获取成员信息。
    /// </summary>
    /// <value>
    /// 返回 <see cref="MemberInfo"/>。
    /// </value>
    public MemberInfo Info { get; init; } = info;

    /// <summary>
    /// 获取序列化选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="BinarySerializerOptions"/>。
    /// </value>
    public BinarySerializerOptions Options { get; init; } = options;

    /// <summary>
    /// 获取顺序标识。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int OrderId { get; init; } = orderId;

    /// <summary>
    /// 获取成员名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public string Name { get; init; } = info.Name;


    /// <summary>
    /// 获取当前成员是否可写。
    /// </summary>
    /// <value>
    /// 返回可写的布尔值。
    /// </value>
    public bool CanWrite => _propertyInfo?.CanWrite ?? !(_fieldInfo!.IsInitOnly || _fieldInfo.IsLiteral);

    /// <summary>
    /// 获取当前成员是否可读。
    /// </summary>
    /// <value>
    /// 返回可读的布尔值。
    /// </value>
    public bool CanRead => _propertyInfo?.CanRead ?? true;


    /// <summary>
    /// 构建成员表达式。
    /// </summary>
    /// <param name="parameterExpression">给定的对象 <see cref="Expression"/>。</param>
    /// <returns>返回 <see cref="MemberExpression"/>。</returns>
    public MemberExpression BuildExpression(Expression parameterExpression)
    {
        return _propertyInfo is not null
            ? Expression.Property(parameterExpression, _propertyInfo)
            : Expression.Field(parameterExpression, _fieldInfo!);
    }


    /// <summary>
    /// 获取必要的成员类型。
    /// </summary>
    /// <returns>返回类型实例。</returns>
    public Type GetRequiredType()
    {
        var memberType = _propertyInfo?.PropertyType ?? _fieldInfo?.FieldType;
        ArgumentNullException.ThrowIfNull(memberType);

        return memberType;
    }


    /// <summary>
    /// 获取当前成员的自定义特性。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <returns>返回 <typeparamref name="TAttribute"/>。</returns>
    public TAttribute? GetCustomAttribute<TAttribute>()
        where TAttribute : Attribute
        => Info.GetCustomAttribute<TAttribute>();

    /// <summary>
    /// 获取当前成员的必要自定义特性。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <returns>返回 <typeparamref name="TAttribute"/>。</returns>
    /// <exception cref="InvalidOperationException">
    /// The input type member must be marked with the <typeparamref name="TAttribute"/> attribute.
    /// </exception>
    public TAttribute GetRequiredAttribute<TAttribute>()
        where TAttribute : Attribute
    {
        var attribute = Info.GetCustomAttribute<TAttribute>();
        if (attribute is null)
        {
            throw new InvalidOperationException($"The '{Info.Name}' member must be marked with the '{typeof(TAttribute).Name}' attribute.");
        }

        return attribute;
    }


    /// <summary>
    /// 获取指定对象的成员值。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    public object? GetValue(object? obj)
        => _propertyInfo?.GetValue(obj) ?? _fieldInfo?.GetValue(obj);

    /// <summary>
    /// 设置指定对象的成员值。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <param name="value">给定的成员值。</param>
    public void SetValue(object? obj, object? value)
    {
        if (_propertyInfo is not null)
        {
            _propertyInfo.SetValue(obj, value);
            return;
        }

        _fieldInfo?.SetValue(obj, value);
    }

}
