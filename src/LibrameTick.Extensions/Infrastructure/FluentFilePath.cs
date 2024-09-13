#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义实现 <see cref="AbstractFluent{TSelf, TChain}"/> 的流畅文件路径。
/// </summary>
/// <param name="initialPath">给定的初始路径。</param>
/// <param name="dependency">给定的 <see cref="IPathDependency"/>（可选；默认使用 <see cref="DependencyRegistration.CurrentContext"/> 的路径依赖）。</param>
/// <param name="encoding">给定的字符编码（可选；默认使用 <see cref="DependencyRegistration.CurrentContext"/> 的字符编码）。</param>
public class FluentFilePath(string initialPath, IPathDependency? dependency = null, Encoding? encoding = null)
    : FluentPath(initialPath, dependency)
{
    /// <summary>
    /// 构造一个 <see cref="FluentFilePath"/>。
    /// </summary>
    /// <param name="fluentPath">给定的 <see cref="FluentPath"/>。</param>
    public FluentFilePath(FluentPath fluentPath)
        : this(fluentPath.CurrentValue, fluentPath.Dependency)
    {
    }


    /// <summary>
    /// 获取当前文件路径的字符编码。
    /// </summary>
    /// <value>
    /// 返回 <see cref="System.Text.Encoding"/>。
    /// </value>
    public Encoding Encoding { get; init; }
        = encoding ?? DependencyRegistration.CurrentContext.Encoding;


    /// <summary>
    /// 获取当前路径的扩展名。
    /// </summary>
    public virtual string? Extension
        => Path.GetExtension(CurrentValue);

    /// <summary>
    /// 获取当前路径的文件名（不包含扩展名）。
    /// </summary>
    public virtual string? FileNameWithoutExtension
        => Path.GetFileNameWithoutExtension(CurrentValue);

    /// <summary>
    /// 判断当前路径是否有扩展名。
    /// </summary>
    public virtual bool HasExtension
        => Path.HasExtension(CurrentValue);


    /// <summary>
    /// 删除当前路径。
    /// </summary>
    public override void Delete()
        => File.Delete(CurrentValue);

    /// <summary>
    /// 检查当前路径是否存在。
    /// </summary>
    /// <returns>返回是否存在的布尔值。</returns>
    public override bool Exists()
        => File.Exists(CurrentValue);

    /// <summary>
    /// 创建当前路径的目录。
    /// </summary>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    public override DirectoryInfo CreateDirectory()
        => Directory.CreateDirectory(GetDirectoryName());


    /// <summary>
    /// 在当前文件路径字符串的扩展名基础上附加扩展名副本。
    /// </summary>
    /// <param name="appendExtension">给定要附加的扩展名。</param>
    /// <param name="appendExtensionFunc">给定的附加方法（可选；默认在当前扩展名后附加扩展名）。</param>
    /// <returns>返回附加后的路径字符串。</returns>
    public virtual string AppendExtension(string? appendExtension,
        Func<string, string>? appendExtensionFunc = null)
        => CurrentValue.AppendExtension(appendExtension, appendExtensionFunc);

    /// <summary>
    /// 修改当前文件路径的扩展名副本。
    /// </summary>
    /// <param name="newExtensionFunc">给定的新扩展名方法。</param>
    /// <returns>返回更改后的路径字符串。</returns>
    public virtual string ChangeExtension(Func<string?, string> newExtensionFunc)
        => CurrentValue.ChangeExtension(newExtensionFunc);

    /// <summary>
    /// 修改当前文件路径的扩展名副本。
    /// </summary>
    /// <param name="newExtension">给定的新扩展名。</param>
    /// <returns>返回更改后的路径字符串。</returns>
    public virtual string ChangeExtension(string newExtension)
        => Path.ChangeExtension(CurrentValue, newExtension);


    /// <summary>
    /// 在当前文件路径字符串的扩展名基础上设置附加扩展名。
    /// </summary>
    /// <param name="newExtension">给定要附加的扩展名。</param>
    /// <param name="appendExtensionFunc">给定的附加方法（可选；默认在当前扩展名后附加扩展名）。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SetAppendExtension(string? newExtension,
        Func<string, string>? appendExtensionFunc = null)
        => SwitchFile(path => AppendExtension(newExtension, appendExtensionFunc));

    /// <summary>
    /// 为当前文件路径设置扩展名。
    /// </summary>
    /// <param name="newExtensionFunc">给定的新扩展名方法。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SetExtension(Func<string?, string> newExtensionFunc)
        => SwitchFile(path => ChangeExtension(newExtensionFunc));

    /// <summary>
    /// 为当前文件路径设置扩展名。
    /// </summary>
    /// <param name="newExtension">给定的新扩展名。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SetExtension(string newExtension)
        => SwitchFile(path => ChangeExtension(newExtension));


    /// <summary>
    /// 为当前文件路径设置基础路径。
    /// </summary>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法（需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="FluentFilePath"/>。</returns>
    public override FluentPath SetBasePath(Func<IPathDependency, string> basePathFunc)
        => SetBasePath(basePathFunc(Dependency));

    /// <summary>
    /// 为当前文件路径设置基础路径。
    /// </summary>
    /// <param name="basePath">给定的基础路径。</param>
    /// <returns>返回当前 <see cref="FluentFilePath"/>。</returns>
    public override FluentPath SetBasePath(string basePath)
        => SwitchFile(path => Path.Combine(basePath, CurrentValue));


    /// <summary>
    /// 切换文件路径字符串。
    /// </summary>
    /// <param name="newFilePathfunc">给定新文件路径字符串的方法。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SwitchFile(Func<FluentFilePath, string> newFilePathfunc)
    {
        base.Switch(fluent => Path.GetFullPath(newFilePathfunc(this)));
        return this;
    }


    /// <summary>
    /// 创建一个当前流畅文件路径的副本。
    /// </summary>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    protected override FluentPath Create()
        => new FluentFilePath(CurrentValue, Dependency, Encoding);


    /// <summary>
    /// 使用二进制存取文件插件。
    /// </summary>
    /// <returns>返回 <see cref="IBinaryAccessFilePlugin"/>。</returns>
    public virtual IBinaryAccessFilePlugin UseBinaryAccessFilePlugin()
        => UsePlugin(fluent => new BinaryAccessFilePlugin((FluentFilePath)fluent));

}
