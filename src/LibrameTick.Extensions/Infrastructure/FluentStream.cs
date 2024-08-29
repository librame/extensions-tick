#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义实现 <see cref="Fluent{TSelf, TChain}"/> 的流畅字节序列流。
/// </summary>
/// <param name="initialStream">给定的 <see cref="Stream"/>。</param>
/// <param name="useBufferedStream">是否使用缓冲流（可选；默认使用 <see cref="BufferedStream"/> 处理 <paramref name="initialStream"/>，除非 <paramref name="initialStream"/> 本身已是 <see cref="BufferedStream"/>）。</param>
public class FluentStream(Stream initialStream, bool useBufferedStream = true)
    : Fluent<FluentStream, Stream>(initialStream), IEquatable<FluentStream>, IDisposable
{
    /// <summary>
    /// 获取是否使用缓冲流。
    /// </summary>
    public bool UseBufferedStream { get; init; } = useBufferedStream;

    /// <summary>
    /// 重写当前流。
    /// </summary>
    /// <remarks>
    ///     <para>重写以在启用 <see cref="UseBufferedStream"/> 时，使用缓冲流处理 <see cref="Fluent{TSelf, TChain}.CurrentValue"/>。</para>
    /// </remarks>
    public override Stream CurrentValue
    {
        get
        {
            return UseBufferedStream
                ? base.CurrentValue.AsBufferedStream()
                : base.CurrentValue;
        }

        protected set => base.CurrentValue = value;
    }


    /// <summary>
    /// 使用新内存流处理链式动作。
    /// </summary>
    /// <remarks>
    ///     <para>使用 <see cref="RecyclableMemoryStream"/> 或其 <see cref="BufferedStream"/> 包装流作为新内存流，并在动作完成后释放。</para>
    /// </remarks>
    /// <param name="newMemoryStreamAction">给定的新内存流动作。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public virtual FluentStream ChainWithNewMemory(Action<Stream> newMemoryStreamAction)
    {
        using var memoryStream = CreateMemoryOrBufferedStream();
        newMemoryStreamAction(memoryStream);

        return this;
    }

    /// <summary>
    /// 创建内存或缓冲流。
    /// </summary>
    /// <returns>返回 <see cref="Stream"/>。</returns>
    public virtual Stream CreateMemoryOrBufferedStream()
    {
        Stream newStream = DependencyRegistration.CurrentContext.MemoryStreams.GetStream();

        if (UseBufferedStream)
        {
            newStream = newStream.AsBufferedStream();
        }

        return newStream;
    }


    /// <summary>
    /// 从指定的副本流复制到当前下层流。
    /// </summary>
    /// <param name="beCopiedStream">给定被复制的流。</param>
    /// <param name="disposing">复制后是否立即释放 <paramref name="beCopiedStream"/> 资源（可选；默认立即释放）。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public virtual FluentStream CopyFrom(ref Stream beCopiedStream, bool disposing = true)
    {
        var currentStream = CurrentValue;

        beCopiedStream.ResetOriginalPositionIfNotBegin();
        beCopiedStream.CopyTo(currentStream);

        currentStream.Flush();

        if (disposing)
        {
            beCopiedStream.Dispose();
        }

        return this;
    }


    /// <summary>
    /// 链式下层流动作。
    /// </summary>
    /// <param name="currentStreamAction">给定的 <see cref="CurrentValue"/> 动作。</param>
    /// <param name="flushing">处理后是否立即刷新当前下层流（可选；默认刷新）。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public virtual FluentStream ChainUnderly(Action<Stream> currentStreamAction, bool flushing = true)
    {
        var currentStream = CurrentValue;
        currentStreamAction(currentStream);

        if (flushing)
        {
            currentStream.Flush();
        }

        return this;
    }

    /// <summary>
    /// 链式下层流动作。
    /// </summary>
    /// <remarks>
    ///     <para>使用 <see cref="RecyclableMemoryStream"/> 或其 <see cref="BufferedStream"/> 包装流作为新内存流，并在动作完成后释放。</para>
    /// </remarks>
    /// <param name="currentStreamAction">给定的 <see cref="CurrentValue"/> 与新内存流动作。</param>
    /// <param name="flushing">处理后是否立即刷新当前下层流（可选；默认刷新）。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public virtual FluentStream ChainUnderlyWithNewMemory(Action<Stream, Stream> currentStreamAction,
        bool flushing = true)
    {
        var currentStream = CurrentValue;
        using var memoryStream = CreateMemoryOrBufferedStream();

        currentStreamAction(currentStream, memoryStream);

        if (flushing)
        {
            currentStream.Flush();
        }

        return this;
    }


    /// <summary>
    /// 从下层流还原指定结果。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="fromFunc">给定的还原方法。</param>
    /// <param name="disposing">还原后是否立即释放资源（可选；默认调用 <see cref="Dispose"/> 释放当前所有流资源）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public virtual TResult FromUnderly<TResult>(Func<Stream, TResult> fromFunc,
        bool disposing = true)
    {
        var stream = CurrentValue;
        stream.ResetOriginalPositionIfNotBegin();

        var result = fromFunc(stream);

        if (disposing)
        {
            Dispose();
        }

        return result;
    }


    /// <summary>
    /// 使用新内存流处理链式动作并切换到指定流。
    /// </summary>
    /// <param name="switchedStream">给定被切换的流。</param>
    /// <param name="newMemoryStreamAction">给定用于切换的内存流动作（传入参数即为 <paramref name="switchedStream"/>）。</param>
    public virtual void SwitchNewMemory(ref Stream switchedStream,
        Action<Stream, Stream> newMemoryStreamAction)
    {
        switchedStream.ResetOriginalPositionIfNotBegin();

        var memoryStream = CreateMemoryOrBufferedStream();
        newMemoryStreamAction(switchedStream, memoryStream);

        switchedStream.Dispose();
        switchedStream = memoryStream;
    }

    /// <summary>
    /// 切换下层流动作。
    /// </summary>
    /// <remarks>
    ///     <para>使用 <see cref="RecyclableMemoryStream"/> 或其 <see cref="BufferedStream"/> 包装流作为新内存流，并在动作完成后切换为 <see cref="CurrentValue"/>。</para>
    /// </remarks>
    /// <param name="currentStreamAction">给定的新内存流动作。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public virtual FluentStream SwitchUnderlyWithNewMemory(Action<Stream, Stream> currentStreamAction)
    {
        return Switch(fluent =>
        {
            var memoryStream = CreateMemoryOrBufferedStream();
            currentStreamAction(fluent.CurrentValue, memoryStream);

            return memoryStream;
        });
    }

    /// <summary>
    /// 切换当前流。
    /// </summary>
    /// <param name="newStreamFunc">给定新流的方法。</param>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public override FluentStream Switch(Func<FluentStream, Stream> newStreamFunc)
    {
        var current = CurrentValue;

        return base.Switch(stream =>
        {
            var newStream = newStreamFunc(stream);
            current?.Dispose();

            return newStream;
        });
    }

    /// <summary>
    /// 复制一个当前流畅字节序列流的副本。
    /// </summary>
    /// <returns>返回 <see cref="FluentStream"/>。</returns>
    public override FluentStream Copy()
        => new(CurrentValue);


    /// <summary>
    /// 释放当前所有字节序列流资源。
    /// </summary>
    public virtual void Dispose()
    {
        var current = CurrentValue;
        var initial = InitialValue;

        current?.Dispose();
        initial?.Dispose();

        GC.SuppressFinalize(this);
    }


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => CurrentValue.GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回当前路径值字符串。</returns>
    public override string ToString()
        => GetType().ToString(); // 此处不能使用 CurrentValue，否则会引发循环调用


    /// <summary>
    /// 相等的流畅字节序列流。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as FluentStream);

    /// <summary>
    /// 相等的流畅字节序列流。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="FluentStream"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(FluentStream? other)
        => other is not null && other.CurrentValue == CurrentValue;


    /// <summary>
    /// 将当前 <see cref="FluentStream"/> 隐式转换为字节序列流形式。
    /// </summary>
    /// <param name="stream">给定的 <see cref="FluentStream"/>。</param>
    public static implicit operator Stream(FluentStream stream)
        => stream.CurrentValue;

}
