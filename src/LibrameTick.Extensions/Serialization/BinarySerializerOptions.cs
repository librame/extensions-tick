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
    private readonly string _useVersionKey = $"[{nameof(UseVersion)}]";

    private MemberTypes _memberTypes = MemberTypes.Property;


    /// <summary>
    /// 构造一个 <see cref="BinarySerializerOptions"/> 默认实例。
    /// </summary>
    public BinarySerializerOptions()
    {
        Converters = InternalBinaryConverters.InitializeConverters();
        Encoding = DependencyRegistration.CurrentContext.Encoding;

        OrderByMembersFunc = (member, index, count)
            => member.GetCustomAttribute<BinaryOrderAttribute>()?.Id ?? (index + 1);
        CascadeChildrenInvokeFunc = InternalBinaryChildrenInvoker.CascadeInvoke;

        TypeResolver = new InternalBinaryTypeResolver(this);
        ConverterResolver = new InternalBinaryConverterResolver(this);
    }

    /// <summary>
    /// 使用指定的 <see cref="BinarySerializerOptions"/> 构造一个 <see cref="BinarySerializerOptions"/> 实例。
    /// </summary>
    /// <param name="options">The options.</param>
    public BinarySerializerOptions(BinarySerializerOptions options)
    {
        Converters = new(options.Converters);
        Encoding = options.Encoding;
        UseVersion = options.UseVersion;

        MemberType = options.MemberType;
        OrderByMembersFunc = options.OrderByMembersFunc;
        CascadeChildrenInvokeFunc = options.CascadeChildrenInvokeFunc;

        TypeResolver = options.TypeResolver;
        ConverterResolver = options.ConverterResolver;
    }


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


    /// <summary>
    /// 过滤不满足解析类型成员的方法。
    /// </summary>
    /// <param name="member">给定的 <see cref="MemberInfo"/>。</param>
    /// <returns>返回满足条件的布尔值。</returns>
    public bool FilterMembers(MemberInfo member)
    {
        var isUnfiltered = member.MemberType == MemberType && !member.IsIgnoreAttributeDefined();

        // 未指定序列化版本，只过滤标注忽略自定义特性的成员
        if (UseVersion is null) return isUnfiltered;

        var memberVersion = member.GetCustomAttribute<BinaryVersionAttribute>();

        // 如果未标注版本自定义特性的成员将不会被过滤
        if (memberVersion is null) return isUnfiltered;

        // 根据版本比较方法过滤不满足条件的成员
        return UseVersion.Comparison switch
        {
            BinaryVersionComparison.Equals => memberVersion.Version == UseVersion.Version,
            BinaryVersionComparison.GreaterThan => memberVersion.Version > UseVersion.Version,
            BinaryVersionComparison.GreaterThanOrEquals => memberVersion.Version >= UseVersion.Version,
            BinaryVersionComparison.LessThan => memberVersion.Version < UseVersion.Version,
            BinaryVersionComparison.LessThanOrEquals => memberVersion.Version <= UseVersion.Version,
            _ => isUnfiltered
        };
    }


    /// <summary>
    /// 读取版本信息。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <returns>返回 <see cref="BinarySerializerVersion"/>。</returns>
    public BinarySerializerVersion? ReadVersion(BinaryReader reader)
        => TryReadVersion(reader, out var version) ? version : null;

    /// <summary>
    /// 写入版本信息。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    public void WriteVersion(BinaryWriter writer)
    {
        writer.Write(_useVersionKey);
        writer.Write(UseVersion is not null);

        if (UseVersion is not null)
        {
            writer.Write(UseVersion.Version);
            writer.Write(Enum.GetName(UseVersion.Comparison) ?? UseVersion.Comparison.ToString());
        }
    }

    /// <summary>
    /// 尝试读取版本信息。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="version">输出可能存在的 <see cref="BinarySerializerVersion"/>。</param>
    /// <returns>返回是否成功读取的布尔值。</returns>
    public bool TryReadVersion(BinaryReader reader,
        [NotNullWhen(true)] out BinarySerializerVersion? version)
    {
        string? str;
        bool has;

        try
        {
            // Try read version key
            str = reader.ReadString();

            // Try read version is not null
            has = reader.ReadBoolean();
        }
        catch
        {
            str = null;
            has = false;
        }

        if (_useVersionKey.Equals(str, StringComparison.Ordinal) && has)
        {
            var number = reader.ReadDouble();
            var comparison = Enum.Parse<BinaryVersionComparison>(reader.ReadString());

            version = new(number, comparison);
            return true;
        }

        version = null;
        return false;
    }

}
