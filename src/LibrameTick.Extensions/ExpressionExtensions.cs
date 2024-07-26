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


    #region GetMember

    /// <summary>
    /// 通过表达式获取调用无参数方法的动作（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回无参数方法的动作。</returns>
    public static Action<TSrc> GetMethodActionByExpression<TSrc>(this string methodName, Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Action<TSrc>>(typeof(TSrc), genericTypesFunc, []);

    /// <summary>
    /// 通过表达式获取调用带参数方法的动作（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg">指定要调用方法的参数类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的动作。</returns>
    public static Action<TSrc, TArg> GetMethodActionByExpression<TSrc, TArg>(this string methodName,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Action<TSrc, TArg>>(typeof(TSrc), genericTypesFunc, [typeof(TArg)]);

    /// <summary>
    /// 通过表达式获取调用带参数方法的动作（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的动作。</returns>
    public static Action<TSrc, TArg0, TArg1> GetMethodActionByExpression<TSrc, TArg0, TArg1>(this string methodName,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Action<TSrc, TArg0, TArg1>>(typeof(TSrc), genericTypesFunc, [typeof(TArg0), typeof(TArg1)]);

    /// <summary>
    /// 通过表达式获取调用带参数方法的动作（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TArg2">指定要调用方法的参数2类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的动作。</returns>
    public static Action<TSrc, TArg0, TArg1, TArg2> GetMethodActionByExpression<TSrc, TArg0, TArg1, TArg2>(this string methodName,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Action<TSrc, TArg0, TArg1, TArg2>>(typeof(TSrc), genericTypesFunc,
            [typeof(TArg0), typeof(TArg1), typeof(TArg2)]);

    /// <summary>
    /// 通过表达式获取调用带参数方法的动作（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TArg2">指定要调用方法的参数2类型。</typeparam>
    /// <typeparam name="TArg3">指定要调用方法的参数3类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的动作。</returns>
    public static Action<TSrc, TArg0, TArg1, TArg2, TArg3> GetMethodActionByExpression<TSrc, TArg0, TArg1, TArg2, TArg3>(
        this string methodName, Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Action<TSrc, TArg0, TArg1, TArg2, TArg3>>(typeof(TSrc), genericTypesFunc,
            [typeof(TArg0), typeof(TArg1), typeof(TArg2), typeof(TArg3)]);


    /// <summary>
    /// 通过表达式获取调用无参数方法的结果值（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TResult">指定的方法结果类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="source">给定的来源实例（如果调用静态方法，此实例可为 NULL）。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回结果值。</returns>
    public static TResult GetMethodValueByExpression<TSrc, TResult>(this string methodName, TSrc? source,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodFuncByExpression<TSrc, TResult>(genericTypesFunc)(source!);

    /// <summary>
    /// 通过表达式获取调用带参数方法的结果值（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg">指定要调用方法的参数类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="source">给定的来源实例（如果调用静态方法，此实例可为 NULL）。</param>
    /// <param name="arg">给定的方法参数。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回结果值。</returns>
    public static TResult GetMethodValueByExpression<TSrc, TArg, TResult>(this string methodName, TSrc? source, TArg arg,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodFuncByExpression<TSrc, TArg, TResult>(genericTypesFunc)(source!, arg);

    /// <summary>
    /// 通过表达式获取调用带参数方法的结果值（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="source">给定的来源实例（如果调用静态方法，此实例可为 NULL）。</param>
    /// <param name="arg0">给定的方法参数0。</param>
    /// <param name="arg1">给定的方法参数1。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回结果值。</returns>
    public static TResult GetMethodValueByExpression<TSrc, TArg0, TArg1, TResult>(this string methodName,
        TSrc? source, TArg0 arg0, TArg1 arg1, Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodFuncByExpression<TSrc, TArg0, TArg1, TResult>(genericTypesFunc)(source!, arg0, arg1);

    /// <summary>
    /// 通过表达式获取调用带参数方法的结果值（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TArg2">指定要调用方法的参数2类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="source">给定的来源实例（如果调用静态方法，此实例可为 NULL）。</param>
    /// <param name="arg0">给定的方法参数0。</param>
    /// <param name="arg1">给定的方法参数1。</param>
    /// <param name="arg2">给定的方法参数2。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回结果值。</returns>
    public static TResult GetMethodValueByExpression<TSrc, TArg0, TArg1, TArg2, TResult>(this string methodName,
        TSrc? source, TArg0 arg0, TArg1 arg1, TArg2 arg2, Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodFuncByExpression<TSrc, TArg0, TArg1, TArg2, TResult>(genericTypesFunc)(source!, arg0, arg1, arg2);

    /// <summary>
    /// 通过表达式获取调用带参数方法的结果值（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TArg2">指定要调用方法的参数2类型。</typeparam>
    /// <typeparam name="TArg3">指定要调用方法的参数3类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="source">给定的来源实例（如果调用静态方法，此实例可为 NULL）。</param>
    /// <param name="arg0">给定的方法参数0。</param>
    /// <param name="arg1">给定的方法参数1。</param>
    /// <param name="arg2">给定的方法参数2。</param>
    /// <param name="arg3">给定的方法参数3。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回结果值。</returns>
    public static TResult GetMethodValueByExpression<TSrc, TArg0, TArg1, TArg2, TArg3, TResult>(this string methodName,
        TSrc? source, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodFuncByExpression<TSrc, TArg0, TArg1, TArg2, TArg3, TResult>(genericTypesFunc)(source!, arg0, arg1, arg2, arg3);


    /// <summary>
    /// 通过表达式获取调用无参数方法的方法（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回无参方法的方法。</returns>
    public static Func<TSrc, TResult> GetMethodFuncByExpression<TSrc, TResult>(this string methodName,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Func<TSrc, TResult>>(typeof(TSrc), genericTypesFunc, []);

    /// <summary>
    /// 通过表达式获取调用带参数方法的方法（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg">指定要调用方法的参数类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的方法。</returns>
    public static Func<TSrc, TArg, TResult> GetMethodFuncByExpression<TSrc, TArg, TResult>(this string methodName,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Func<TSrc, TArg, TResult>>(typeof(TSrc), genericTypesFunc, [typeof(TArg)]);

    /// <summary>
    /// 通过表达式获取调用带参数方法的方法（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的方法。</returns>
    public static Func<TSrc, TArg0, TArg1, TResult> GetMethodFuncByExpression<TSrc, TArg0, TArg1, TResult>(this string methodName,
        Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Func<TSrc, TArg0, TArg1, TResult>>(typeof(TSrc), genericTypesFunc,
            [typeof(TArg0), typeof(TArg1)]);

    /// <summary>
    /// 通过表达式获取调用带参数方法的方法（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TArg2">指定要调用方法的参数2类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的方法。</returns>
    public static Func<TSrc, TArg0, TArg1, TArg2, TResult> GetMethodFuncByExpression<TSrc, TArg0, TArg1, TArg2, TResult>(
        this string methodName, Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Func<TSrc, TArg0, TArg1, TArg2, TResult>>(typeof(TSrc), genericTypesFunc,
            [typeof(TArg0), typeof(TArg1), typeof(TArg2)]);

    /// <summary>
    /// 通过表达式获取调用带参数方法的方法（支持非公开与静态方法成员。如果调用泛型方法定义，则参数类型集合前N个参数依次对应泛型方法形参类型）。
    /// </summary>
    /// <typeparam name="TSrc">指定要调用方法的来源类型。</typeparam>
    /// <typeparam name="TArg0">指定要调用方法的参数0类型。</typeparam>
    /// <typeparam name="TArg1">指定要调用方法的参数1类型。</typeparam>
    /// <typeparam name="TArg2">指定要调用方法的参数2类型。</typeparam>
    /// <typeparam name="TArg3">指定要调用方法的参数3类型。</typeparam>
    /// <typeparam name="TResult">指定的返回值类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <returns>返回带参数方法的方法。</returns>
    public static Func<TSrc, TArg0, TArg1, TArg2, TArg3, TResult> GetMethodFuncByExpression<TSrc, TArg0, TArg1, TArg2, TArg3, TResult>(
        this string methodName, Func<Type[], Type[]>? genericTypesFunc = null)
        => methodName.GetMethodByExpression<Func<TSrc, TArg0, TArg1, TArg2, TArg3, TResult>>(typeof(TSrc), genericTypesFunc,
            [typeof(TArg0), typeof(TArg1), typeof(TArg2), typeof(TArg3)]);

    /// <summary>
    /// 通过表达式获取调用方法（支持非公开与静态方法成员，也支持调用泛型方法定义）。
    /// </summary>
    /// <typeparam name="TResult">指定调用方法的结果类型。</typeparam>
    /// <param name="methodName">给定的方法名称。</param>
    /// <param name="souceType">给定要调用方法的来源类型。</param>
    /// <param name="genericTypesFunc">给定要调用泛型方法的实际参数类型集合（如果是非泛型方法，此参数可为空；传入参数为泛型参数定义集合）。</param>
    /// <param name="argumentTypes">给定要调用方法的参数类型集合。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    /// <exception cref="ArgumentException">
    /// The method with the specified name was not found.
    /// </exception>
    public static TResult GetMethodByExpression<TResult>(this string methodName, Type souceType, Func<Type[], Type[]>? genericTypesFunc,
        params Type[] argumentTypes)
    {
        var srcPara = Expression.Parameter(souceType, "p");

        var argParas = argumentTypes.Length > 0
            ? argumentTypes.Select((s, i) => Expression.Parameter(s, $"a{i}")).ToArray()
            : [];

        var method = souceType.GetMethod(methodName, TypeExtensions.AllMemberFlagsWithStatic, argumentTypes)
            ?? souceType.GetMethods(TypeExtensions.AllMemberFlagsWithStatic)
                .Where(p => p.Name == methodName && p.IsGenericMethodDefinition)
                .FirstOrDefault() // 尝试查找首个同名的泛型方法定义
            ?? throw new ArgumentException($"The method with the specified name '{methodName}' was not found.");

        // 泛型方法定义需手动注入泛型参数
        if (method.IsGenericMethodDefinition)
        {
            ArgumentNullException.ThrowIfNull(genericTypesFunc);

            var genericTypes = genericTypesFunc(method.GetGenericArguments());
            method = method.MakeGenericMethod(genericTypes);
        }

        var methodCall = method.IsStatic
            ? Expression.Call(method, argParas)
            : Expression.Call(srcPara, method, argParas);

        var lambda = Expression.Lambda<TResult>(methodCall, EnumerableExtensions.Combine(srcPara, argParas));
        return lambda.Compile();
    }


    /// <summary>
    /// 通过表达式获取字段值（支持非公开与静态字段成员）。
    /// </summary>
    /// <typeparam name="TSource">指定折来源类型。</typeparam>
    /// <typeparam name="TField">指定的字段类型。</typeparam>
    /// <param name="fieldName">给定的字段名称。</param>
    /// <param name="source">给定的来源实例（如果调用静态字段，此实例可为 NULL）。</param>
    /// <returns>返回字段值。</returns>
    public static TField GetFieldValueByExpression<TSource, TField>(this string fieldName, TSource? source)
        => fieldName.GetFieldFuncByExpression<TSource, TField>()(source!);

    /// <summary>
    /// 通过表达式获取属性值（支持非公开与静态属性成员）。
    /// </summary>
    /// <typeparam name="TSource">指定折来源类型。</typeparam>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="source">给定的来源实例（如果调用静态属性，此实例可为 NULL）。</param>
    /// <returns>返回属性值。</returns>
    public static TProperty GetPropertyValueByExpression<TSource, TProperty>(this string propertyName, TSource? source)
        => propertyName.GetPropertyFuncByExpression<TSource, TProperty>()(source!);


    /// <summary>
    /// 通过表达式获取字段方法（支持非公开与静态字段成员）。
    /// </summary>
    /// <typeparam name="TSource">指定折来源类型。</typeparam>
    /// <typeparam name="TField">指定的字段类型。</typeparam>
    /// <param name="fieldName">给定的字段名称。</param>
    /// <returns>返回字段方法。</returns>
    public static Func<TSource, TField> GetFieldFuncByExpression<TSource, TField>(this string fieldName)
        => GetMemberFunc<TSource, TField>(fieldName, MemberTypes.Field);

    /// <summary>
    /// 通过表达式获取属性方法（支持非公开与静态属性成员）。
    /// </summary>
    /// <typeparam name="TSource">指定折来源类型。</typeparam>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <returns>返回属性方法。</returns>
    public static Func<TSource, TProperty> GetPropertyFuncByExpression<TSource, TProperty>(this string propertyName)
        => GetMemberFunc<TSource, TProperty>(propertyName, MemberTypes.Property);

    private static Func<TSource, TMember> GetMemberFunc<TSource, TMember>(string memberName, MemberTypes memberType)
    {
        var srcType = typeof(TSource);
        var srcPara = Expression.Parameter(srcType, "p");

        MemberExpression? memberExpression;

        if (memberType == MemberTypes.Field)
        {
            var field = srcType.GetField(memberName, TypeExtensions.AllMemberFlagsWithStatic)
                ?? throw new ArgumentException($"The field with the specified name '{memberName}' was not found.");

            memberExpression = field.IsStatic
                ? Expression.Field(expression: null, field)
                : Expression.Field(srcPara, field);
        }
        else if (memberType == MemberTypes.Property)
        {
            var property = srcType.GetProperty(memberName, TypeExtensions.AllMemberFlagsWithStatic)
                ?? throw new ArgumentException($"The property with the specified name '{memberName}' was not found.");

            var isStatic = ((property.GetMethod ?? property.SetMethod)?.IsStatic) ?? false;

            memberExpression = isStatic
                ? Expression.Property(expression: null, property)
                : Expression.Property(srcPara, property);
        }
        else
        {
            throw new NotSupportedException($"Unsupported member type '{memberType}'.");
        }

        var lambda = Expression.Lambda<Func<TSource, TMember>>(memberExpression, srcPara);
        return lambda.Compile();
    }

    #endregion


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
        => propertyName.CreatePropertyMethodExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            Expression.GreaterThan);

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
        => propertyName.CreatePropertyMethodExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            Expression.GreaterThanOrEqual);

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
        => propertyName.CreatePropertyMethodExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            Expression.LessThan);

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
        => propertyName.CreatePropertyMethodExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            Expression.LessThanOrEqual);

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
        => propertyName.CreatePropertyMethodExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            Expression.NotEqual);

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
        => propertyName.CreatePropertyMethodExpression<T, BinaryExpression>(propertyType ?? value.GetType(), value,
            Expression.Equal);

    /// <summary>
    /// 创建用于比较属性值特定方法的 Lambda 表达式（例：p => p.PropertyName.CompareTo(value)）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <typeparam name="TExpression">指定的表达式类型。</typeparam>
    /// <param name="propertyName">给定的属性名。</param>
    /// <param name="propertyType">给定的属性类型。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="compareToFunc">给定的对比方法。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreatePropertyMethodExpression<T, TExpression>(this string propertyName,
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
    /// 创建用于比较属性值特定方法的 Lambda 表达式（例：p => p.PropertyName.CallMethodName(value)）。
    /// </summary>
    /// <typeparam name="T">指定的实体类型。</typeparam>
    /// <param name="propertyName">给定的属性名。</param>
    /// <param name="propertyType">给定的属性类型。</param>
    /// <param name="value">给定的参考值。</param>
    /// <param name="callMethodName">给定要调用的方法名。</param>
    /// <returns>返回 Lambda 表达式。</returns>
    public static Expression<Func<T, bool>> CreatePropertyMethodExpression<T>(this string propertyName,
        Type propertyType, object? value, string callMethodName)
    {
        var type = typeof(T);

        var p = Expression.Parameter(type, "p");
        var property = Expression.Property(p, propertyName);
        var constant = Expression.Constant(value, propertyType);

        var propertyInfo = type.GetProperty(propertyName, TypeExtensions.AllMemberFlagsWithStatic);
        if (propertyInfo is null)
            throw new ArgumentException($"The property '{type}' with the specified name '{propertyName}' was not found.");

        var method = propertyInfo.PropertyType.GetMethod(callMethodName, TypeExtensions.AllMemberFlagsWithStatic, [propertyType]);
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
        => type.NewByExpression([parameter], [parameterType ?? parameter.GetType()]);

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
            parameterTypes ??= parameters.Select(static s => s.GetType()).ToArray();

            var constructor = type.GetConstructor(parameterTypes)
                ?? throw new ArgumentException($"No ConstructorInfo matching the specified parameters was found in the type '{type}'.");

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
