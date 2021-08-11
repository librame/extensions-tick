#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 选项接口。
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// 属性通知器。
        /// </summary>
        IPropertyNotifier Notifier { get; }
    }
}
