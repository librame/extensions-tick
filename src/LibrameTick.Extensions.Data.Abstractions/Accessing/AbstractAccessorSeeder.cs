#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Concurrent;

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// 抽象实现 <see cref="IAccessorSeeder"/>。
    /// </summary>
    public abstract class AbstractAccessorSeeder : IAccessorSeeder
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAccessorSeeder"/>。
        /// </summary>
        /// <param name="idGeneratorFactory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        protected AbstractAccessorSeeder(IIdentificationGeneratorFactory idGeneratorFactory)
        {
            SeedBank = new ConcurrentDictionary<string, object>();
            IdGeneratorFactory = idGeneratorFactory;
        }


        /// <summary>
        /// 种子银行。
        /// </summary>
        protected ConcurrentDictionary<string, object> SeedBank { get; init; }

        /// <summary>
        /// 标识生成器工厂。
        /// </summary>
        public IIdentificationGeneratorFactory IdGeneratorFactory { get; init; }
    }
}
