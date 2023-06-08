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
/// 定义一个设置值集合接口。
/// </summary>
/// <typeparam name="TSettingRoot">指定实现 <see cref="ISettingRoot"/> 的设置根类型。</typeparam>
public interface ISettingValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TSettingRoot>
    where TSettingRoot : ISettingRoot
{
    /// <summary>
    /// 获取单例值。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSettingRoot"/>。</returns>
    TSettingRoot GetSingletonValue();
}
