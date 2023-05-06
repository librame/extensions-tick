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
/// 定义抽象实现 <see cref="ISettingProvider{TSetting}"/> 的设置提供程序。
/// </summary>
public abstract class AbstractSettingProvider<TSetting> : ISettingProvider<TSetting>
    where TSetting : ISetting
{
    /// <summary>
    /// 设置类型。
    /// </summary>
    public Type SettingType => typeof(TSetting);


    /// <summary>
    /// 存在设置。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public abstract bool Exist();


    /// <summary>
    /// 生成设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public virtual TSetting? Generate()
        => default;


    /// <summary>
    /// 加载或保存新生成的设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public virtual TSetting LoadOrSave()
    {
        if (!Exist())
        {
            var setting = Generate();

            if (setting is not null)
                return Save(setting);
        }

        return Load();
    }

    /// <summary>
    /// 加载设置。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public abstract TSetting Load();

    /// <summary>
    /// 保存设置。
    /// </summary>
    /// <param name="setting">给定的 <typeparamref name="TSetting"/>。</param>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    public abstract TSetting Save(TSetting setting);
}
