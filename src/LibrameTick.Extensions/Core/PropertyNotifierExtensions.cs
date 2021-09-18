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
/// <see cref="IPropertyNotifier"/> 静态扩展。
/// </summary>
public static class PropertyNotifierExtensions
{

    /// <summary>
    /// 获取添加目录。默认获取到目录后将尝试检测目录是否存在（不为空），如果不存在则会尝试创建。
    /// </summary>
    /// <param name="notifier">给定的 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="propertyName">给定的目录属性。</param>
    /// <param name="addPropertyValue">给定的添加目录。</param>
    /// <returns>返回目录字符串。</returns>
    public static string GetOrAddDirectory(this IPropertyNotifier notifier, string propertyName, string addPropertyValue)
        => notifier.GetOrAdd(propertyName, addPropertyValue, gotAction: dir =>
        {
            if (!string.IsNullOrEmpty(dir) && !dir.DirectoryExists())
                dir.CreateDirectory();
        });

}
