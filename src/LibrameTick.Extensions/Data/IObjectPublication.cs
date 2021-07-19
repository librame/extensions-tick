#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 对象发表接口。
    /// </summary>
    public interface IObjectPublication : IObjectPublisher, IObjectPublicationTime, IObjectCreation
    {
    }
}
