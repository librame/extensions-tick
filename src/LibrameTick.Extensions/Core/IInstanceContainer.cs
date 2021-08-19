#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义实例容器接口。
    /// </summary>
    public interface IInstanceContainer : IEnumerable<KeyValuePair<TypeNamedKey, object>>
    {
        /// <summary>
        /// 实例数。
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 实例键集合。
        /// </summary>
        ICollection<TypeNamedKey> Keys { get; }


        /// <summary>
        /// 包含指定键。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <returns>返回布尔值。</returns>
        bool ContainsKey(TypeNamedKey key);


        /// <summary>
        /// 注册指定键实例对象方法。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="instanceFunc">给定的实例对象方法。</param>
        /// <returns>返回实例对象。</returns>
        TInstance Register<TInstance>(TypeNamedKey<TInstance> key, Func<TypeNamedKey, TInstance> instanceFunc);

        /// <summary>
        /// 注册指定键实例对象方法。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="instanceFunc">给定的实例对象方法。</param>
        /// <returns>返回实例对象。</returns>
        object Register(TypeNamedKey key, Func<TypeNamedKey, object> instanceFunc);


        /// <summary>
        /// 注册指定键实例对象。
        /// </summary>
        /// <typeparam name="TInstance">指定的实例类型。</typeparam>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="instance">给定的 <typeparamref name="TInstance"/>。</param>
        /// <returns>返回实例。</returns>
        TInstance Register<TInstance>(TypeNamedKey<TInstance> key, TInstance instance);

        /// <summary>
        /// 注册指定键实例对象。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="instance">给定的实例对象。</param>
        /// <returns>返回实例对象。</returns>
        object Register(TypeNamedKey key, object instance);


        /// <summary>
        /// 解析指定键实例。
        /// </summary>
        /// <typeparam name="TInstance">指定的实例类型。</typeparam>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="instanceFunc">给定的 <typeparamref name="TInstance"/> 方法。</param>
        /// <returns>返回实例。</returns>
        TInstance Resolve<TInstance>(TypeNamedKey<TInstance> key, Func<TypeNamedKey, TInstance> instanceFunc);

        /// <summary>
        /// 解析指定键实例对象。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="instanceFunc">给定的实例对象方法。</param>
        /// <returns>返回实例对象。</returns>
        object Resolve(TypeNamedKey key, Func<TypeNamedKey, object> instanceFunc);
    }
}
