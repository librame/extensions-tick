#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies;

/// <summary>
/// 定义继承 <see cref="AbstractFilePersistenceProvider{TPersistence}"/> 的 JSON 文件持久化提供程序。
/// </summary>
/// <typeparam name="TPersistence">指定的持久化类型。</typeparam>
/// <param name="isWatching">是否监视文件变化。</param>
/// <param name="filePath">给定的文件路径。</param>
/// <param name="encoding">给定的字符编码。</param>
/// <param name="initialFunc">给定的实例初始方法。</param>
public class JsonFilePersistenceProvider<TPersistence>(bool isWatching, string filePath, Encoding encoding, Func<TPersistence> initialFunc)
    : AbstractFilePersistenceProvider<TPersistence>(isWatching, filePath, encoding, initialFunc)
{
    /// <summary>
    /// 构造一个不需要监视文件变化的 <see cref="JsonFilePersistenceProvider{TPersistence}"/> 实例。
    /// </summary>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="encoding">给定的字符编码。</param>
    /// <param name="initialFunc">给定的实例初始方法。</param>
    public JsonFilePersistenceProvider(string filePath, Encoding encoding, Func<TPersistence> initialFunc)
        : this(isWatching: false, filePath, encoding, initialFunc)
    {
    }


    /// <summary>
    /// 加载实例。
    /// </summary>
    /// <returns>返回 <typeparamref name="TPersistence"/>。</returns>
    public override TPersistence Load()
    {
        var options = GetJsonSerializerOptions();
        var json = File.ReadAllText(FilePath, FileEncoding);

        var persistence = JsonSerializer.Deserialize<TPersistence>(json, options);

        return persistence ?? Save(InitialFunc());
    }

    /// <summary>
    /// 保存实例。
    /// </summary>
    /// <param name="persistence">给定的 <typeparamref name="TPersistence"/>。</param>
    /// <returns>返回 <typeparamref name="TPersistence"/>。</returns>
    public override TPersistence Save(TPersistence persistence)
    {
        var options = GetJsonSerializerOptions();
        var json = JsonSerializer.Serialize(persistence, options);

        File.WriteAllText(FilePath, json, FileEncoding);

        return persistence;
    }


    /// <summary>
    /// 获取 JSON 序列化选项。
    /// </summary>
    /// <returns>返回 <see cref="JsonSerializerOptions"/>。</returns>
    protected virtual JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new()
        {
            WriteIndented = true,
        };
    }

}
