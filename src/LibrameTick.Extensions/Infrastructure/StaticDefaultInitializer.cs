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
/// <typeparam name="TInit">指定的初始化类型。</typeparam>
public class StaticDefaultInitializer<TInit>
    where TInit : class, new()
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


    private static TInit? s_instance;

    /// <summary>
    /// 获取默认实例。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TInit"/> 实例。
    /// </value>
    public static TInit Default
    {
        get
        {
            if (s_instance is not TInit instance)
            {
                instance = GetOrCreateDefaultInstance();
            }

            return instance;
        }
    }

    private static TInit GetOrCreateDefaultInstance()
    {
        var instance = new TInit();

        return Interlocked.CompareExchange(ref s_instance, instance, null) ?? instance;
    }

}
