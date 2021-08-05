#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义程序集加载选项。
    /// </summary>
    public class AssemblyLoadingOptions
    {
        /// <summary>
        /// 程序集筛选方式（默认无筛选）。
        /// </summary>
        public AssemblyFiltration Filtration { get; set; }
            = AssemblyFiltration.None;

        /// <summary>
        /// 程序集过滤字符串列表。
        /// </summary>
        public List<string> Filters { get; init; }
            = new List<string>();

        /// <summary>
        /// 要加载的第三方程序集列表。
        /// </summary>
        [JsonIgnore]
        public List<Assembly> Others { get; init; }
            = new List<Assembly>();
    }
}
