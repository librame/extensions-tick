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
using System;

namespace Librame.Extensions.Core
{
    using Data.Accessors;

    /// <summary>
    /// <see cref="ServiceInitializerActivator"/> 静态扩展。
    /// </summary>
    public static class ServiceInitializerActivatorExtensions
    {
        private static readonly Type _basePopulatorDefinition
            = typeof(AbstractAccessorInitializer<>);


        private static IServiceInitializer? CreateAccessorInitializer(IServiceProvider services,
            Type initializerType)
        {
            if (initializerType.IsImplementedType(_basePopulatorDefinition, out var resultType))
            {
                var accessorType = resultType!.GenericTypeArguments[0];
                var accessor = services.GetRequiredService(accessorType);

                return (IServiceInitializer)initializerType.NewByExpression(accessor, accessorType);
            }

            return null;
        }


        /// <summary>
        /// 激活 <see cref="IAccessorInitializer"/>。
        /// </summary>
        /// <param name="activator">给定的 <see cref="ServiceInitializerActivator"/>。</param>
        /// <returns>返回 <see cref="ServiceInitializerActivator"/>。</returns>
        public static ServiceInitializerActivator? ActivateAccessor(this ServiceInitializerActivator? activator)
            => activator?.Activate(CreateAccessorInitializer);

    }
}
