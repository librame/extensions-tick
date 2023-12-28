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
/// 定义一个延迟单例（仅支持包含私有无参构造函数的单例类型）。
/// </summary>
public static class LazySingleton
{
    /// <summary>
    /// 获取指定类型的单例。
    /// </summary>
    /// <typeparam name="TSingleton">指定要创建或获取的单例类型（请确保此类型存在私有无参构造函数）。</typeparam>
    /// <returns>返回单例。</returns>
    public static TSingleton GetInstance<TSingleton>()
        where TSingleton : class
        => LazySingleton<TSingleton>.Instance;
}


/// <summary>
/// 定义一个延迟单例（仅支持包含私有无参构造函数的单例类型）。
/// </summary>
/// <typeparam name="TSingleton">指定要创建或获取的单例类型（请确保此类型存在私有无参构造函数）。</typeparam>
public static class LazySingleton<TSingleton>
    where TSingleton : class
{
    /// <summary>
    /// 延迟创建（每个线程唯一）实例。
    /// </summary>
    [ThreadStatic]
    private static readonly Lazy<TSingleton> _lazy;


    static LazySingleton()
    {
        _lazy ??= new(CreateInstance);
    }


    /// <summary>
    /// 得到单例。
    /// </summary>
    public static TSingleton Instance
        => _lazy.Value;


    private static TSingleton CreateInstance()
    {
        var type = typeof(TSingleton);

        if (!type.TryGetPrivateConstructor(isUnique: false, out var info))
            throw new MissingMethodException($"The type '{type.FullName}' must have a private constructor to be used in the singleton pattern.");

        return (TSingleton)info.Invoke(parameters: null);
    }

}
