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
    /// 定义分片命名描述符。
    /// </summary>
    public class ShardingNamingDescriptor : IEquatable<ShardingNamingDescriptor>
    {
        /// <summary>
        /// 默认后缀连接符。
        /// </summary>
        public const string DefaultSuffixConnector = "_";


        /// <summary>
        /// 构造一个 <see cref="ShardingNamingDescriptor"/>。
        /// </summary>
        /// <param name="baseName">给定的基础名称。</param>
        /// <param name="suffix">给定的后缀。</param>
        public ShardingNamingDescriptor(string? baseName, string suffix)
            : this(baseName, suffix, suffixConnector: null)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ShardingNamingDescriptor"/>。
        /// </summary>
        /// <param name="baseName">给定的基础名称。</param>
        /// <param name="suffix">给定的后缀。</param>
        /// <param name="suffixConnector">给定的后缀连接符（可空；默认使用 <see cref="DefaultSuffixConnector"/>）。</param>
        public ShardingNamingDescriptor(string? baseName, string suffix, string? suffixConnector)
        {
            BaseName = baseName;
            Suffix = suffix;
            SuffixConnector = suffixConnector ?? DefaultSuffixConnector;
        }


        /// <summary>
        /// 基础名称。
        /// </summary>
        public string? BaseName { get; private set; }

        /// <summary>
        /// 后缀。
        /// </summary>
        public string Suffix { get; init; }

        /// <summary>
        /// 后缀连接符。
        /// </summary>
        public string SuffixConnector { get; init; }


        /// <summary>
        /// 设置基础名称。
        /// </summary>
        /// <param name="baseName">给定的基础名称。</param>
        /// <returns>返回 <see cref="ShardingNamingDescriptor"/>。</returns>
        public ShardingNamingDescriptor SetBaseName(string baseName)
        {
            BaseName = baseName;
            return this;
        }


        /// <summary>
        /// 使用指定的新基础名称创建一个新 <see cref="ShardingNamingDescriptor"/>。
        /// </summary>
        /// <param name="newBaseName">给定的新基础名称。</param>
        /// <returns>返回 <see cref="ShardingNamingDescriptor"/>。</returns>
        public ShardingNamingDescriptor WithBaseName(string newBaseName)
            => new(newBaseName, Suffix, SuffixConnector);

        /// <summary>
        /// 使用指定的新后缀创建一个新 <see cref="ShardingNamingDescriptor"/>。
        /// </summary>
        /// <param name="newSuffix">给定的新后缀。</param>
        /// <returns>返回 <see cref="ShardingNamingDescriptor"/>。</returns>
        public ShardingNamingDescriptor WithSuffix(string newSuffix)
            => new(BaseName, newSuffix, SuffixConnector);

        /// <summary>
        /// 使用指定的新后缀连接符创建一个新 <see cref="ShardingNamingDescriptor"/>。
        /// </summary>
        /// <param name="newSuffixConnector">给定的新后缀连接符。</param>
        /// <returns>返回 <see cref="ShardingNamingDescriptor"/>。</returns>
        public ShardingNamingDescriptor WithSuffixConnector(string newSuffixConnector)
            => new(BaseName, Suffix, newSuffixConnector);


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="ShardingNamingDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(ShardingNamingDescriptor? other)
            => other != null && ToString() == other.ToString();


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();


        /// <summary>
        /// 转为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Suffix))
                return $"{BaseName}{SuffixConnector}{Suffix}";

            return BaseName ?? string.Empty;
        }


        /// <summary>
        /// 从字符串中解析 <see cref="ShardingNamingDescriptor"/>。
        /// </summary>
        /// <param name="shardingNaming">给定的分片命名字符串。</param>
        /// <returns>返回 <see cref="ShardingNamingDescriptor"/>。</returns>
        public static ShardingNamingDescriptor Parse(string shardingNaming)
            => Parse(shardingNaming, DefaultSuffixConnector);

        /// <summary>
        /// 从字符串中解析 <see cref="ShardingNamingDescriptor"/>。
        /// </summary>
        /// <param name="shardingNaming">给定的分片命名字符串。</param>
        /// <param name="suffixConnector">给定的后缀连接符。</param>
        /// <returns>返回 <see cref="ShardingNamingDescriptor"/>。</returns>
        public static ShardingNamingDescriptor Parse(string shardingNaming, string suffixConnector)
        {
            if (shardingNaming.TrySplitPair(suffixConnector, out var pair))
                return new ShardingNamingDescriptor(pair.Key, pair.Value, suffixConnector);

            return new ShardingNamingDescriptor(shardingNaming, string.Empty, suffixConnector);
        }


        /// <summary>
        /// 隐式转为字符串。
        /// </summary>
        /// <param name="shardingNaming">给定的 <see cref="ShardingNamingDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        public static implicit operator string(ShardingNamingDescriptor shardingNaming)
            => shardingNaming.ToString();

        /// <summary>
        /// 显式转为 <see cref="ShardingNamingDescriptor"/>（仅支持 <see cref="DefaultSuffixConnector"/>）。
        /// </summary>
        /// <param name="shardingNaming">给定的分片字符串。</param>
        /// <returns>返回 <see cref="ShardingNamingDescriptor"/>。</returns>
        public static implicit operator ShardingNamingDescriptor(string shardingNaming)
            => Parse(shardingNaming);

    }
}
