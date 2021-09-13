#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Storing
{
    /// <summary>
    /// 定义实现 <see cref="IIdentifier{Int32}"/> 的实体表。
    /// </summary>
    public class Tabulation : AbstractIdentifier<int>
    {
        /// <summary>
        /// 架构。
        /// </summary>
        public virtual string Schema { get; set; }
            = string.Empty;

        /// <summary>
        /// 表名。
        /// </summary>
        public virtual string TableName { get; set; }
            = string.Empty;

        /// <summary>
        /// 是否分表。
        /// </summary>
        public virtual bool IsSharding { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public virtual string Description { get; set; }
            = string.Empty;

        /// <summary>
        /// 实体名。
        /// </summary>
        public virtual string EntityName { get; set; }
            = string.Empty;

        /// <summary>
        /// 程序集名。
        /// </summary>
        public virtual string AssemblyName { get; set; }
            = string.Empty;
    }
}
