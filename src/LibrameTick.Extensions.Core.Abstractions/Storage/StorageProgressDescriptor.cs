#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Storage
{
    /// <summary>
    /// 定义存储进度描述符。
    /// </summary>
    public record StorageProgressDescriptor
    {
        /// <summary>
        /// 内容长度。
        /// </summary>
        public long ContentLength { get; init; }

        /// <summary>
        /// 开始位置。
        /// </summary>
        public long StartPosition { get; init; }

        /// <summary>
        /// 处理大小。
        /// </summary>
        public long ProcessingSize { get; init; }

        /// <summary>
        /// 处理速度。
        /// </summary>
        public long ProcessingSpeed { get; init; }

        /// <summary>
        /// 处理百分比。
        /// </summary>
        public int ProcessingPercent {  get; init; }
    }
}
