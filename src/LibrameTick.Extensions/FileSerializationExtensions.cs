#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure.Dependency;

namespace Librame.Extensions;

/// <summary>
/// 定义用于文件序列化的静态扩展。
/// </summary>
public static class FileSerializationExtensions
{
    /// <summary>
    /// 当前扩展依赖。
    /// </summary>
    public static IDependencyContext Dependency
        => DependencyRegistration.CurrentContext;


    #region Binary

    /// <summary>
    /// 反序列化二进制文件（支持自定义反序列化字段类型）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="obj">给定的对象实例（可选；默认使用对象类型创建实例）。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="flags">给定要写入成员的 <see cref="BindingFlags"/>（可选；默认包含静态在内的所有字段和属性成员集合）。</param>
    /// <param name="customReadFuncs">给定的自定义字段类型读取方法字典集合（可选）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T DeserializeBinaryFile<T>(this string filePath, object? obj = null,
        Encoding? encoding = null, BindingFlags? flags = null,
        Dictionary<Type, Func<BinaryReader, object, object>>? customReadFuncs = null)
        => (T)filePath.DeserializeBinaryFile(typeof(T), obj, encoding, flags, customReadFuncs);

    /// <summary>
    /// 反序列化二进制文件（支持自定义反序列化字段类型）。
    /// </summary>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="objType">给定要读取的对象类型。</param>
    /// <param name="obj">给定的对象实例（可选；默认使用对象类型创建实例）。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="flags">给定要写入成员的 <see cref="BindingFlags"/>（可选；默认包含静态在内的所有字段和属性成员集合）。</param>
    /// <param name="customReadFuncs">给定的自定义字段类型读取方法字典集合（可选）。</param>
    /// <returns>返回对象。</returns>
    public static object DeserializeBinaryFile(this string filePath, Type objType,
        object? obj = null, Encoding? encoding = null, BindingFlags? flags = null,
        Dictionary<Type, Func<BinaryReader, object, object>>? customReadFuncs = null)
    {
        using (var input = File.Open(filePath, FileMode.Open))
        using (var reader = new BinaryReader(input, encoding ?? Dependency.Encoding))
        {
            return DeserializeBinaryCore(reader, objType, obj, flags, customReadFuncs);
        }
    }

    private static object DeserializeBinaryCore(BinaryReader reader,
        Type objType, object? obj = null, BindingFlags? flags = null,
        Dictionary<Type, Func<BinaryReader, object, object>>? customReadFuncs = null)
    {
        obj ??= Activator.CreateInstance(objType);
        ArgumentNullException.ThrowIfNull(obj);

        var fields = flags is null
            ? objType.GetFields(TypeExtensions.AllMemberFlagsWithStatic)
            : objType.GetFields(flags.Value);

        foreach (var field in fields)
        {
            field.SetValue(obj, ReadValue(reader, field.FieldType, obj));
        }

        return obj;

        object ReadValue(BinaryReader reader, Type fieldType, object obj)
        {
            switch (fieldType.FullName)
            {
                case "System.Boolean":
                    return reader.ReadBoolean();

                case "System.Byte":
                    return reader.ReadByte();

                case "System.Byte[]":
                    return Convert.FromBase64String(reader.ReadString());

                case "System.Char":
                    return reader.ReadChar();

                case "System.Char[]":
                    return reader.ReadString().ToCharArray();

                case "System.Decimal":
                    return reader.ReadDecimal();

                case "System.Double":
                    return reader.ReadDouble();

                case "System.Guid":
                    return Guid.Parse(reader.ReadString());

                case "System.Half":
                    return reader.ReadHalf();

                case "System.Int16":
                    return reader.ReadInt16();

                case "System.Int32":
                    return reader.ReadInt32();

                case "System.Int64":
                    return reader.ReadInt64();

                case "System.SByte":
                    return reader.ReadSByte();

                case "System.Single":
                    return reader.ReadSingle();

                case "System.String":
                    return reader.ReadString();

                case "System.UInt16":
                    return reader.ReadUInt16();

                case "System.UInt32":
                    return reader.ReadUInt32();

                case "System.UInt64":
                    return reader.ReadUInt64();

                default:
                    if (customReadFuncs is not null && customReadFuncs.TryGetValue(fieldType, out var func))
                        return func.Invoke(reader, obj);

                    return DeserializeBinaryCore(reader, objType, flags);
            }
        }
    }


    /// <summary>
    /// 序列化二进制文件（支持自定义序列化字段类型）。
    /// </summary>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="obj">给定要写入文件的对象。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="IDependencyContext.Encoding"/>）。</param>
    /// <param name="flags">给定要写入成员的 <see cref="BindingFlags"/>（可选；默认包含静态在内的所有字段和属性成员集合）。</param>
    /// <param name="customWriteActions">给定的自定义字段类型写入动作字典集合（可选）。</param>
    public static void SerializeBinaryFile(this string filePath, object obj,
        Encoding? encoding = null, BindingFlags? flags = null,
        Dictionary<Type, Action<BinaryWriter, object?>>? customWriteActions = null)
    {
        using (var output = File.Open(filePath, FileMode.Create))
        using (var writer = new BinaryWriter(output, encoding ?? Dependency.Encoding))
        {
            SerializeBinaryCore(writer, obj, objType: null, flags, customWriteActions);
        }
    }

    private static void SerializeBinaryCore(BinaryWriter writer,
        object obj, Type? objType = null, BindingFlags? flags = null,
        Dictionary<Type, Action<BinaryWriter, object?>>? customWriteActions = null)
    {
        if (objType is null)
            objType = obj.GetType();

        var fields = flags is null
            ? objType.GetFields(TypeExtensions.AllMemberFlagsWithStatic)
            : objType.GetFields(flags.Value);

        foreach (var field in fields)
        {
            WriteValue(writer, field.FieldType, field.GetValue(obj));
        }

        void WriteValue(BinaryWriter writer, Type fieldType, object? fieldValue)
        {
            switch (fieldType.FullName)
            {
                case "System.Boolean":
                    writer.Write((bool)fieldValue!);
                    break;

                case "System.Byte":
                    writer.Write((byte)fieldValue!);
                    break;

                case "System.Byte[]":
                    var bytes = (byte[]?)fieldValue;
                    if (bytes is not null && bytes.Length > 0)
                        writer.Write(Convert.ToBase64String(bytes));
                    break;

                case "System.Char":
                    writer.Write((char)fieldValue!);
                    break;

                case "System.Char[]":
                    writer.Write(new string((char[]?)fieldValue));
                    break;

                case "System.Decimal":
                    writer.Write((decimal)fieldValue!);
                    break;

                case "System.Double":
                    writer.Write((double)fieldValue!);
                    break;

                case "System.Guid":
                    writer.Write(((Guid)fieldValue!).ToString());
                    break;

                case "System.Half":
                    writer.Write((Half)fieldValue!);
                    break;

                case "System.Int16":
                    writer.Write((short)fieldValue!);
                    break;

                case "System.Int32":
                    writer.Write((int)fieldValue!);
                    break;

                case "System.Int64":
                    writer.Write((long)fieldValue!);
                    break;

                case "System.SByte":
                    writer.Write((sbyte)fieldValue!);
                    break;

                case "System.Single":
                    writer.Write((float)fieldValue!);
                    break;

                case "System.String":
                    writer.Write(fieldValue?.ToString() ?? string.Empty);
                    break;

                case "System.UInt16":
                    writer.Write((ushort)fieldValue!);
                    break;

                case "System.UInt32":
                    writer.Write((uint)fieldValue!);
                    break;

                case "System.UInt64":
                    writer.Write((ulong)fieldValue!);
                    break;

                default:
                    if (customWriteActions is not null && customWriteActions.TryGetValue(fieldType, out var action))
                    {
                        action.Invoke(writer, fieldValue);
                        break;
                    }

                    if (fieldValue is not null)
                    {
                        SerializeBinaryCore(writer, fieldValue, fieldType, flags);
                        break;
                    }
                    
                    throw new NotSupportedException($"The current type '{fieldType}' of writing is not supported.");
            }
        }
    }

    #endregion

}
