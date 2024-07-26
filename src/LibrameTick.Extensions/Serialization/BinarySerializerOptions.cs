#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;
using Librame.Extensions.Infrastructure.Dependency;
using InternalBinaryConverters = Librame.Extensions.Serialization.Internal.BinaryConverters;
using InternalBinaryTypeResolver = Librame.Extensions.Serialization.Internal.BinaryTypeResolver;

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制序列化器选项。
/// </summary>
public class BinarySerializerOptions : StaticDefaultInitializer<BinarySerializerOptions>
{
    /// <summary>
    /// 构造一个 <see cref="BinarySerializerOptions"/> 默认实例。
    /// </summary>
    public BinarySerializerOptions()
    {
        Converters = InternalBinaryConverters.InitializeConverters();
        Encoding = DependencyRegistration.CurrentContext.Encoding;
        MemberType = MemberTypes.Property;
        TypeResolver = new InternalBinaryTypeResolver();
        OrderByMembers = (member, index, count)
            => member.GetCustomAttribute<BinaryOrderAttribute>()?.Id ?? (index + 1);
    }

    /// <summary>
    /// 使用指定的 <see cref="BinarySerializerOptions"/> 构造一个 <see cref="BinarySerializerOptions"/> 实例。
    /// </summary>
    /// <param name="options">The options.</param>
    public BinarySerializerOptions(BinarySerializerOptions options)
    {
        Converters = new List<IBinaryConverter>(options.Converters);
        Encoding = options.Encoding;
        MemberType = MemberTypes.Property;
        TypeResolver = options.TypeResolver;
        OrderByMembers = options.OrderByMembers;
    }


    /// <summary>
    /// 获取转换器列表。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IList{BinaryConverter}"/>。
    /// </value>
    public List<IBinaryConverter> Converters { get; init; }

    /// <summary>
    /// 获取或设置字符编码。
    /// </summary>
    /// <value>
    /// 返回 <see cref="System.Text.Encoding"/>。
    /// </value>
    public Encoding Encoding { get; set; }

    /// <summary>
    /// 获取或设置支持类型序列化的成员类型。
    /// </summary>
    /// <remarks>
    /// 当前仅支持序列化 <see cref="MemberTypes.Property"/>（默认）或 <see cref="MemberTypes.Field"/>，设置其他类型将抛出异常。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="MemberTypes"/>。
    /// </value>
    public MemberTypes MemberType { get; set; }

    /// <summary>
    /// 获取或设置类型解析器。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IBinaryTypeResolver"/>。
    /// </value>
    public IBinaryTypeResolver TypeResolver { get; set; }

    /// <summary>
    /// 获取或设置按成员排序。传入参数依次为成员信息、成员当前索引、成员总数。
    /// </summary>
    /// <value>
    /// 返回排序方法。
    /// </value>
    public Func<MemberInfo, int, int, int> OrderByMembers { get; set; }


    /// <summary>
    /// 获取指定类型的二进制转换器。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="named">给定的 <see cref="BinaryConverterAttribute"/>。</param>
    /// <returns>返回 <see cref="BinaryConverter{T}"/>。</returns>
    public BinaryConverter<T>? GetBinaryConverter<T>(BinaryConverterAttribute? named)
    {
        var typeToConvert = typeof(T);

        if (named?.Type is not null)
        {
            typeToConvert = named.Type;
        }

        return (BinaryConverter<T>?)GetBinaryConverter(typeToConvert, named?.Name);
    }

    /// <summary>
    /// 获取指定类型的二进制转换器。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="name">给定的转换器名称（可选）。</param>
    /// <returns>返回 <see cref="BinaryConverter{T}"/>。</returns>
    public BinaryConverter<T>? GetBinaryConverter<T>(string? name = null)
        => (BinaryConverter<T>?)GetBinaryConverter(typeof(T), name);

    /// <summary>
    /// 获取指定类型的二进制转换器。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <param name="named">给定的 <see cref="BinaryConverterAttribute"/>。</param>
    /// <returns>返回 <see cref="IBinaryConverter"/>。</returns>
    public IBinaryConverter? GetBinaryConverter(Type typeToConvert, BinaryConverterAttribute? named)
    {
        if (named?.Type is not null)
        {
            typeToConvert = named.Type;
        }

        return GetBinaryConverter(typeToConvert, named?.Name);
    }

    /// <summary>
    /// 获取指定类型的二进制转换器。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <param name="name">给定的转换器名称（可选）。</param>
    /// <returns>返回 <see cref="IBinaryConverter"/>。</returns>
    public IBinaryConverter? GetBinaryConverter(Type typeToConvert, string? name = null)
    {
        var converters = Converters.Where(c => c.TargetType == typeToConvert);
        if (name is not null)
        {
            converters = converters.Where(c => c.CurrentName == name);
        }

        return converters.FirstOrDefault();
    }

}
