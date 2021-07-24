#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 服务特征。
    /// </summary>
    public record ServiceCharacteristic
    {
        /// <summary>
        /// 构造一个 <see cref="ServiceCharacteristic"/>。
        /// </summary>
        /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>（可选；默认使用 <see cref="ServiceLifetime.Singleton"/>）。</param>
        /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
        public ServiceCharacteristic(ServiceLifetime lifetime = ServiceLifetime.Singleton,
            bool replaceIfExists = false)
        {
            Lifetime = lifetime;
            ReplaceIfExists = replaceIfExists;
        }


        /// <summary>
        /// 服务的生命周期。
        /// </summary>
        public ServiceLifetime Lifetime{ get; init; }

        /// <summary>
        /// 是否替换已存在的服务。
        /// </summary>
        public bool ReplaceIfExists{ get; init; }


        /// <summary>
        /// 单例服务。
        /// </summary>
        /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristic"/>。</returns>
        public static ServiceCharacteristic Singleton(bool replaceIfExists = false)
            => new ServiceCharacteristic(replaceIfExists: replaceIfExists);

        /// <summary>
        /// 域例服务。
        /// </summary>
        /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristic"/>。</returns>
        public static ServiceCharacteristic Scope(bool replaceIfExists = false)
            => new ServiceCharacteristic(ServiceLifetime.Scoped, replaceIfExists);

        /// <summary>
        /// 瞬例服务。
        /// </summary>
        /// <param name="replaceIfExists">是否替换已存在的服务（可选；默认不替换）。</param>
        /// <returns>返回 <see cref="ServiceCharacteristic"/>。</returns>
        public static ServiceCharacteristic Transient(bool replaceIfExists = false)
            => new ServiceCharacteristic(ServiceLifetime.Transient, replaceIfExists);
    }
}
