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
    /// <typeparam name="TConnection">指定的连接类型。</typeparam>
    public interface IConnectable<TConnection>
    {
        /// <summary>
        /// 当前连接字符串。
        /// </summary>
        string? CurrentConnectionString { get; }

        /// <summary>
        /// 改变时动作。
        /// </summary>
        Action<TConnection>? ChangingAction { get; set; }

        /// <summary>
        /// 改变后动作。
        /// </summary>
        Action<TConnection>? ChangedAction { get; set; }


        /// <summary>
        /// 改变数据库连接。
        /// </summary>
        /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
        /// <returns>返回 <typeparamref name="TConnection"/>。</returns>
        TConnection ChangeConnection(string newConnectionString);


        /// <summary>
        /// 尝试创建数据库。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        bool TryCreateDatabase();

        /// <summary>
        /// 异步尝试创建数据库。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default);
    }
}
