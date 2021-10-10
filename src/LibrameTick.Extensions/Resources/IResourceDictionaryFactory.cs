#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Resources;

/// <summary>
/// 定义一个资源字典工厂接口。
/// </summary>
public interface IResourceDictionaryFactory
{
    /// <summary>
    /// 当前文化信息。
    /// </summary>
    CultureInfo? CurrentCulture { get; }


    /// <summary>
    /// 创建指定文化信息的资源字典。
    /// </summary>
    /// <param name="culture">给定的 <see cref="CultureInfo"/>（可选）。</param>
    /// <returns>返回 <see cref="IResourceDictionary"/>。</returns>
    IResourceDictionary Create(CultureInfo? culture = null);
}
