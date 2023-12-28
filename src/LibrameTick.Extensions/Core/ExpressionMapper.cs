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
/// 定义一个可映射类型成员的表达式映射器。
/// </summary>
/// <typeparam name="TInput">指定的输入类型。</typeparam>
/// <typeparam name="TOutput">指定的输出类型。</typeparam>
public static class ExpressionMapper<TInput, TOutput>
{
    private static readonly Func<TInput, TOutput> _cacheMapAllPublicPropertiesFunc = InitialMapAllPublicPropertiesFunc();
    private static readonly Func<TInput, TOutput> _cacheMapAllFieldsFunc = InitialMapAllFieldsFunc();


    /// <summary>
    /// 通过映射 <typeparamref name="TInput"/> 的所有公共属性创建 <typeparamref name="TOutput"/> 的新实例。
    /// </summary>
    /// <param name="input">给定的 <typeparamref name="TInput"/>。</param>
    /// <returns>返回 <typeparamref name="TOutput"/>。</returns>
    public static TOutput NewByMapAllPublicProperties(TInput input)
        => _cacheMapAllPublicPropertiesFunc(input);

    /// <summary>
    /// 通过映射 <typeparamref name="TInput"/> 的所有字段（私有字段包含属性实现）创建 <typeparamref name="TOutput"/> 的新实例。
    /// </summary>
    /// <param name="input">给定的 <typeparamref name="TInput"/>。</param>
    /// <returns>返回 <typeparamref name="TOutput"/>。</returns>
    public static TOutput NewByMapAllFields(TInput input)
        => _cacheMapAllFieldsFunc(input);


    private static Func<TInput, TOutput> InitialMapAllPublicPropertiesFunc()
    {
        var inputType = typeof(TInput);
        var outputType = typeof(TOutput);

        var memberAssignments = new List<MemberAssignment>();

        var parameterExpression = Expression.Parameter(inputType, "p");

        // 映射所有公共属性
        foreach (var outProp in outputType.GetProperties())
        {
            if (!outProp.CanWrite || outProp.PropertyType.IsSameOrNullableType(outputType))
                continue; // 排除自引用类型

            var inProp = inputType.GetProperty(outProp.Name);
            if (inProp is not null && outProp.PropertyType.IsSameOrNullableType(inProp.PropertyType))
            {
                var propertyExpression = Expression.Property(parameterExpression, inProp);
                var memberBinding = Expression.Bind(outProp, propertyExpression);

                memberAssignments.Add(memberBinding);
            }
        }

        var memberInitExpression = Expression.MemberInit(Expression.New(outputType),
            memberAssignments.ToArray());

        var lambda = Expression.Lambda<Func<TInput, TOutput>>(memberInitExpression, [parameterExpression]);

        return lambda.Compile();
    }

    private static Func<TInput, TOutput> InitialMapAllFieldsFunc()
    {
        var inputType = typeof(TInput);
        var outputType = typeof(TOutput);

        var memberAssignments = new List<MemberAssignment>();

        var parameterExpression = Expression.Parameter(inputType, "p");

        // 映射所有字段（私有字段包含属性实现）
        foreach (var outField in outputType.GetFields(TypeExtensions.AllMemberFlags))
        {
            if (outField.FieldType.IsSameOrNullableType(outputType))
                continue; // 排除自引用类型

            var inField = inputType.GetField(outField.Name, TypeExtensions.AllMemberFlags);
            if (inField is not null && outField.FieldType.IsSameOrNullableType(inField.FieldType))
            {
                var propertyExpression = Expression.Field(parameterExpression, inField);
                var memberBinding = Expression.Bind(outField, propertyExpression);

                memberAssignments.Add(memberBinding);
            }
        }

        var memberInitExpression = Expression.MemberInit(Expression.New(outputType),
            memberAssignments.ToArray());

        var lambda = Expression.Lambda<Func<TInput, TOutput>>(memberInitExpression, [parameterExpression]);
        return lambda.Compile();
    }

}
