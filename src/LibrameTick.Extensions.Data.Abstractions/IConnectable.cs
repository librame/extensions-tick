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
    /// 定义可连接的接口。
    /// </summary>
    public interface IConnectable
    {
        /// <summary>
        /// 当前连接字符串。
        /// </summary>
        string? CurrentConnectionString { get; }

        /// <summary>
        /// 改变时动作。
        /// </summary>
        Action<IConnectable>? ChangingAction { get; set; }

        /// <summary>
        /// 改变后动作。
        /// </summary>
        Action<IConnectable>? ChangedAction { get; set; }


        /// <summary>
        /// 改变数据库连接。
        /// </summary>
        /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
        /// <returns>返回 <see cref="IConnectable"/>。</returns>
        IConnectable ChangeConnection(string newConnectionString);
    }
}
