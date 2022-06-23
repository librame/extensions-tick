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
/// 定义一个用于描述拦截信息的拦截描述符。
/// </summary>
/// <typeparam name="T">指定的实体类型。</typeparam>
public class InterceptionDescriptor<T> : InterceptionDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="InterceptionDescriptor"/>。
    /// </summary>
    /// <param name="memberName">给定的成员名称。</param>
    public InterceptionDescriptor(string memberName)
        : base(memberName)
    {
    }


    /// <summary>
    /// 使用指定的参数与当前其他信息创建一个新实例。
    /// </summary>
    /// <param name="parameterTypes">给定的参数类型数组。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor{T}"/>。</returns>
    public virtual new InterceptionDescriptor<T> WithParameters(Type[]? parameterTypes,
        object?[]? parameters)
    {
        return new(MemberName)
        {
            MemberFlags = MemberFlags,
            ParameterTypes = parameterTypes,
            Parameters = parameters,
            PreAction = PreAction,
            PostAction = PostAction
            //InvokeValue = InvokeValue
        };
    }


    /// <summary>
    /// 创建一个拦截属性获取器的拦截描述符。
    /// </summary>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyExpression">给定的属性表达式。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor{T}"/>。</returns>
    public static InterceptionDescriptor<T> InterceptPropertyGetter<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        => new($"{PropertyGetterPrefix}{propertyExpression.AsPropertyName()}");

    /// <summary>
    /// 创建一个拦截属性设置器的拦截描述符。
    /// </summary>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyExpression">给定的属性表达式。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor{T}"/>。</returns>
    public static InterceptionDescriptor<T> InterceptPropertySetter<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        => new($"{PropertySetterPrefix}{propertyExpression.AsPropertyName()}");

}


/// <summary>
/// 定义一个用于描述拦截信息的拦截描述符。
/// </summary>
public class InterceptionDescriptor : IEquatable<InterceptionDescriptor>, IEquatable<MethodInfo>
{
    /// <summary>
    /// 属性获取器前缀。
    /// </summary>
    public static readonly string PropertyGetterPrefix = "get_";

    /// <summary>
    /// 属性设置器前缀。
    /// </summary>
    public static readonly string PropertySetterPrefix = "set_";


    /// <summary>
    /// 构造一个 <see cref="InterceptionDescriptor"/>。
    /// </summary>
    /// <param name="memberName">给定的成员名称。</param>
    public InterceptionDescriptor(string memberName)
    {
        MemberName = memberName;
    }


    /// <summary>
    /// 成员名称。
    /// </summary>
    public string MemberName { get; init; }

    /// <summary>
    /// 成员标志。
    /// </summary>
    public BindingFlags? MemberFlags { get; set; }

    /// <summary>
    /// 参数类型数组。
    /// </summary>
    public Type[]? ParameterTypes { get; set; }

    /// <summary>
    /// 参数数组。
    /// </summary>
    public object?[]? Parameters { get; set; }

    /// <summary>
    /// 调用值。
    /// </summary>
    public object? InvokeValue { get; set; }

    /// <summary>
    /// 前置动作。
    /// </summary>
    public Action<object, InterceptionDescriptor>? PreAction { get; set; }

    /// <summary>
    /// 后置动作。
    /// </summary>
    public Action<object, InterceptionDescriptor>? PostAction { get; set; }


    /// <summary>
    /// 比较相等（支持方法重载）。
    /// </summary>
    /// <param name="other">给定的 <see cref="InterceptionDescriptor"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(InterceptionDescriptor? other)
    {
        // 不忽略大小写
        if (!MemberName.Equals(other?.MemberName, StringComparison.Ordinal))
            return false;

        if (ParameterTypes is null || ParameterTypes.Length == 0)
            return (other.ParameterTypes is null || other.ParameterTypes.Length == 0) ? true : false;

        if (other.ParameterTypes is null || other.ParameterTypes.Length == 0)
            return false;

        return ParameterTypes.SequenceEqual(other.ParameterTypes);
    }

    /// <summary>
    /// 比较相等（支持方法重载）。
    /// </summary>
    /// <param name="method">给定的 <see cref="MethodInfo"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(MethodInfo? method)
        => Equals(method, args: null);

    /// <summary>
    /// 比较相等（支持方法重载和指定参数值）。
    /// </summary>
    /// <param name="method">给定的 <see cref="MethodInfo"/>。</param>
    /// <param name="args">给定的方法参数数组。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(MethodInfo? method, object?[]? args)
    {
        // 首先比较名称（不忽略大小写）
        if (!MemberName.Equals(method?.Name, StringComparison.Ordinal))
            return false;

        // 其次比较参数类型（支持方法重载）
        var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
        if (!ParameterTypes.SequenceEqual(methodParameterTypes, bothNullReturn: true))
            return false;

        // 最后比较参数值
        return Parameters.SequenceEqual(args, bothNullReturn: true);
    }


    /// <summary>
    /// 使用指定的参数与当前其他信息创建一个新实例。
    /// </summary>
    /// <param name="parameterTypes">给定的参数类型数组。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor"/>。</returns>
    public virtual InterceptionDescriptor WithParameters(Type[]? parameterTypes,
        object?[]? parameters)
    {
        return new(MemberName)
        {
            MemberFlags = MemberFlags,
            ParameterTypes = parameterTypes,
            Parameters = parameters,
            PreAction = PreAction,
            PostAction = PostAction
            //InvokeValue = InvokeValue
        };
    }


    /// <summary>
    /// 创建一个拦截方法的拦截描述符。
    /// </summary>
    /// <param name="methodName">给定的方法名称。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor"/>。</returns>
    public static InterceptionDescriptor InterceptMethod(string methodName)
        => new(methodName);


    /// <summary>
    /// 创建一个拦截属性获取器的拦截描述符。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor"/>。</returns>
    public static InterceptionDescriptor InterceptPropertyGetter(string propertyName)
        => new($"{PropertyGetterPrefix}{propertyName}");

    /// <summary>
    /// 创建一个拦截属性设置器的拦截描述符。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <returns>返回 <see cref="InterceptionDescriptor"/>。</returns>
    public static InterceptionDescriptor InterceptPropertySetter(string propertyName)
        => new($"{PropertySetterPrefix}{propertyName}");

}
