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
    /// 定义用于分片命名的特性（可用于分库、分表操作）。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ShardingNamingAttribute : Attribute
    {
        /// <summary>
        /// 构造一个 <see cref="ShardingNamingAttribute"/>。
        /// </summary>
        /// <param name="strategyType">给定的策略类型。</param>
        /// <param name="suffix">给定的后缀（支持的参数可参考指定的分片策略类型）。</param>
        public ShardingNamingAttribute(Type strategyType, string suffix)
        {
            StrategyType = strategyType;
            Suffix = suffix;
        }


        /// <summary>
        /// 策略类型。
        /// </summary>
        public Type StrategyType { get; set; }

        /// <summary>
        /// 基础名称（可空；默认根据不同的上下文环境使用对应的名称，比如分表将使用标记特性的实体类型、分库使用从连接字符串解析的数据库名等）。
        /// </summary>
        public string? BaseName { get; set; }

        /// <summary>
        /// 后缀。
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 后缀连接符（可空；默认使用 <see cref="ShardingNamingDescriptor.DefaultSuffixConnector"/>）。
        /// </summary>
        public string? SuffixConnector { get; set; }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{nameof(BaseName)}={BaseName};{nameof(StrategyType)}={StrategyType.Name};{nameof(SuffixConnector)}={SuffixConnector};{nameof(Suffix)}={Suffix}";

    }
}
