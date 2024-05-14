#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure.Dependency;

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义算法文件路径。
/// </summary>
public static class AlgorithmFilePath
{

    /// <summary>
    /// 构建算法密钥环的文件路径。
    /// </summary>
    /// <param name="dependency">给定的 <see cref="IDependencyContext"/>。</param>
    /// <returns>返回包含文件名的路径字符串。</returns>
    public static string BuildAlgorithmKeyringFilePath(IDependencyContext dependency)
    {
        var filePath = BuildFileName(dependency, $"{dependency.AssemblyNameString}.keyring");
        return filePath;
    }

    /// <summary>
    /// 构建非对称算法私有数字证书的文件路径。
    /// </summary>
    /// <param name="dependency">给定的 <see cref="IDependencyContext"/>。</param>
    /// <returns>返回包含文件名的路径字符串。</returns>
    public static string BuildAsymmetricPrivateCertFilePath(IDependencyContext dependency)
    {
        var filePath = BuildFileName(dependency, $"{dependency.AssemblyNameString}.pfx");
        return filePath;
    }

    /// <summary>
    /// 构建签名私有数字证书的文件路径。
    /// </summary>
    /// <param name="dependency">给定的 <see cref="IDependencyContext"/>。</param>
    /// <returns>返回包含文件名的路径字符串。</returns>
    public static string BuildSignaturePrivateCertFilePath(IDependencyContext dependency)
    {
        var fileBaseName = $"{dependency.AssemblyNameString}.Signature";

        var filePath = BuildFileName(dependency, $"{fileBaseName}.pfx");
        return filePath;
    }

    /// <summary>
    /// 构建公有数字证书的文件路径。
    /// </summary>
    /// <param name="privateCertFilePath">给定的私有数字证书的文件路径。</param>
    /// <returns>返回包含文件名的路径字符串。</returns>
    public static string BuildPublicCertFilePath(string privateCertFilePath)
        => Path.ChangeExtension(privateCertFilePath, ".cer");


    private static string BuildFileName(IDependencyContext dependency, string fileNameWithoutPath)
        => $"{dependency.Paths.InitialPath}{Path.DirectorySeparatorChar}{fileNameWithoutPath}";

}
