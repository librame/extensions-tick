#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Compression;
using Librame.Extensions.Cryptography;
using Librame.Extensions.Dependency;
using Librame.Extensions.Infrastructure;
using InternalBinaryChildrenInvoker = Librame.Extensions.Serialization.Internal.BinaryChildrenInvoker;
using InternalBinaryConverterResolver = Librame.Extensions.Serialization.Internal.BinaryConverterResolver;
using InternalBinaryConverters = Librame.Extensions.Serialization.Internal.BinaryConverters;
using InternalBinaryTypeResolver = Librame.Extensions.Serialization.Internal.BinaryTypeResolver;

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制序列化器选项。
/// </summary>
public class BinarySerializerOptions : StaticDefaultInitializer<BinarySerializerOptions>
{
    private MemberTypes _memberTypes = MemberTypes.Property;


    /// <summary>
    /// 构造一个默认不启用算法与压缩功能的 <see cref="BinarySerializerOptions"/> 实例。
    /// </summary>
    public BinarySerializerOptions()
        : this(enableAlgorithms: false, enableCompressions: false)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="BinarySerializerOptions"/> 实例。
    /// </summary>
    /// <param name="enableAlgorithms">启用算法功能。</param>
    /// <param name="enableCompressions">启用压缩功能。</param>
    public BinarySerializerOptions(bool enableAlgorithms, bool enableCompressions)
    {
        Algorithms = new() { IsReferenceEnabled = enableAlgorithms };
        Compressions = new() { IsReferenceEnabled = enableCompressions };
        Converters = InternalBinaryConverters.InitializeConverters();
        Encoding = DependencyRegistration.CurrentContext.Encoding;
        AutomaticInstantiationIfNull = true;

        OrderByMembersFunc = (member, index, count)
            => member.GetCustomAttribute<BinaryOrderAttribute>()?.Id ?? (index + 1);
        CascadeChildrenInvokeFunc = InternalBinaryChildrenInvoker.CascadeInvoke;

        TypeResolver = new InternalBinaryTypeResolver(this);
        ConverterResolver = new InternalBinaryConverterResolver(this);
    }

    /// <summary>
    /// 使用指定的 <see cref="BinarySerializerOptions"/> 构造一个 <see cref="BinarySerializerOptions"/> 实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="BinarySerializerOptions"/>。</param>
    public BinarySerializerOptions(BinarySerializerOptions options)
    {
        Algorithms = options.Algorithms;
        Compressions = options.Compressions;
        Converters = new(options.Converters);
        Encoding = options.Encoding;
        AutomaticInstantiationIfNull = options.AutomaticInstantiationIfNull;
        UseVersion = options.UseVersion;

        MemberType = options.MemberType;
        OrderByMembersFunc = options.OrderByMembersFunc;
        CascadeChildrenInvokeFunc = options.CascadeChildrenInvokeFunc;

        TypeResolver = options.TypeResolver;
        ConverterResolver = options.ConverterResolver;
    }


    /// <summary>
    /// 获取或设置算法选项。
    /// </summary>
    /// <remarks>
    /// 如果压缩选项不为空，则序列化时将启用加解密功能。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="AlgorithmReferenceOptions"/>。
    /// </value>
    public AlgorithmReferenceOptions Algorithms { get; set; }

    /// <summary>
    /// 获取或设置压缩选项。
    /// </summary>
    /// <remarks>
    /// 如果压缩选项不为空，则序列化时将启用压缩功能。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="CompressionOptions"/>。
    /// </value>
    public CompressionOptions Compressions { get; set; }

    /// <summary>
    /// 获取转换器列表。
    /// </summary>
    /// <value>
    /// 返回 <see cref="List{BinaryConverter}"/>。
    /// </value>
    public List<IBinaryConverter> Converters { get; init; }

    /// <summary>
    /// 获取或设置要序列化类型成员的版本。
    /// </summary>
    /// <remarks>
    /// 如果版本不为空，则序列化时将启用版本功能。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="BinarySerializerVersion"/>。
    /// </value>
    public BinarySerializerVersion? UseVersion { get; set; }

    /// <summary>
    /// 获取或设置字符编码。
    /// </summary>
    /// <value>
    /// 返回 <see cref="System.Text.Encoding"/>。
    /// </value>
    public Encoding Encoding { get; set; }

    /// <summary>
    /// 获取或设置如果自定义类型实例为 NULL 时，是否需要自动实例化。
    /// </summary>
    /// <remarks>
    /// 默认自动实例化为 NULL 的自定义类型实例。
    /// </remarks>
    /// <value>
    /// 返回是否自动实例化的布尔值。
    /// </value>
    public bool AutomaticInstantiationIfNull { get; set; }

    /// <summary>
    /// 获取或设置转换器解析器。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IBinaryConverterResolver"/>。
    /// </value>
    public IBinaryConverterResolver ConverterResolver { get; set; }

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
    public Func<MemberInfo, int, int, int> OrderByMembersFunc { get; set; }

    /// <summary>
    /// 获取或设置级联子级成员调用方法。传入参数依次为 <see cref="BinaryMemberMapping"/>、父级对象、级联子级成员信息列表。
    /// </summary>
    public Func<BinaryMemberMapping, object, List<BinaryMemberInfo>, object> CascadeChildrenInvokeFunc { get; set; }

    /// <summary>
    /// 获取或设置支持类型序列化的成员类型。
    /// </summary>
    /// <remarks>
    /// 当前仅支持序列化 <see cref="MemberTypes.Property"/>（默认）或 <see cref="MemberTypes.Field"/>，设置其他类型将抛出异常。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="MemberTypes"/>。
    /// </value>
    public MemberTypes MemberType
    {
        get => _memberTypes;
        set
        {
            if (value != MemberTypes.Property && value != MemberTypes.Field)
            {
                throw new NotSupportedException($"Resolving member type '{Enum.GetName(value)}' other than field and property is not supported.");
            }
            
            _memberTypes = value;
        }
    }

}
