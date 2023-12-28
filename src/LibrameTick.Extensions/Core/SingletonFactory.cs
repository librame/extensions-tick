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
/// 定义一个单例工厂（仅支持包含私有无参构造函数的单例类型）。
/// </summary>
/// <typeparam name="TSingleton">指定的单例类型（请确保此类型存在私有无参构造函数）。</typeparam>
public sealed class SingletonFactory<TSingleton>
    where TSingleton : class
{
    /// <summary>
    /// 创建（每个线程唯一）弱引用对象（表示在引用对象的同时仍然允许通过垃圾回收来回收该对象）。
    /// </summary>
    [ThreadStatic]
    private static WeakReference? _reference;


    /// <summary>
    /// 防止编译器生成默认构造函数。
    /// </summary>
    private SingletonFactory()
    {
    }

    /// <summary>
    /// 显式静态构造函数，告诉 C# 编译器不要将类型标记为 BeforeFieldInit。
    /// </summary>
    static SingletonFactory()
    {
    }


    /// <summary>
    /// 当前实例。
    /// </summary>
    public static TSingleton Instance
    {
        get
        {
            if (_reference?.Target is not TSingleton instance)
            {
                instance = CreateInstance();
                _reference = new WeakReference(instance);
            }

            return instance;
        }
    }


    [MethodImpl(MethodImplOptions.NoInlining)]
    private static TSingleton CreateInstance()
    {
        var type = typeof(TSingleton);

        if (!type.TryGetPrivateConstructor(isUnique: false, out var info))
            throw new MissingMethodException($"The type '{type.FullName}' must have a private constructor to be used in the singleton pattern.");

        return (TSingleton)info.Invoke(parameters: null);
    }

}
