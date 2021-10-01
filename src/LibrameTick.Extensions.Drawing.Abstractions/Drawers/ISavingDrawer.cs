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

namespace Librame.Extensions.Drawing.Drawers;

/// <summary>
/// 定义实现 <see cref="IDrawer"/> 的存储绘制器接口。
/// </summary>
public interface ISavingDrawer : IDrawer
{
    /// <summary>
    /// 保存子路径方法。默认以当前本地时间为基础，建立年月和日的相对二级子路径（如：2108\15）。
    /// </summary>
    Func<IClock, string> SaveSubpathFunc { get; set; }

    /// <summary>
    /// 保存文件基础名称方法（即不包含路径和文件扩展名的文件名）。默认如果位图来自于文件，则尝试获取文件基础名，反之则使用当前本地时间的周期毫秒数。
    /// </summary>
    Func<IClock, BitmapDescriptor, string> SaveFileBaseNameFunc { get; set; }
}
