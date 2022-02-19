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
/// 定义一个可序列化的自动密钥。
/// </summary>
public class Autokey
{
    /// <summary>
    /// 密钥标识。
    /// </summary>
    public byte[]? Id { get; set; }


    /// <summary>
    /// HMACMD5 密钥。
    /// </summary>
    public byte[]? HmacMd5Key { get; set; }

    /// <summary>
    /// HMACSHA256 密钥。
    /// </summary>
    public byte[]? HmacSha256Key { get; set; }

    /// <summary>
    /// HMACSHA384 密钥。
    /// </summary>
    public byte[]? HmacSha384Key { get; set; }

    /// <summary>
    /// HMACSHA512 密钥。
    /// </summary>
    public byte[]? HmacSha512Key { get; set; }

    /// <summary>
    /// AES 密钥。
    /// </summary>
    public byte[]? AesKey { get; set; }

    /// <summary>
    /// AES 向量。
    /// </summary>
    public byte[]? AesIV { get; set; }


    /// <summary>
    /// 获取默认 <see cref="Autokey"/> 的文件路径。
    /// </summary>
    /// <returns>返回路径字符串。</returns>
    public static string GetDefaultFilePath()
    {
        // 默认支持格式为“当前项目程序集名.autokey”的文件名
        // 且限定自动密钥文件需存放在与程序集文件相同的目录
        return $"{typeof(Autokey).Assembly.GetName().Name}.autokey"
            .SetBasePath(PathExtensions.CurrentDirectory);
    }


    /// <summary>
    /// 读取 JSON 格式的自动密钥文件。
    /// </summary>
    /// <param name="filePath">给定的 JSON 文件路径（可选；默认使用 <see cref="GetDefaultFilePath()"/>）。</param>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    /// <exception cref="NotSupportedException">Unsupported autokey file format.</exception>
    public static Autokey ReadJsonFile(string? filePath = null)
    {
        if (string.IsNullOrEmpty(filePath))
            filePath = GetDefaultFilePath();

        var keys = filePath.DeserializeJsonFile<Autokey>();
        if (keys is null)
            throw new NotSupportedException("Unsupported autokey file format.");

        return keys;
    }

    /// <summary>
    /// 写入 JSON 格式的自动密钥文件。
    /// </summary>
    /// <param name="filePath">给定的 JSON 文件路径（可选；默认使用 <see cref="GetDefaultFilePath()"/>）。</param>
    /// <param name="autokey">给定的 <see cref="Autokey"/>（可选；默认新建实例）。</param>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public static Autokey WriteJsonFile(string? filePath = null, Autokey? autokey = null)
    {
        if (string.IsNullOrEmpty(filePath))
            filePath = GetDefaultFilePath();

        if (autokey is null)
        {
            autokey = new Autokey
            {
                Id = RandomExtensions.GenerateByteArray(16),
                HmacMd5Key = RandomExtensions.GenerateByteArray(8),
                HmacSha256Key = RandomExtensions.GenerateByteArray(8),
                HmacSha384Key = RandomExtensions.GenerateByteArray(16),
                HmacSha512Key = RandomExtensions.GenerateByteArray(16),
                AesKey = RandomExtensions.GenerateByteArray(32),
                AesIV = RandomExtensions.GenerateByteArray(16)
            };
        }

        filePath.SerializeJsonFile(autokey);
        return autokey;
    }

}
