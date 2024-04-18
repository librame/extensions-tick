#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies.Internal;

internal sealed class PathManager : IPathManager
{
    public PathManager()
        : this(Environment.CurrentDirectory)
    {
        // CurrentDirectory 路径不包含最后一个目录分隔符字符
    }

    public PathManager(string initialPath)
    {
        var (basePath, binPath) = GetPreviousDirectory(initialPath, binPath: null);

        InitialPath = initialPath;
        BasePath = basePath;
        BinPath = binPath;
    }


    public string InitialPath { get; init; }

    public string BasePath { get; init; }

    public string? BinPath { get; init; }


    private static readonly string _binSubDir = $"{Path.DirectorySeparatorChar}bin";

    private static (string basePath, string? binPath) GetPreviousDirectory(string path, string? binPath)
    {
        if (path.Contains(_binSubDir))
        {
            if (path.EndsWith(_binSubDir))
            {
                binPath = path;
            }

            // 利用 GetDirectoryName 每次会截断路径（不包含最后一个目录分隔符字符）
            // 的子级来返回前一级目录
            var prevDirName = Path.GetDirectoryName(path);
            if (prevDirName is null)
            {
                return (path, binPath);
            }

            return GetPreviousDirectory(prevDirName, binPath);
        }

        return (path, binPath);
    }

}
