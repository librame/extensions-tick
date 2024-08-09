#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义继承 <see cref="IDependency"/> 的 JSON 依赖接口。
/// </summary>
public interface IJsonDependency : IDependency
{
    /// <summary>
    /// 延迟获取序列化器选项。
    /// </summary>
    Lazy<JsonSerializerOptions> Options { get; }
}
