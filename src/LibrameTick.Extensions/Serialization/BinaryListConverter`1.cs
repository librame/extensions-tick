#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义继承 <see cref="BinaryConverter{TConverted}"/> 的泛型二进制列表转换器。
/// </summary>
/// <typeparam name="TList">指定要转换的目标列表类型。</typeparam>
/// <typeparam name="TItem">指定要转换的目标列表元素类型。</typeparam>
/// <param name="namedFunc">给定的命名方法（可选）。</param>
public class BinaryListConverter<TList, TItem>(Func<string, string>? namedFunc = null)
    : BinaryConverter<TList>
    where TList : IList<TItem>, new()
{
    private readonly Type _itemType = typeof(TItem);

    private ArgumentException ItemConverterNotFoundException(BinaryMemberInfo member)
        => new($"Cannot resolve converter for '{base.BeConvertedType.Name}' item type '{_itemType}'. The member '{member.GetRequiredType()} {member.Info.Name}' whether lacks a '{nameof(BinaryExpressionMappingAttribute)}' or '{nameof(BinaryObjectMappingAttribute)}' annotation.");


    /// <summary>
    /// 获取转换器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public override string ConverterName
        => namedFunc?.Invoke(base.ConverterName) ?? base.ConverterName;


    /// <summary>
    /// 通过读取器读取指定类型实例核心。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    /// <returns>返回字节数组。</returns>
    protected override TList ReadCore(BinaryReader reader, BinaryMemberInfo member)
    {
        var count = reader.ReadInt32();
        var list = new TList();

        var converter = member.Options.ConverterResolver.ResolveConverter(_itemType);
        if (converter is BinaryConverter<TItem> itemConverter)
        {
            for (var i = 0; i < count; i++)
            {
                var item = itemConverter.Read(reader, _itemType, member);
                list.Add(item!); // 此处为虚可空实例，实际是否为空由泛型决定
            }
        }
        else if (member.Info.IsExpressionMappingAttributeDefined())
        {
            var mappings = BinaryExpressionMapper<TItem>.GetMappings(member.Options);
            for (var j = 0; j < count; j++)
            {
                var item = Activator.CreateInstance<TItem>();
                for (var i = 0; i < mappings.Count; i++)
                {
                    mappings[i].Read(reader, item);
                }

                list.Add(item);
            }
        }
        else if (member.Info.IsObjectMappingAttributeDefined())
        {
            var mappings = BinaryObjectMapper.GetMappings(_itemType, member.Options);
            for (var j = 0; j < count; j++)
            {
                var item = Activator.CreateInstance<TItem>();
                for (var i = 0; i < mappings.Count; i++)
                {
                    mappings[i].ReadObject(reader, item!);
                }

                list.Add(item);
            }
        }
        else
        {
            throw ItemConverterNotFoundException(member);
        }

        return list;
    }

    /// <summary>
    /// 通过写入器写入指定类型实例核心。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="value">给定要写入的字节数组。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    protected override void WriteCore(BinaryWriter writer, TList value, BinaryMemberInfo member)
    {
        var count = value.Count;
        writer.Write(count);

        var converter = member.Options.ConverterResolver.ResolveConverter(_itemType);
        if (converter is BinaryConverter<TItem> itemConverter)
        {
            for (var i = 0; i < count; i++)
            {
                var item = value[i];
                itemConverter.Write(writer, _itemType, item, member);
            }
        }
        else if (member.Info.IsExpressionMappingAttributeDefined())
        {
            var mappings = BinaryExpressionMapper<TItem>.GetMappings(member.Options);
            for (var j = 0; j < value.Count; j++)
            {
                var item = value[j];
                for (var i = 0; i < mappings.Count; i++)
                {
                    mappings[i].Write(writer, item);
                }
            }
        }
        else if (member.Info.IsObjectMappingAttributeDefined())
        {
            var mappings = BinaryObjectMapper.GetMappings(_itemType, member.Options);
            for (var j = 0; j < value.Count; j++)
            {
                var item = value[j];
                for (var i = 0; i < mappings.Count; i++)
                {
                    mappings[i].WriteObject(writer, item!);
                }
            }
        }
        else
        {
            throw ItemConverterNotFoundException(member);
        }
    }

}
