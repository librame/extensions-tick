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
/// 定义单例工厂。
/// </summary>
/// <typeparam name="TSingleton">指定的单例类型。</typeparam>
public sealed class SingletonFactory<TSingleton>
    where TSingleton : class
{
    /// <summary>
    /// 创建弱引用对象（表示在引用对象的同时仍然允许通过垃圾回收来回收该对象）。
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
            if (!(_reference?.Target is TSingleton instance))
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

        try
        {
            var value = type.InvokeMember(type.Name,
                BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic,
                binder: null,
                target: null,
                args: null,
                CultureInfo.InvariantCulture);

            if (value is not null)
                return (TSingleton)value;

            return ExpressionExtensions.New<TSingleton>();
        }
        catch (MissingMethodException ex)
        {
            throw new TypeLoadException(
                string.Format(CultureInfo.InvariantCulture,
                    "The type '{0}' must have a private constructor to be used in the singleton pattern.",
                    type.FullName), ex);
        }
    }

}
