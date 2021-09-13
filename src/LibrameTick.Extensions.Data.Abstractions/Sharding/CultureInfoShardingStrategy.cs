#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Globalization;

namespace Librame.Extensions.Data.Sharding
{
    /// <summary>
    /// 定义基于当前文化信息的分片策略。
    /// </summary>
    /// <remarks>
    /// 当前文化信息的分片策略支持的参数（区分大小写）包括：%c（当前文化信息名称）、%uic（当前 UI 文化信息名称）。
    /// </remarks>
    public class CultureInfoShardingStrategy : AbstractShardingStrategy
    {
        private readonly Dictionary<string, string> _parameters = new()
        {
            { BuildParameterKey("c"), CultureInfo.CurrentCulture.Name },
            { BuildParameterKey("uic"), CultureInfo.CurrentUICulture.Name }
        };


        /// <summary>
        /// 格式化后缀核心。
        /// </summary>
        /// <param name="suffix">给定的后缀。</param>
        /// <param name="basis">给定的分片依据。</param>
        /// <returns>返回字符串。</returns>
        protected override string FormatSuffixCore(string suffix, object? basis)
        {
            foreach (var p in _parameters)
            {
                // 区分大小写
                suffix = suffix.Replace(p.Key, p.Value, StringComparison.Ordinal);
            }

            return suffix;
        }

    }
}
