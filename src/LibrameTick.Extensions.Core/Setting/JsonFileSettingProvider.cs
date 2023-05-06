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
/// 定义继承 <see cref="AbstractFileSettingProvider{TSetting}"/> 的 JSON 文件型设置提供程序。
/// </summary>
public class JsonFileSettingProvider<TSetting> : AbstractFileSettingProvider<TSetting>
    where TSetting : ISetting
{
    /// <summary>
    /// 构造一个 <see cref="JsonFileSettingProvider{TSetting}"/>。
    /// </summary>
    /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
    /// <param name="filePath">给定的文件路径。</param>
    public JsonFileSettingProvider(ILoggerFactory loggerFactory, string filePath)
        : base(loggerFactory, filePath)
    {
    }


    /// <summary>
    /// 加载设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public override TSetting Load()
    {
        var setting = FilePath.DeserializeJsonFile<TSetting>();
        if (setting is null)
            throw new NotSupportedException($"Unsupported {nameof(TSetting)} file format.");

        return setting;
    }

    /// <summary>
    /// 保存设置。
    /// </summary>
    /// <param name="setting">给定的 <typeparamref name="TSetting"/>。</param>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public override TSetting Save(TSetting setting)
    {
        FilePath.SerializeJsonFile(setting);

        return setting;
    }

}
