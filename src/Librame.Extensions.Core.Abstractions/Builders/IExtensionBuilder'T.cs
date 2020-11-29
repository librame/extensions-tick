#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Builders
{
    using Options;

    /// <summary>
    /// 扩展构建器接口。
    /// </summary>
    /// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
    public interface IExtensionBuilder<TOptions> : IExtensionBuilder
        where TOptions : IExtensionOptions
    {
        /// <summary>
        /// 扩展选项。
        /// </summary>
        new TOptions Options { get; }
    }
}
