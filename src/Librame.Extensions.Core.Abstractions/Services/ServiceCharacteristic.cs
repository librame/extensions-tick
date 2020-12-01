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

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 服务特征。
    /// </summary>
    public record ServiceCharacteristic
    {
        /// <summary>
        /// 构造一个 <see cref="ServiceCharacteristic"/>。
        /// </summary>
        /// <param name="lifetime">给定的 <see cref="ServiceLifetime"/>。</param>
        public ServiceCharacteristic(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }


        /// <summary>
        /// 生命周期。
        /// </summary>
        public ServiceLifetime Lifetime { get; init; }


        /// <summary>
        /// 单例服务。
        /// </summary>
        /// <returns>返回 <see cref="ServiceCharacteristic"/>。</returns>
        public static ServiceCharacteristic Singleton()
            => new ServiceCharacteristic(ServiceLifetime.Singleton);
    }
}
