#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding
{
    /// <summary>
    /// 定义基于分片依据模运算的分片策略。
    /// </summary>
    /// <remarks>
    /// 分片依据模运算的分片策略支持的参数（区分大小写）包括：%m（后面跟要参与模运算的数字，如：%m3，即表示分片依据的整数标识除以 3 的余数）。
    /// </remarks>
    public class ModShardingStrategy : AbstractShardingStrategy
    {
        private readonly string _modKey = BuildParameterKey("m");


        /// <summary>
        /// 格式化后缀核心。
        /// </summary>
        /// <param name="suffix">给定的后缀。</param>
        /// <param name="basis">给定的分片依据。</param>
        /// <returns>返回字符串。</returns>
        protected override string FormatSuffixCore(string suffix, object? basis)
        {
            if (!suffix.Contains(_modKey) || !(basis is IObjectIdentifier identifier))
                return suffix;

            var objId = identifier.GetObjectId();
            if (!(objId is int id))
                return suffix;

            var modString = string.Empty;

            var index = suffix.IndexOf(_modKey);
            var temp = suffix.Substring(index);

            // 将模参数键后面的数字当作取模
            for (var i = 0; i < temp.Length; i++)
            {
                var ch = temp[i];
                if (ch.IsDigit())
                    modString += ch;
            }

            var mod = int.Parse(modString);
            return (id % mod).ToString();
        }

    }
}
