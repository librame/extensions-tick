#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义 <see cref="IServiceInitializer"/> 加载器。
    /// </summary>
    public static class ServiceInitializerLoader
    {
        private static readonly Type _initializerType
            = typeof(IServiceInitializer);


        /// <summary>
        /// 加载实现 <see cref="IServiceInitializer"/> 的类型列表集合。
        /// </summary>
        /// <param name="options">给定的 <see cref="AssemblyLoadingOptions"/>（可选）。</param>
        /// <returns>返回 <see cref="List{Type}"/>。</returns>
        public static List<Type>? LoadInitializerTypes(AssemblyLoadingOptions? options = null)
        {
            var assemblies = AssemblyLoader.LoadAssemblies(options);
            if (assemblies == null)
                return null;

            return assemblies.ExportedTypes(FilterInitializerType);

            // 过滤初始化器可实例化类型
            static bool FilterInitializerType(Type type)
                => type.IsAssignableToBaseType(_initializerType) && !type.IsAbstract && !type.IsInterface;
        }

    }
}
