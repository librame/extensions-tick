#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Persistence;

/// <summary>
/// 定义抽象实现 <see cref="IPersistenceProvider{TPersistence}"/> 的持久化提供程序。
/// </summary>
/// <typeparam name="TPersistence">指定的持久化类型。</typeparam>
/// <remarks>
/// 构造一个 <see cref="AbstractPersistenceProvider{TPersistence}"/> 实例。
/// </remarks>
/// <param name="initialFunc">给定的实例初始方法。</param>
public abstract class AbstractPersistenceProvider<TPersistence>(Func<TPersistence> initialFunc) : IPersistenceProvider<TPersistence>
{
    /// <summary>
    /// 获取实例初始方法。
    /// </summary>
    protected Func<TPersistence> InitialFunc { get; init; } = initialFunc;

    /// <summary>
    /// 当前实例。
    /// </summary>
    public TPersistence Current { get; protected set; } = initialFunc();


    /// <summary>
    /// 存在实例。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public abstract bool Exist();

    /// <summary>
    /// 加载实例。
    /// </summary>
    /// <returns>返回 <typeparamref name="TPersistence"/>。</returns>
    public abstract TPersistence Load();

    /// <summary>
    /// 保存实例。
    /// </summary>
    /// <param name="persistence">给定的 <typeparamref name="TPersistence"/>。</param>
    /// <returns>返回 <typeparamref name="TPersistence"/>。</returns>
    public abstract TPersistence Save(TPersistence persistence);
}
