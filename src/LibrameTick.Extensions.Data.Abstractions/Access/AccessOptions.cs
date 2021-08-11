#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using System;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Data.Access
{
    /// <summary>
    /// 访问选项。
    /// </summary>
    public class AccessOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AccessOptions"/>。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="IPropertyNotifier"/>。</param>
        public AccessOptions(IPropertyNotifier notifyProperty)
        {
            NotifyProperty = notifyProperty;
        }


        /// <summary>
        /// 通知属性。
        /// </summary>
        protected IPropertyNotifier NotifyProperty { get; init; }


        /// <summary>
        /// 默认切片方法（当相同所属群组存在多访问器且交互方式一致时，此选项有效）。
        /// </summary>
        [JsonIgnore]
        public Func<IAccessor, bool>? DefaultSliceFunc { get; set; }

        /// <summary>
        /// 当连接数据库时，如果数据库已存在，则可以确保将可能已存在的数据库删除（默认未启用功能。注：务必慎用此功能，推荐用于测试环境，不可用于正式环境）。
        /// </summary>
        public bool EnsureDatabaseDeleted { get; set; }

        /// <summary>
        /// 当连接数据库时，如果数据库不存在，则可以确保新建数据库（默认启用此功能）。
        /// </summary>
        public bool EnsureDatabaseCreated { get; set; }
            = true;

        /// <summary>
        /// 自动迁移数据库（默认自动迁移）。
        /// </summary>
        public bool AutomaticMigration { get; set; }
            = true;
    }
}
