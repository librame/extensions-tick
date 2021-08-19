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
    /// 定义实例化器。
    /// </summary>
    public static class Instantiator
    {
        private static readonly IInstanceContainer _container;


        static Instantiator()
        {
            if (_container == null)
                _container = new InternalInstanceContainer();
        }


        /// <summary>
        /// 实例容器。
        /// </summary>
        public static IInstanceContainer Container
            => _container;


        /// <summary>
        /// 获取时钟（默认使用内置本地时钟）。
        /// </summary>
        /// <returns>返回 <see cref="IClock"/>。</returns>
        public static IClock GetClock()
            => _container.Resolve(new TypeNamedKey<IClock>(), key => new InternalLocalClock());

        /// <summary>
        /// 获取属性通知器。
        /// </summary>
        /// <param name="source">给定的属性源。</param>
        /// <param name="sourceAliase">给定的源别名（可选）。</param>
        /// <returns>返回 <see cref="IPropertyNotifier"/>。</returns>
        public static IPropertyNotifier GetPropertyNotifier(object source, string? sourceAliase = null)
            => _container.Resolve(new TypeNamedKey<IPropertyNotifier>(source.GetType().Name),
                key => new PropertyNotifier(source, sourceAliase));

    }
}
