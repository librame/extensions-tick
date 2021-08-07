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
    /// 定义实现 <see cref="INotifyProperty"/> 的默认通知属性。
    /// </summary>
    public class DefaultNotifyProperty : AbstractNotifyProperty
    {
        /// <summary>
        /// 当前实例。
        /// </summary>
        public static readonly INotifyProperty Current = new DefaultNotifyProperty();


        private DefaultNotifyProperty()
            : base()
        {
        }

    }
}
