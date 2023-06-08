#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义抽象实现 <see cref="IFileSettingProvider{TSettingRoot}"/> 的文件型设置提供程序。
/// </summary>
public abstract class AbstractFileSettingProvider<TSettingRoot> : AbstractSettingProvider<TSettingRoot>, IFileSettingProvider<TSettingRoot>
    where TSettingRoot : ISettingRoot
{
    /// <summary>
    /// 构造一个 <see cref="AbstractFileSettingProvider{TSettingRoot}"/>。
    /// </summary>
    /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
    /// <param name="filePath">给定的文件路径。</param>
    protected AbstractFileSettingProvider(ILoggerFactory loggerFactory, string filePath)
    {
        Logger = loggerFactory.CreateLogger(GetType());
        FilePath = filePath;
    }


    /// <summary>
    /// 日志。
    /// </summary>
    protected ILogger Logger { get; init; }

    /// <summary>
    /// 文件路径。
    /// </summary>
    public string FilePath { get; }


    /// <summary>
    /// 存在设置。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public override bool Exist()
    {
        if (!File.Exists(FilePath))
        {
            Logger.LogWarning($"The file '{FilePath}' is not found.");
            return false;
        }

        return true;
    }

}
