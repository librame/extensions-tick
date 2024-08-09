#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义类型命名键。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="TypeNamedKey"/>。
/// </remarks>
/// <param name="Typed">给定的类型。</param>
/// <param name="Named">给定的命名（可选）。</param>
public record class TypeNamedKey(Type Typed, string? Named = null)
{
    /// <summary>
    /// 字符串连接符。
    /// </summary>
    public string? Connector { get; private set; }


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (string.IsNullOrEmpty(Named))
        {
            return Typed.GetFriendlyName();
        }

        return $"{Typed.GetFriendlyName()}{Connector}{Named}";
    }


    /// <summary>
    /// 创建泛型类型命名键。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <see cref="TypeNamedKey"/>。</returns>
    public static TypeNamedKey Create<T>(string? named = null)
        => new(typeof(T), named);

    /// <summary>
    /// 创建带连接符的泛型类型命名键。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="connector">给定的连接符。</param>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <see cref="TypeNamedKey"/>。</returns>
    public static TypeNamedKey CreateWithConnector<T>(string connector, string? named = null)
        => new(typeof(T), named) { Connector = connector };

    /// <summary>
    /// 创建带连接符的类型命名键。
    /// </summary>
    /// <param name="type">给定的类型。</param>
    /// <param name="connector">给定的连接符。</param>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <see cref="TypeNamedKey"/>。</returns>
    public static TypeNamedKey CreateWithConnector(Type type, string connector, string? named = null)
        => new(type, named) { Connector = connector };

}
