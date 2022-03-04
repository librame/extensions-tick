#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义抽象实现 <see cref="IExtensionOptions"/> 的泛型扩展选项。
/// </summary>
/// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
public abstract class AbstractExtensionOptions<TOptions> : AbstractExtensionOptions
    where TOptions : IExtensionOptions
{
    /// <summary>
    /// 使用 <see cref="Bootstrapper.GetDirectories()"/> 构造一个 <see cref="AbstractExtensionOptions{TOptions}"/>。
    /// </summary>
    protected AbstractExtensionOptions()
        : base()
    {
    }

    /// <summary>
    /// 构造一个 <see cref="AbstractExtensionOptions{TOptions}"/>。
    /// </summary>
    /// <param name="directories">给定的 <see cref="IDirectoryStructureBootstrap"/>。</param>
    protected AbstractExtensionOptions(IDirectoryStructureBootstrap directories)
        : base(directories)
    {
    }


    /// <summary>
    /// 扩展类型。
    /// </summary>
    [JsonIgnore]
    public override Type ExtensionType
        => typeof(TOptions);
}


/// <summary>
/// 定义抽象实现 <see cref="IExtensionOptions"/>、<see cref="IExtensionInfo"/> 的扩展选项。
/// </summary>
public abstract class AbstractExtensionOptions : AbstractExtensionInfo, IExtensionOptions
{
    /// <summary>
    /// 使用 <see cref="Bootstrapper.GetDirectories()"/> 构造一个 <see cref="AbstractExtensionOptions"/>。
    /// </summary>
    protected AbstractExtensionOptions()
        : this(Bootstrapper.GetDirectories())
    {
    }

    /// <summary>
    /// 构造一个 <see cref="AbstractExtensionOptions"/>。
    /// </summary>
    /// <param name="directories">给定的 <see cref="IDirectoryStructureBootstrap"/>。</param>
    protected AbstractExtensionOptions(IDirectoryStructureBootstrap directories)
    {
        Directories = directories;
    }


    /// <summary>
    /// 目录集合。
    /// </summary>
    public IDirectoryStructureBootstrap Directories { get; set; }
}
