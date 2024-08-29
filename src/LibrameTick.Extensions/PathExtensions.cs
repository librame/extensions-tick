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
/// 定义 <see cref="Path"/> 静态扩展。
/// </summary>
public static class PathExtensions
{

    #region ChangeExtension

    /// <summary>
    /// 在指定路径字符串的扩展名基础上附加扩展名。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <param name="appendExtension">给定要附加的扩展名。</param>
    /// <param name="appendExtensionFunc">给定的附加方法（可选；默认在当前扩展名后附加扩展名）。</param>
    /// <returns>返回附加后的路径字符串。</returns>
    public static string AppendExtension(this string path, string? appendExtension,
        Func<string, string>? appendExtensionFunc = null)
    {
        if (appendExtension is null) return path;

        var pathExtension = Path.GetExtension(path);
        if (pathExtension is null)
        {
            return Path.GetFullPath($"{path}{appendExtension}");
        }

        appendExtensionFunc ??= LocalAppendExtension;

        return Path.ChangeExtension(path, appendExtensionFunc(pathExtension));


        string LocalAppendExtension(string oldExtension)
            => $"{oldExtension}{appendExtension}";
    }


    /// <summary>
    /// 更改指定路径字符串的扩展名。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <param name="newExtensionFunc">给定的新扩展名方法。</param>
    /// <returns>返回更改后的路径字符串。</returns>
    public static string ChangeExtension(this string path, Func<string?, string> newExtensionFunc)
    {
        var oldExtension = Path.GetExtension(path);
        if (oldExtension is null)
        {
            return Path.GetFullPath($"{path}{newExtensionFunc(null)}");
        }

        return path.ChangeExtension(newExtensionFunc(oldExtension));
    }

    /// <summary>
    /// 更改指定路径字符串的扩展名。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <param name="newExtension">给定的新扩展名。</param>
    /// <returns>返回更改后的路径字符串。</returns>
    public static string ChangeExtension(this string path, string? newExtension)
        => Path.ChangeExtension(path, newExtension);


    /// <summary>
    /// 获取指定路径字符串的扩展名。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <returns>返回扩展名字符串。</returns>
    public static string? GetExtension(this string path)
        => Path.GetExtension(path);

    #endregion

}
