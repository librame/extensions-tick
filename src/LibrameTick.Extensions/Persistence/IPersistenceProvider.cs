#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Persistence;

/// <summary>
/// 定义持久化提供程序接口。
/// </summary>
/// <typeparam name="TPersistence">指定的持久化类型。</typeparam>
public interface IPersistenceProvider<TPersistence>
{
    /// <summary>
    /// 当前实例。
    /// </summary>
    TPersistence Current { get; }


    /// <summary>
    /// 存在实例。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    bool Exist();

    /// <summary>
    /// 加载实例。
    /// </summary>
    /// <returns>返回 <typeparamref name="TPersistence"/>。</returns>
    TPersistence Load();

    /// <summary>
    /// 保存实例。
    /// </summary>
    /// <param name="persistence">给定的 <typeparamref name="TPersistence"/>。</param>
    /// <returns>返回 <typeparamref name="TPersistence"/>。</returns>
    TPersistence Save(TPersistence persistence);
}
