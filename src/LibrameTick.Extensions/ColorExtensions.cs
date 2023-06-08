#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// 颜色静态扩展。
/// </summary>
/// <remarks>
/// <para>参考：https://mp.weixin.qq.com/s/9shZsup87fIQn9_z_Cqk9Q</para>
/// </remarks>
public static class ColorExtensions
{

    /// <summary>
    /// 获取两个颜色的分量值，可用于判定两个颜色相似的程度。
    /// </summary>
    /// <param name="left">给定的 <see cref="Color"/>。</param>
    /// <param name="right">给定的 <see cref="Color"/>。</param>
    /// <returns>返回整数。</returns>
    public static int GetComponent(this Color left, Color right)
    {
        var diffR = Math.Abs(left.R - right.R);
        var diffG = Math.Abs(left.G - right.G);
        var diffB = Math.Abs(left.B - right.B);

        return diffR + diffG + diffB;
    }

    /// <summary>
    /// 获取两个颜色之间的欧几里德距离，即两个颜色在 RGB 中的空间距离，可用于判定两个颜色相似的程度。
    /// </summary>
    /// <param name="left">给定的 <see cref="Color"/>。</param>
    /// <param name="right">给定的 <see cref="Color"/>。</param>
    /// <returns>返回浮点数。</returns>
    public static double GetEuclideanDistance(this Color left, Color right)
    {
        var diffR = left.R - right.R;
        var diffG = left.G - right.G;
        var diffB = left.B - right.B;

        return Math.Sqrt(diffR * diffR + diffG * diffG + diffB * diffB);
    }


    /// <summary>
    /// 判定两个颜色是否相似。
    /// </summary>
    /// <param name="left">给定的 <see cref="Color"/>。</param>
    /// <param name="right">给定的 <see cref="Color"/>。</param>
    /// <param name="componentThreshold">用于判定相似程度的分量阈值（可选；默认 15，通过 #FFFFFF@R255-G255-B255 与 #FAFAFA@R250-G250-B250 测算）。</param>
    /// <param name="distanceThreshold">用于判定相似程度的欧几里德距离阈值（可选；默认 8.66，通过 #FFFFFF@R255-G255-B255 与 #FAFAFA@R250-G250-B250 测算）。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsSimilar(this Color left, Color right,
        double componentThreshold = 15, double distanceThreshold = 8.66)
    {
        var component = left.GetComponent(right);
        var distance = left.GetEuclideanDistance(right).SubWithoutRound(2);

        return component <= componentThreshold && distance <= distanceThreshold;
    }

}
