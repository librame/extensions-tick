#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Drawing.Processing
{
    /// <summary>
    /// <see cref="IProcessorManager"/> 静态扩展。
    /// </summary>
    public static class ProcessorManagerExtensions
    {

        /// <summary>
        /// 使用可绘制保存处理器。
        /// </summary>
        /// <param name="processors">给定的 <see cref="IProcessorManager"/>。</param>
        /// <returns>返回 <see cref="ISavingDrawableProcessor"/>。</returns>
        public static ISavingDrawableProcessor UseSavingProcessor(this IProcessorManager processors)
            => processors.UseProcessor<ISavingDrawableProcessor>();

        /// <summary>
        /// 使用可绘制缩放处理器。
        /// </summary>
        /// <param name="processors">给定的 <see cref="IProcessorManager"/>。</param>
        /// <returns>返回 <see cref="IScalingDrawableProcessor"/>。</returns>
        public static IScalingDrawableProcessor UseScalingProcessor(this IProcessorManager processors)
            => processors.UseProcessor<IScalingDrawableProcessor>();

        /// <summary>
        /// 使用可绘制水印处理器。
        /// </summary>
        /// <param name="processors">给定的 <see cref="IProcessorManager"/>。</param>
        /// <returns>返回 <see cref="IWatermarkDrawableProcessor"/>。</returns>
        public static IWatermarkDrawableProcessor UseWatermarkProcessor(this IProcessorManager processors)
            => processors.UseProcessor<IWatermarkDrawableProcessor>();

    }
}
