#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Cryptography;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 用户模型。
    /// </summary>
    [Audited]
    [Sharded(typeof(CultureInfoShardingStrategy), "%c")]
    public class User : AbstractCreationIdentifier<long, long>
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
