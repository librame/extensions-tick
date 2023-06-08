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
/// 定义一个设置提供程序接口。
/// </summary>
/// <typeparam name="TSettingRoot">指定实现 <see cref="ISettingRoot"/> 的设置根类型。</typeparam>
public interface ISettingProvider<TSettingRoot>
    where TSettingRoot : ISettingRoot
{
    /// <summary>
    /// 设置根类型。
    /// </summary>
    Type SettingRootType { get; }


    /// <summary>
    /// 存在设置。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    bool Exist();


    /// <summary>
    /// 生成设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    TSettingRoot Generate();


    /// <summary>
    /// 加载或保存新生成的设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    TSettingRoot LoadOrSave();

    /// <summary>
    /// 加载设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    TSettingRoot Load();


    /// <summary>
    /// 保存设置。
    /// </summary>
    /// <param name="settingRoot">给定的 <typeparamref name="TSettingRoot"/>。</param>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    TSettingRoot Save(TSettingRoot settingRoot);
}
