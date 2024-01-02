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
/// <typeparam name="TSetting">指定的设置类型。</typeparam>
public interface ISettingProvider<TSetting>
    where TSetting : class, ISetting
{
    /// <summary>
    /// 当前设置。
    /// </summary>
    TSetting CurrentSetting { get; }


    /// <summary>
    /// 保存变化（默认直接返回当前设置）。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    TSetting SaveChanges();
}
