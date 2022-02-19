#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Expression"/> 静态扩展。
/// </summary>
public static class ExpressionExtensions
{

    /// <summary>
    /// 作为属性表达式的名称。
    /// </summary>
    /// <exception cref="NotSupportedException">
    /// <paramref name="propertyExpression"/> not supported.
    /// </exception>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyExpression">给定的属性表达式。</param>
    /// <returns>返回字符串。</returns>
    public static string AsPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> propertyExpression)
    {
        return propertyExpression.Body switch
        {
            // 一元运算符
            UnaryExpression unary => ((MemberExpression)unary.Operand).Member.Name,
            // 访问的字段或属性
            MemberExpression member => member.Member.Name,
            // 参数表达式
            ParameterExpression parameter => parameter.Type.Name,
            // 默认
            _ => throw new NotSupportedException("Not supported property expression.")
        };
    }

    /// <summary>
    /// 如果指定类型实例的属性值。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TValue">指定的属性值类型。</typeparam>
    /// <param name="propertyExpression">给定的属性表达式。</param>
    /// <param name="source">给定要获取属性值的类型实例。</param>
    /// <returns>返回属性值。</returns>
    public static TValue? AsPropertyValue<T, TValue>(this Expression<Func<T, TValue>> propertyExpression, T source)
    {
        var name = propertyExpression.AsPropertyName();

        try
        {
            var pi = typeof(T).GetRuntimeProperty(name);
            return (TValue?)pi?.GetValue(source);
        }
        catch (AmbiguousMatchException)
        {
            return default;
        }
    }


    #region CreatePropertyExpression

    /// <summary>
    /// 创建用于比较大于属性值的 Lambda 表达式（例：p => p.PropertyName > compareValue）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定用于对比的属性名。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="propertyType">给定的属性类型（可选；默认使用参考值获取类型）。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreateGreaterThanPropertyExpression<T>(this string propertyName,
        object value, Type? propertyType = null)
        => propertyName.CreatePropertyExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            (p, c) => Expression.GreaterThan(p, c));

    /// <summary>
    /// 创建用于比较大于或等于属性值的 Lambda 表达式（例：p => p.PropertyName >= compareValue）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定用于对比的属性名。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="propertyType">给定的属性类型（可选；默认使用参考值获取类型）。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreateGreaterThanOrEqualPropertyExpression<T>(this string propertyName,
        object value, Type? propertyType = null)
        => propertyName.CreatePropertyExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            (p, c) => Expression.GreaterThanOrEqual(p, c));

    /// <summary>
    /// 创建用于比较小于属性值的 Lambda 表达式（例：p => p.PropertyName 〈 compareValue）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定用于对比的属性名。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="propertyType">给定的属性类型（可选；默认使用参考值获取类型）。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreateLessThanPropertyExpression<T>(this string propertyName,
        object value, Type? propertyType = null)
        => propertyName.CreatePropertyExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            (p, c) => Expression.LessThan(p, c));

    /// <summary>
    /// 创建用于比较小于或等于属性值的 Lambda 表达式（例：p => p.PropertyName 〈= compareValue）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定用于对比的属性名。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="propertyType">给定的属性类型（可选；默认使用参考值获取类型）。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreateLessThanOrEqualPropertyExpression<T>(this string propertyName,
        object value, Type? propertyType = null)
        => propertyName.CreatePropertyExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            (p, c) => Expression.LessThanOrEqual(p, c));

    /// <summary>
    /// 创建用于比较不等于属性值的 Lambda 表达式（例：p => p.PropertyName != compareValue）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定用于对比的属性名。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="propertyType">给定的属性类型（可选；默认使用参考值获取类型）。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreateNotEqualPropertyExpression<T>(this string propertyName,
        object value, Type? propertyType = null)
        => propertyName.CreatePropertyExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            (p, c) => Expression.NotEqual(p, c));

    /// <summary>
    /// 创建用于比较等于属性值的 Lambda 表达式（例：p => p.PropertyName is compareValue）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定用于对比的属性名。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="propertyType">给定的属性类型（可选；默认使用参考值获取类型）。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreateEqualPropertyExpression<T>(this string propertyName,
        object value, Type? propertyType = null)
        => propertyName.CreatePropertyExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            (p, c) => Expression.Equal(p, c));

    /// <summary>
    /// 创建用于比较属性值的 Lambda 表达式（例：p => p.PropertyName.CompareTo(value)）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <typeparam name="TExpression">指定的表达式类型。</typeparam>
    /// <param name="propertyName">给定的属性名。</param>
    /// <param name="propertyType">给定的属性类型。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="compareToFunc">给定的对比方法。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreatePropertyExpression<T, TExpression>(this string propertyName,
        Type propertyType, object? value, Func<MemberExpression, ConstantExpression, TExpression> compareToFunc)
        where TExpression : Expression
    {
        var p = Expression.Parameter(typeof(T), "p");
        var property = Expression.Property(p, propertyName);
        var constant = Expression.Constant(value, propertyType);

        // 调用方法（如：Expression.Equal(property, constant);）
        var body = compareToFunc(property, constant);

        // p => p.PropertyName is value
        return Expression.Lambda<Func<T, bool>>(body, p);
    }

    /// <summary>
    /// 创建用于比较属性值的 Lambda 表达式（例：p => p.PropertyName.CallMethodName(value)）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定的属性名。</param>
    /// <param name="propertyType">给定的属性类型。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="callMethodName">给定要调用的方法名。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreatePropertyExpression<T>(this string propertyName,
        Type propertyType, object? value, string callMethodName)
    {
        var type = typeof(T);

        var p = Expression.Parameter(type, "p");
        var property = Expression.Property(p, propertyName);
        var constant = Expression.Constant(value, propertyType);

        var propertyInfo = type.GetRuntimeProperty(propertyName);
        if (propertyInfo is null)
            throw new ArgumentException($"The property '{type}' with the specified name '{propertyName}' was not found.");

        var method = propertyInfo.PropertyType.GetRuntimeMethod(callMethodName, new Type[] { propertyType });
        if (method is null)
            throw new ArgumentException($"The method '{type}' with the specified name '{propertyName}' was not found.");

        var body = Expression.Call(property, method, constant);

        // p => p.PropertyName.CallMethodName(value)
        return Expression.Lambda<Func<T, bool>>(body, p);
    }


    /// <summary>
    /// 创建指定属性名的 Lambda 表达式（例：p => p.PropertyName）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定的属性名。</param>
    /// <returns>返回 lambda 表达式。</returns>
    public static Expression<Func<T, object>> CreatePropertyExpression<T>(this string propertyName)
    {
        var p = Expression.Parameter(typeof(T), "p");

        var property = Expression.Property(p, propertyName);

        // p => p.PropertyName
        return Expression.Lambda<Func<T, object>>(property, p);
    }

    #endregion


    #region Join

    /// <summary>
    /// 使用和运算逻辑连接两个表达式。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="expr1">给定的表达式1。</param>
    /// <param name="expr2">给定的表达式2。</param>
    /// <returns>返回 <see cref="Expression"/>。</returns>
    public static Expression<Func<T, TProperty>> AndAlso<T, TProperty>(
        this Expression<Func<T, TProperty>> expr1,
        Expression<Func<T, TProperty>> expr2)
        => expr1.Join(expr2, Expression.AndAlso);

    /// <summary>
    /// 使用或运算逻辑连接两个表达式。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="expr1">给定的表达式1。</param>
    /// <param name="expr2">给定的表达式2。</param>
    /// <returns>返回 <see cref="Expression"/>。</returns>
    public static Expression<Func<T, TProperty>> OrElse<T, TProperty>(
        this Expression<Func<T, TProperty>> expr1,
        Expression<Func<T, TProperty>> expr2)
        => expr1.Join(expr2, Expression.OrElse);

    /// <summary>
    /// 按指定的方法连接两个表达式。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="expr1">给定的表达式1。</param>
    /// <param name="expr2">给定的表达式2。</param>
    /// <param name="func">给定的连接方法。</param>
    /// <returns>返回 <see cref="Expression"/>。</returns>
    public static Expression<Func<T, TProperty>> Join<T, TProperty>(this Expression<Func<T, TProperty>> expr1,
        Expression<Func<T, TProperty>> expr2, Func<Expression, Expression, BinaryExpression> func)
    {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new Core.ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);
        ArgumentNullException.ThrowIfNull(left);

        var rightVisitor = new Core.ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);
        ArgumentNullException.ThrowIfNull(right);

        return Expression.Lambda<Func<T, TProperty>>(
            func(left, right), parameter);
    }

    #endregion


    #region NewByExpression

    /// <summary>
    /// 通过表达式新建无参类型实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <returns>返回创建的 <typeparamref name="T"/>。</returns>
    public static T New<T>()
    {
        var newExpression = Expression.New(typeof(T));
        var lambda = Expression.Lambda<Func<T>>(newExpression, parameters: null);
        return lambda.Compile()();
    }

    /// <summary>
    /// 通过表达式使用参数数组新建类型实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="parameter">给定的对象参数。</param>
    /// <param name="parameterType">给定的对象参数类型（可选；默认使用对象参数获取类型）。</param>
    /// <returns>返回创建的 <typeparamref name="T"/>。</returns>
    public static T New<T>(object parameter, Type? parameterType = null)
        => (T)typeof(T).NewByExpression(parameter, parameterType);

    /// <summary>
    /// 通过表达式使用参数数组新建类型实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="parameters">给定的对象参数数组。</param>
    /// <returns>返回创建的 <typeparamref name="T"/>。</returns>
    public static T New<T>(params object[] parameters)
        => (T)typeof(T).NewByExpression(parameters, parameterTypes: null);

    /// <summary>
    /// 通过表达式使用参数数组新建类型实例。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="parameters">给定的对象参数数组（可选）。</param>
    /// <param name="parameterTypes">给定的对象参数类型数组（可选；默认使用对象参数获取类型）。</param>
    /// <returns>返回创建的 <typeparamref name="T"/>。</returns>
    public static T New<T>(object[]? parameters = null, Type[]? parameterTypes = null)
        => (T)typeof(T).NewByExpression(parameters, parameterTypes);


    /// <summary>
    /// 通过表达式使用单个参数新建对象。
    /// </summary>
    /// <param name="type">给定的对象类型。</param>
    /// <param name="parameter">给定的对象参数。</param>
    /// <param name="parameterType">给定的对象参数类型（可选；默认使用对象参数获取类型）。</param>
    /// <returns>返回创建的对象。</returns>
    public static object NewByExpression(this Type type, object parameter, Type? parameterType = null)
        => type.NewByExpression(new object[] { parameter }, new Type[] { parameterType ?? parameter.GetType() });

    /// <summary>
    /// 通过表达式使用单个参数新建对象。
    /// </summary>
    /// <param name="type">给定的对象类型。</param>
    /// <param name="parameters">给定的对象参数数组。</param>
    /// <returns>返回创建的对象。</returns>
    public static object NewByExpression(this Type type, params object[] parameters)
        => type.NewByExpression(parameters, parameterType: null);

    /// <summary>
    /// 通过表达式使用参数数组新建对象。
    /// </summary>
    /// <param name="type">给定的对象类型。</param>
    /// <param name="parameters">给定的对象参数数组（可选）。</param>
    /// <param name="parameterTypes">给定的对象参数类型数组（可选；默认使用对象参数获取类型）。</param>
    /// <returns>返回创建的对象。</returns>
    public static object NewByExpression(this Type type, object[]? parameters = null, Type[]? parameterTypes = null)
    {
        if (parameters is null || parameters.Length < 1)
        {
            var newExpression = Expression.New(type);
            var lambda = Expression.Lambda<Func<object>>(newExpression, parameters: null);
            return lambda.Compile()();
        }
        else
        {
            if (parameterTypes is null)
                parameterTypes = parameters.Select(s => s.GetType()).ToArray();

            var constructor = type.GetConstructor(parameterTypes);
            if (constructor is null)
                throw new ArgumentException($"No ConstructorInfo matching the specified parameters was found in the type '{type}'.");

            var argsExpression = Expression.Parameter(typeof(object[]), "args");

            var parameterExpressions = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var argExpression = Expression.ArrayIndex(argsExpression, Expression.Constant(i));
                parameterExpressions[i] = Expression.Convert(argExpression, parameterTypes[i]);
            }

            var newExpression = Expression.New(constructor, parameterExpressions);
            var lambda = Expression.Lambda<Func<object[], object>>(newExpression, argsExpression);

            return lambda.Compile()(parameters);
        }
    }

    #endregion

}
