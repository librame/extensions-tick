#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个可注册目录集合接口。
/// </summary>
public interface IRegisterableDirectories : IRegisterable
{
    /// <summary>
    /// 基础目录。
    /// </summary>
    string BaseDirectory { get; }


    /// <summary>
    /// 配置目录（通常用于存放功能配置）。
    /// </summary>
    string ConfigDirectory { get; }

    /// <summary>
    /// 报告目录（通常用于存放生成报告）。
    /// </summary>
    string ReportDirectory { get; }

    /// <summary>
    /// 资源目录（通常用于存放静态资源）。
    /// </summary>
    string ResourceDirectory { get; }
}
