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
/// 定义一个继承 <see cref="ICloneable"/> 的泛型可克隆接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public interface ICloneable<TSource> : ICloneable
{
    /// <summary>
    /// 创建一个泛型克隆对象（默认支持包含静态在内的所有字段和属性成员集合）。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSource"/>。</returns>
    TSource CloneAs();
}


/// <summary>
/// 定义实现 <see cref="ICloneable{TClone}"/> 的可克隆类。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class Cloneable<TSource> : ICloneable<TSource>
{
    private readonly TSource _source;


    /// <summary>
    /// 构造一个 <see cref="Cloneable{TClone}"/>。
    /// </summary>
    /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
    public Cloneable(TSource source)
    {
        _source = source;
        
        SourceMembers = new ConcurrentDictionary<string, object?>();
    }


    /// <summary>
    /// 已克隆的成员字典。
    /// </summary>
    protected ConcurrentDictionary<string, object?> SourceMembers { get; init; }


    /// <summary>
    /// 创建一个通用克隆对象（默认支持包含静态在内的所有字段和属性成员集合）。
    /// </summary>
    /// <returns>返回克隆对象。</returns>
    public virtual object Clone()
    {
        var clone = CloneCore(typeof(TSource), _source, SourceMembers);

        if (SourceMembers.Count > 0)
            SourceMembers.Clear();

        if (clone is null)
            throw new NotSupportedException($"The source type '{typeof(TSource)}' that do not support deep clone.");

        return clone;
    }

    /// <summary>
    /// 创建一个泛型克隆对象（默认支持包含静态在内的所有字段和属性成员集合）。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSource"/>。</returns>
    public virtual TSource CloneAs()
        => (TSource)Clone();


    private static object? CloneCore(Type objType, object? obj,
        ConcurrentDictionary<string, object?> objMembers)
    {
        // 如果是值或字符串类型
        if (objType.IsValueType || objType.IsStringType())
            return obj;

        // 如果支持序列化模式（二进制序列化因安全性问题被弃用）
        //if (cloneType.IsSerializable)
        //{
        //}

        // 创建默认实例副本
        var copy = objType.NewByExpression();

        foreach (var field in objType.GetAllFieldsWithStatic())
        {
            var fieldValue = objMembers.GetOrAdd(objType.Name + field.Name, key =>
            {
                // 链式克隆
                return CloneCore(field.FieldType, field.GetValue(obj), objMembers);
            });

            field.SetValue(copy, fieldValue);
        }

        return copy;
    }

}
