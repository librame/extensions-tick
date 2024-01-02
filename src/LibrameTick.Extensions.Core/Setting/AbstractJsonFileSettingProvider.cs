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

///// <summary>
///// 定义抽象实现 <see cref="AbstractFileSettingProvider{TSettingRoot}"/> 的 JSON 文件型设置提供程序。
///// </summary>
//public abstract class AbstractJsonFileSettingProvider<TSettingRoot> : AbstractFileSettingProvider<TSettingRoot>
//    where TSettingRoot : ISettingRoot
//{
//    /// <summary>
//    /// 构造一个 <see cref="AbstractJsonFileSettingProvider{TSettingRoot}"/>。
//    /// </summary>
//    /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
//    /// <param name="filePath">给定的文件路径。</param>
//    protected AbstractJsonFileSettingProvider(ILoggerFactory loggerFactory, string filePath)
//        : base(loggerFactory, filePath)
//    {
//    }


//    /// <summary>
//    /// 加载设置。
//    /// </summary>
//    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
//    public override TSettingRoot Load()
//    {
//        var setting = FilePath.DeserializeJsonFile<TSettingRoot>();
//        if (setting is null)
//            throw new NotSupportedException($"Unsupported {nameof(TSettingRoot)} file format.");

//        return setting;
//    }

//    /// <summary>
//    /// 保存设置。
//    /// </summary>
//    /// <param name="root">给定的 <typeparamref name="TSettingRoot"/>。</param>
//    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
//    public override TSettingRoot Save(TSettingRoot root)
//    {
//        FilePath.SerializeJsonFile(root);

//        return root;
//    }

//}
