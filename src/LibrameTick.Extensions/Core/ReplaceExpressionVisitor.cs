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
/// 定义一个继承 <see cref="ExpressionVisitor"/> 的替换表达式访问器。
/// </summary>
public class ReplaceExpressionVisitor : ExpressionVisitor
{
    private readonly Expression _oldValue;
    private readonly Expression _newValue;


    /// <summary>
    /// 构造一个 <see cref="ReplaceExpressionVisitor"/>。
    /// </summary>
    /// <param name="oldValue">给定的旧表达式。</param>
    /// <param name="newValue">给定的新表达式。</param>
    public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
    {
        _oldValue = oldValue;
        _newValue = newValue;
    }


    /// <summary>
    /// 分配表达式。
    /// </summary>
    /// <param name="node">给定的 <see cref="Expression"/>。</param>
    /// <returns>返回 <see cref="Expression"/>。</returns>
    public override Expression? Visit(Expression? node)
    {
        if (node == _oldValue)
            return _newValue;

        return base.Visit(node);
    }

}
