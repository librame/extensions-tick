#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Crypto;

/// <summary>
/// 定义继承 <see cref="AbstractFileAutokeyProvider"/> 的 JSON 文件型自动密钥提供程序。
/// </summary>
public class JsonFileAutokeyProvider : AbstractFileAutokeyProvider
{
    /// <summary>
    /// 构造一个 <see cref="JsonFileAutokeyProvider"/>。
    /// </summary>
    /// <param name="filePath">给定的文件路径（可选）。</param>
    public JsonFileAutokeyProvider(string? filePath = null)
        : base(filePath)
    {
    }


    /// <summary>
    /// 加载自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public override Autokey Load()
    {
        var autokey = FilePath.DeserializeJsonFile<Autokey>();
        if (autokey is null)
            throw new NotSupportedException("Unsupported autokey file format.");

        return autokey;
    }

    /// <summary>
    /// 保存自动密钥。
    /// </summary>
    /// <param name="autokey">给定的 <see cref="Autokey"/>。</param>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public override Autokey Save(Autokey autokey)
    {
        FilePath.SerializeJsonFile(autokey);

        return autokey;
    }

}
