#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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

            return assemblyNames.Select(Assembly.Load).ToArray();
        }

    }
}
