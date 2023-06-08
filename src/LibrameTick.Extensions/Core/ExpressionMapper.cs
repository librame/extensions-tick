﻿#region License

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
/// 定义一个可映射对象所有公共属性的表达式映射器。
/// </summary>
/// <typeparam name="TInput">指定的输入类型。</typeparam>
/// <typeparam name="TOutput">指定的输出类型。</typeparam>
public static class ExpressionMapper<TInput, TOutput>
{
    private static readonly Func<TInput, TOutput> _cache = InitialFunc();


    private static Func<TInput, TOutput> InitialFunc()
    {
        var inputType = typeof(TInput);
        var outputType = typeof(TOutput);

        var memberAssignments = new List<MemberAssignment>();

        var parameterExpression = Expression.Parameter(inputType, "p");

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

        var lambda = Expression.Lambda<Func<TInput, TOutput>>(memberInitExpression,
            new ParameterExpression[] { parameterExpression });

        return lambda.Compile();
    }


    /// <summary>
    /// 映射输入对象。
    /// </summary>
    /// <param name="input">给定的 <typeparamref name="TInput"/>。</param>
    /// <returns>返回 <typeparamref name="TOutput"/>。</returns>
    public static TOutput Map(TInput input)
        => _cache(input);

}
