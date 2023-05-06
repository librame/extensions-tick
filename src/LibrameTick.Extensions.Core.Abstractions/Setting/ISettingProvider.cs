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
/// <typeparam name="TSetting">指定实现 <see cref="ISetting"/> 的设置类型。</typeparam>
public interface ISettingProvider<TSetting>
    where TSetting : ISetting
{
    /// <summary>
    /// 设置类型。
    /// </summary>
    Type SettingType { get; }


    /// <summary>
    /// 存在设置。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    bool Exist();


    /// <summary>
    /// 生成设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    TSetting? Generate();


    /// <summary>
    /// 加载或保存新生成的设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    TSetting LoadOrSave();

    /// <summary>
    /// 加载设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    TSetting Load();

    /// <summary>
    /// 保存设置。
    /// </summary>
    /// <param name="setting">给定的 <typeparamref name="TSetting"/>。</param>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    TSetting Save(TSetting setting);
}
