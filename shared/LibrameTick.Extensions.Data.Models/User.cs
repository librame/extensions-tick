#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Cryptography;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 用户模型。
    /// </summary>
    [Sharded<CultureInfoShardingStrategy>("%c")]
    public class User : AbstractCreationIdentifier<string, string>
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [Encrypted]
        public virtual string? Passwd { get; set; }
    }
}
