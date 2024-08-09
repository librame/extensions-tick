#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Device;

/// <summary>
/// 定义一个网络流量。
/// </summary>
public sealed class NetworkTraffic : StaticDefaultInitializer<NetworkTraffic>
{
    /// <summary>
    /// 产生时间。
    /// </summary>
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 总接收字节数。
    /// </summary>
    public long ReceivedLength { get; set; }

    /// <summary>
    /// 总发送字节数。
    /// </summary>
    public long SendLength { get; set; }

    /// <summary>
    /// 接收速率。
    /// </summary>
    public float ReceivedRate { get; set; }

    /// <summary>
    /// 发送速率。
    /// </summary>
    public float SendRate { get; set; }


    /// <summary>
    /// 计算利用率。
    /// </summary>
    /// <param name="fixedSpeed">给定的固定速率。</param>
    /// <returns>返回浮点数。</returns>
    public float CalcUsageRate(long fixedSpeed)
    {
        // 全双工模式使用单向字节数除以固定速率
        var maxLength = ReceivedLength > SendLength ? ReceivedLength : SendLength;
        return fixedSpeed == 0 ? 0 : Convert.ToSingle(maxLength / fixedSpeed);
    }


    /// <summary>
    /// 使用新发送与接收速率创建一个实例副本。
    /// </summary>
    /// <param name="newReceivedRate">给定的新接收速率。</param>
    /// <param name="newSendRate">给定的新发送速率。</param>
    /// <returns>返回 <see cref="ProcessorMoment"/>。</returns>
    public NetworkTraffic WithRate(float newReceivedRate, float newSendRate)
        => Create(CreateTime, ReceivedLength, SendLength, newReceivedRate, newSendRate);


    /// <summary>
    /// 创建网络流量。
    /// </summary>
    /// <param name="createTime">给定的产生时间。</param>
    /// <param name="receivedLength">给定的总接收字节数。</param>
    /// <param name="sendLength">给定的总发送字节数。</param>
    /// <param name="lastTraffic">上一次 <see cref="NetworkTraffic"/>。</param>
    /// <returns>返回 <see cref="NetworkTraffic"/>。</returns>
    public static NetworkTraffic Create(DateTimeOffset createTime, long receivedLength, long sendLength,
        NetworkTraffic lastTraffic)
    {
        var interval = (createTime - lastTraffic.CreateTime).TotalSeconds;
        var receive = (receivedLength - lastTraffic.ReceivedLength) / interval;
        var send = (sendLength - lastTraffic.SendLength) / interval;

        return new()
        {
            CreateTime = createTime,
            ReceivedLength = receivedLength,
            SendLength = sendLength,
            ReceivedRate = Convert.ToSingle(receive),
            SendRate = Convert.ToSingle(send)
        };
    }

    /// <summary>
    /// 创建网络流量。
    /// </summary>
    /// <param name="createTime">给定的产生时间。</param>
    /// <param name="receivedLength">给定的总接收字节数。</param>
    /// <param name="sendLength">给定的总发送字节数。</param>
    /// <param name="receivedRate">给定的接收速率，通常以百分比数值表示（默认为 -1）。</param>
    /// <param name="sendRate">给定的发送速率，通常以百分比数值表示（默认为 -1）。</param>
    /// <returns>返回 <see cref="NetworkTraffic"/>。</returns>
    public static NetworkTraffic Create(DateTimeOffset createTime, long receivedLength, long sendLength,
        float? receivedRate = null, float? sendRate = null)
    {
        return new()
        {
            CreateTime = createTime,
            ReceivedLength = receivedLength,
            SendLength = sendLength,
            ReceivedRate = receivedRate ?? -1,
            SendRate = sendRate ?? -1
        };
    }

}
