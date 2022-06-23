#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

//using System.Collections;

//namespace Librame.Extensions.Collections;

///// <summary>
///// 定义一个实现 <see cref="IQueryable{T}"/> 的泛型复合可查询类型。
///// </summary>
///// <typeparam name="T">指定的类型。</typeparam>
//public class CompositeQueryable<T> : IQueryable<T>
//{
//    private readonly IEnumerable<IQueryable<T>> _queries;


//    /// <summary>
//    /// 构造一个 <see cref="CompositeQueryable{T}"/>。
//    /// </summary>
//    /// <param name="queries">给定的 <see cref="IQueryable{T}"/> 可枚举集合。</param>
//    public CompositeQueryable(IEnumerable<IQueryable<T>> queries)
//    {
//        _queries = queries;
//    }


//    /// <summary>
//    /// 元素类型。
//    /// </summary>
//    public Type ElementType
//        => _queries.First().ElementType;

//    /// <summary>
//    /// 查询表达式。
//    /// </summary>
//    public Expression Expression
//        => throw new NotImplementedException();

//    /// <summary>
//    /// 查询提供程序。
//    /// </summary>
//    public IQueryProvider Provider
//        => throw new NotImplementedException();



//    public IEnumerator<T> GetEnumerator()
//    {
//        foreach (var query in _queries)
//        {
//            foreach(var item in query)
//                yield return item;
//        }
//    }

//    IEnumerator IEnumerable.GetEnumerator()
//    {
//        throw new NotImplementedException();
//    }
//}
