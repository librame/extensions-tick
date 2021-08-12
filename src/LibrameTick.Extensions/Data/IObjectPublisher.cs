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
    /// 定义对象发表者接口。
    /// </summary>
    public interface IObjectPublisher : IObjectCreator
    {
        /// <summary>
        /// 发表者类型。
        /// </summary>
        Type PublishedByType { get; }


        /// <summary>
        /// 获取对象发表者。
        /// </summary>
        /// <returns>返回发表者（兼容标识或字符串）。</returns>
        object? GetObjectPublishedBy();

        /// <summary>
        /// 异步获取对象发表者。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object?> GetObjectPublishedByAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 设置对象发表者。
        /// </summary>
        /// <param name="newPublishedBy">给定的新发表者对象。</param>
        /// <returns>返回发表者（兼容标识或字符串）。</returns>
        object? SetObjectPublishedBy(object? newPublishedBy);

        /// <summary>
        /// 异步设置对象发表者。
        /// </summary>
        /// <param name="newPublishedBy">给定的新发表者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
        ValueTask<object?> SetObjectPublishedByAsync(object? newPublishedBy, CancellationToken cancellationToken = default);
    }
}
