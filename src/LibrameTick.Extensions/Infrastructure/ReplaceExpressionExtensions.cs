#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义 <see cref="ReplaceExpressionVisitor"/> 静态扩展。
/// </summary>
public static class ReplaceExpressionExtensions
{

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

        var leftVisitor = new Infrastructure.ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);
        ArgumentNullException.ThrowIfNull(left);

        var rightVisitor = new Infrastructure.ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);
        ArgumentNullException.ThrowIfNull(right);

        return Expression.Lambda<Func<T, TProperty>>(
            func(left, right), parameter);
    }

}
