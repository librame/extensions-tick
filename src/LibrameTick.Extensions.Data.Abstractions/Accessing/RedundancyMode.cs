#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义支持对存取器集合进行镜像或分割的冗余模式。
/// </summary>
public enum RedundancyMode
{
    /// <summary>
    /// 未启用。
    /// </summary>
    None,

    /// <summary>
    /// 复合模式。在此模式下，同分组的存取器将复合为一，所有存取器均遍历执行。
    /// </summary>
    Compositing,

    /// <summary>
    /// 镜像模式。在此模式下，同分组的存取器将组合为镜像，互为主备存取，适用于高安全性部署场景。
    /// </summary>
    Mirroring,

    /// <summary>
    /// 分割模式。在此模式下，同分组的存取器将切分为不同条带分散存取，适用于安全性不高的高并发部署场景。
    /// </summary>
    Striping
}
