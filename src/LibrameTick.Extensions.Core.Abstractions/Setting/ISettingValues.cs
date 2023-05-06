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
/// <typeparam name="TSetting">指定实现 <see cref="ISetting"/> 的设置类型。</typeparam>
public interface ISettingValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TSetting>
    where TSetting : ISetting
{
    /// <summary>
    /// 获取单例值。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSetting"/>。</returns>
    TSetting GetSingletonValue();
}
