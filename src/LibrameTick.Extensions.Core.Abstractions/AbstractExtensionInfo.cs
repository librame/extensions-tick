#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义抽象实现 <see cref="IExtensionInfo"/>。
    /// </summary>
    public abstract class AbstractExtensionInfo : IExtensionInfo
    {
        /// <summary>
        /// 抽象一个 <see cref="AbstractExtensionInfo"/>。
        /// </summary>
        /// <param name="parentInfo">给定的父级 <see cref="IExtensionInfo"/>（可空；为空则表示当前为父级扩展）。</param>
        protected AbstractExtensionInfo(IExtensionInfo? parentInfo)
        {
            ParentInfo = parentInfo;
        }


        /// <summary>
        /// 信息类型。
        /// </summary>
        [JsonIgnore]
        public virtual Type InfoType
            => GetType();

        /// <summary>
        /// 名称。
        /// </summary>
        [JsonIgnore]
        public virtual string Name
            => InfoType.Name;

        /// <summary>
        /// 父级信息。
        /// </summary>
        [JsonIgnore]
        public virtual IExtensionInfo? ParentInfo { get; init; }
    }
}
