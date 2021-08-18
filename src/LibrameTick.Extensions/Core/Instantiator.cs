#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Concurrent;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义实例化器。
    /// </summary>
    public static class Instantiator
    {

        private static readonly ConcurrentDictionary<string, object> _instances = new();

    }
}
