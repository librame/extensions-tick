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
/// 定义线程安全的静态默认初始化器。
/// </summary>
/// <typeparam name="TDefault">指定的初始化类型。</typeparam>
public class StaticDefaultInitializer<TDefault>
    where TDefault : class, new()
{
    /// <summary>
    /// 构造一个 <see cref="StaticDefaultInitializer{T}"/> 默认实例。
    /// </summary>
    protected StaticDefaultInitializer()
    {
    }

    static StaticDefaultInitializer()
    {
    }


    private static TDefault? s_instance;

    /// <summary>
    /// 获取默认实例。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TDefault"/> 实例。
    /// </value>
    public static TDefault Default
    {
        get
        {
            if (s_instance is not TDefault instance)
            {
                instance = GetOrCreateDefaultInstance();
            }
            
            return instance;
        }
    }

    private static TDefault GetOrCreateDefaultInstance()
    {
        var instance = new TDefault();

        return Interlocked.CompareExchange(ref s_instance, instance, null) ?? instance;
    }

}
