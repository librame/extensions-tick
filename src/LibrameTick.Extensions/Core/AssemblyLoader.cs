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
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义程序集加载器。
    /// </summary>
    public static class AssemblyLoader
    {

        /// <summary>
        /// 加载程序集集合。
        /// </summary>
        /// <param name="options">给定的 <see cref="AssemblyLoadingOptions"/>（可选）。</param>
        /// <returns>返回 <see cref="Assembly"/> 数组。</returns>
        public static Assembly[]? LoadAssemblies(AssemblyLoadingOptions? options = null)
        {
            if (options == null)
                options = new AssemblyLoadingOptions();

            var assemblyNames = Assembly.GetEntryAssembly()?.GetReferencedAssemblies();
            if (assemblyNames == null)
                return null;

            // 如果启用筛选
            if (options.Filtration != AssemblyFiltration.None && options.Filters.Count > 0)
            {
                if (options.Filtration == AssemblyFiltration.Exclusive)
                {
                    // 排除过滤程序集字符串
                    assemblyNames = assemblyNames.Where(p =>
                        options.Filters.Any(filter => !p.Name!.Contains(filter))).ToArray();
                }
                else
                {
                    // 包含过滤程序集字符串
                    assemblyNames = assemblyNames.Where(p =>
                        options.Filters.Any(filter => p.Name!.Contains(filter))).ToArray();
                }
            }

            var assemblies = assemblyNames.Select(Assembly.Load);
            if (options.Others.Count > 0)
                assemblies = assemblies.Concat(options.Others).DistinctBy(s => s.FullName);

            return assemblies.ToArray();
        }

        /// <summary>
        /// 通过程序集集合加载指定基础类型的可实例化类型集合。
        /// </summary>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="options">给定的 <see cref="AssemblyLoadingOptions"/>（可选）。</param>
        /// <returns>返回 <see cref="Type"/> 数组。</returns>
        public static Type[]? LoadInstantiableTypesByAssemblies(Type baseType, AssemblyLoadingOptions? options = null)
        {
            var assemblies = LoadAssemblies(options);
            if (assemblies == null)
                return null;

            return assemblies.SelectMany(s => s.ExportedTypes)
                .Where(type => FilterInstantiableType(type, baseType)).ToArray();
        }

        private static bool FilterInstantiableType(Type currentType, Type baseType)
            => currentType.IsAssignableToBaseType(baseType) && currentType.IsConcreteType();

    }
}
