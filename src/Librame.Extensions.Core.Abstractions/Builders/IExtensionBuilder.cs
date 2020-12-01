#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core.Builders
{
    using Options;

    /// <summary>
    /// 扩展构建器接口。
    /// </summary>
    public interface IExtensionBuilder : IExtensionInfo<IExtensionBuilder>
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 扩展选项。
        /// </summary>
        IExtensionOptions Options { get; }
    }
}
