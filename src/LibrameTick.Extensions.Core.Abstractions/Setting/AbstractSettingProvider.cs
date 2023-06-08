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
/// 定义抽象实现 <see cref="ISettingProvider{TSettingRoot}"/> 的设置提供程序。
/// </summary>
public abstract class AbstractSettingProvider<TSettingRoot> : ISettingProvider<TSettingRoot>
    where TSettingRoot : ISettingRoot
{
    /// <summary>
    /// 设置类型。
    /// </summary>
    public Type SettingRootType => typeof(TSettingRoot);


    /// <summary>
    /// 存在设置。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public abstract bool Exist();


    /// <summary>
    /// 生成设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    public abstract TSettingRoot Generate();


    /// <summary>
    /// 加载或保存新生成的设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    public virtual TSettingRoot LoadOrSave()
    {
        if (!Exist())
        {
            var setting = Generate();

            return Save(setting);
        }

        return Load();
    }

    /// <summary>
    /// 加载设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    public abstract TSettingRoot Load();

    /// <summary>
    /// 保存设置。
    /// </summary>
    /// <param name="settingRoot">给定的 <typeparamref name="TSettingRoot"/>。</param>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    public abstract TSettingRoot Save(TSettingRoot settingRoot);
}
