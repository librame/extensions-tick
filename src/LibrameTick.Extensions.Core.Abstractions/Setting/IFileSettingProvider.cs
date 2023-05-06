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
/// 定义一个继承 <see cref="ISettingProvider{TSetting}"/> 的文件型设置提供程序接口。
/// </summary>
public interface IFileSettingProvider<TSetting> : ISettingProvider<TSetting>
    where TSetting : ISetting
{
    /// <summary>
    /// 文件路径。
    /// </summary>
    string FilePath { get; }
}
