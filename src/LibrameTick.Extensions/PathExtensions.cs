#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Path"/> 静态扩展。
/// </summary>
public static class PathExtensions
{
    /// <summary>
    /// <see cref="Directory.GetCurrentDirectory()"/> 当前目录。
    /// </summary>
    public static readonly string CurrentDirectory
        = Directory.GetCurrentDirectory(); //Environment.ProcessPath;

    /// <summary>
    /// 除去开发相对路径部分的 <see cref="Directory.GetCurrentDirectory()"/> 当前目录。
    /// </summary>
    public static string CurrentDirectoryWithoutDevelopmentRelativeSubpath
        => CurrentDirectory.TrimDevelopmentRelativeSubpath();


    /// <summary>
    /// 创建目录。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    public static DirectoryInfo CreateDirectory(this string path)
        => Directory.CreateDirectory(path);

    /// <summary>
    /// 确保目录已存在。
    /// </summary>
    /// <param name="directory">给定的目录。</param>
    /// <returns>返回目录字符串。</returns>
    public static string EnsureDirectory(string directory)
    {
        directory.CreateDirectory();
        return directory;
    }


    /// <summary>
    /// 删除目录。
    /// </summary>
    /// <param name="path">给定的目录路径。</param>
    public static void DirectoryDelete(this string path)
        => Directory.Delete(path);

    /// <summary>
    /// 目录是否存在。
    /// </summary>
    /// <param name="path">给定的目录路径。</param>
    /// <returns>返回布尔值。</returns>
    public static bool DirectoryExists(this string path)
        => Directory.Exists(path);


    /// <summary>
    /// 删除文件。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    public static void FileDelete(this string path)
        => File.Delete(path);

    /// <summary>
    /// 文件是否存在。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <returns>返回布尔值。</returns>
    public static bool FileExists(this string path)
        => File.Exists(path);


    /// <summary>
    /// 设置基础路径。
    /// </summary>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <param name="basePath">给定的基础路径（可选；默认使用没有开发相对子路径的当前目录）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string SetBasePath(this string relativePath, string? basePath = null)
    {
        if (string.IsNullOrEmpty(basePath))
            basePath = CurrentDirectoryWithoutDevelopmentRelativeSubpath;

        if (relativePath.StartsWith("./")
            || relativePath.StartsWith(".\\")
            || !relativePath.StartsWith(basePath))
        {
            return Path.Combine(basePath, relativePath);
        }

        return relativePath;
    }


    #region Combine

    /// <summary>
    /// 合并路径。
    /// </summary>
    /// <param name="basePath">给定的基础路径。</param>
    /// <param name="relativePath">给定的相对路径。</param>
    /// <returns>返回路径字符串。</returns>
    public static string CombinePath(this string basePath, string relativePath)
        => Path.Combine(basePath, relativePath);


    /// <summary>
    /// 将文件夹名称集合组合为相对子路径（如：“folder1、folder2 => folder1\folder2\ 或 \folder1\folder2”）。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="folderNames"/> has invalid path chars.
    /// </exception>
    /// <param name="folderNames">给定的文件夹名称集合。</param>
    /// <param name="pathSeparatorEscaping">将路径分隔符转义（可选；默认不转义）。</param>
    /// <param name="pathSeparatorForward">将路径分隔符前置（可选；默认后置）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string CombineRelativeSubpath(this IEnumerable<string> folderNames,
        bool pathSeparatorEscaping = false, bool pathSeparatorForward = false)
        => folderNames.Select(value =>
        {
            return value.CombineRelativeSubpath(pathSeparatorEscaping, pathSeparatorForward);
        })
        .JoinString();

    /// <summary>
    /// 将文件夹名称组合为相对子路径（如：“folder => folder\ 或 \folder”）。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="folderName"/> has invalid path chars.
    /// </exception>
    /// <param name="folderName">给定的文件夹名称。</param>
    /// <param name="pathSeparatorEscaping">将路径分隔符转义（可选；默认不转义）。</param>
    /// <param name="pathSeparatorForward">将路径分隔符前置（可选；默认后置）。</param>
    /// <returns>返回路径字符串。</returns>
    public static string CombineRelativeSubpath(this string folderName,
        bool pathSeparatorEscaping = false, bool pathSeparatorForward = false)
    {
        if (folderName.HasInvalidPathChars())
            throw new InvalidOperationException($"'{folderName}' has invalid path chars.");

        var separator = Path.DirectorySeparatorChar.ToString();
        if (pathSeparatorEscaping)
            separator = Regex.Escape(separator);

        if (pathSeparatorForward)
            return $"{separator}{folderName}";

        return $"{folderName}{separator}";
    }

    #endregion


    #region BinaryFileRead & BinaryFileWrite

    /// <summary>
    /// 二进制文件读取（此方法的类型仅支持包含常见的值类型和字符串类型成员）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <param name="flags">给定要写入成员的 <see cref="BindingFlags"/>（可选；默认包含静态在内的所有字段和属性成员集合）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public static T BinaryFileRead<T>(this string path, Encoding? encoding = null, BindingFlags? flags = null)
        => (T)path.BinaryFileRead(typeof(T), encoding, flags);

    /// <summary>
    /// 二进制文件读取（此方法的对象仅支持包含常见的值类型和字符串类型成员）。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="objType">给定要读取的对象类型。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <param name="flags">给定要写入成员的 <see cref="BindingFlags"/>（可选；默认包含静态在内的所有字段和属性成员集合）。</param>
    /// <returns>返回对象。</returns>
    public static object BinaryFileRead(this string path, Type objType, Encoding? encoding = null, BindingFlags? flags = null)
    {
        var obj = objType.NewByExpression();

        var fields = flags is null ? objType.GetAllFieldsAndPropertiesWithStatic() : objType.GetFields(flags.Value);

        using (var input = File.Open(path, FileMode.Open))
        using (var reader = new BinaryReader(input, encoding ?? EncodingExtensions.UTF8Encoding))
        {
            foreach (var field in fields)
            {
                field.SetValue(obj, ReadValue(reader, field.FieldType));
            }
        }

        return obj;

        object ReadValue(BinaryReader reader, Type type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    return reader.ReadBoolean();

                case "System.Byte":
                    return reader.ReadByte();

                //case "System.Byte[]":
                //    return reader.ReadBytes(?);

                case "System.Char":
                    return reader.ReadChar();

                //case "System.Char[]":
                //    return reader.ReadChars(?);

                case "System.Decimal":
                    return reader.ReadDecimal();

                case "System.Double":
                    return reader.ReadDouble();

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
                    throw new NotSupportedException($"The current type '{type}' of reading is not supported.");
            }
        }
    }


    /// <summary>
    /// 二进制文件写入（此方法的对象仅支持包含常见的值类型和字符串类型成员）。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="obj">给定要写入文件的对象。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <param name="flags">给定要写入成员的 <see cref="BindingFlags"/>（可选；默认包含静态在内的所有字段和属性成员集合）。</param>
    public static void BinaryFileWrite(this string path, object obj, Encoding? encoding = null, BindingFlags? flags = null)
    {
        var objType = obj.GetType();

        var fields = flags is null ? objType.GetAllFieldsAndPropertiesWithStatic() : objType.GetFields(flags.Value);

        using (var output = File.Open(path, FileMode.Create))
        using (var writer = new BinaryWriter(output, encoding ?? EncodingExtensions.UTF8Encoding))
        {
            foreach (var field in fields)
            {
                WriteValue(writer, field.FieldType, field.GetValue(obj));
            }
        }

        void WriteValue(BinaryWriter writer, Type type, object? value)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    writer.Write((bool)value!);
                    break;

                case "System.Byte":
                    writer.Write((byte)value!);
                    break;

                //case "System.Byte[]":
                //    writer.Write((byte[])value);
                //    break;

                case "System.Char":
                    writer.Write((char)value!);
                    break;

                //case "System.Char[]":
                //    writer.Write((char[])value);
                //    break;

                case "System.Decimal":
                    writer.Write((decimal)value!);
                    break;

                case "System.Double":
                    writer.Write((double)value!);
                    break;

                case "System.Half":
                    writer.Write((Half)value!);
                    break;

                case "System.Int16":
                    writer.Write((short)value!);
                    break;

                case "System.Int32":
                    writer.Write((int)value!);
                    break;

                case "System.Int64":
                    writer.Write((long)value!);
                    break;

                case "System.SByte":
                    writer.Write((sbyte)value!);
                    break;

                case "System.Single":
                    writer.Write((float)value!);
                    break;

                case "System.String":
                    writer.Write(value?.ToString() ?? string.Empty);
                    break;

                case "System.UInt16":
                    writer.Write((ushort)value!);
                    break;

                case "System.UInt32":
                    writer.Write((uint)value!);
                    break;

                case "System.UInt64":
                    writer.Write((ulong)value!);
                    break;

                default:
                    throw new NotSupportedException($"The current type '{type}' of writing is not supported.");
            }
        }
    }

    #endregion


    #region FileRead & FileWrite

    /// <summary>
    /// 文件读取。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FileRead(this string path)
        => path.FileRead(0L);

    /// <summary>
    /// 文件读取。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="fileOffset">给定的读取偏移量。</param>
    /// <returns>返回字节数组。</returns>
    public static byte[] FileRead(this string path, long fileOffset)
    {
        using (var handle = File.OpenHandle(path))
        {
            var length = RandomAccess.GetLength(handle);
            var buffer = new byte[length - fileOffset];

            var readLength = RandomAccess.Read(handle, buffer, fileOffset);
            return buffer;
        }
    }

    /// <summary>
    /// 文件写入。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="buffer">给定的字节数组。</param>
    public static void FileWrite(this string path, byte[] buffer)
        => path.FileWrite(buffer, 0L);

    /// <summary>
    /// 文件写入。
    /// </summary>
    /// <param name="path">给定的文件路径。</param>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="fileOffset">给定的读取偏移量。</param>
    public static void FileWrite(this string path, byte[] buffer, long fileOffset)
    {
        using (var handle = File.OpenHandle(path, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            RandomAccess.Write(handle, buffer, fileOffset);
        }
    }

    #endregion


    #region InvalidPathChars

    /// <summary>
    /// 含无效的路径字符集合。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <returns>返回布尔值。</returns>
    public static bool HasInvalidPathChars(this string path)
        => path.HasInvalidChars(Path.GetInvalidPathChars());

    /// <summary>
    /// 含无效的文件名称字符集合。
    /// </summary>
    /// <param name="fileName">给定的文件名。</param>
    /// <returns>返回布尔值。</returns>
    public static bool HasInvalidFileNameChars(this string fileName)
        => fileName.HasInvalidChars(Path.GetInvalidFileNameChars());

    #endregion


    #region TrimRelativeSubpath

    private static readonly string _developmentRelativeSubpath
        = InitDevelopmentRelativeSubpath();

    private static string InitDevelopmentRelativeSubpath()
    {
        var separator = Regex.Escape(Path.DirectorySeparatorChar.ToString());

        var sb = new StringBuilder();

        // 采用路径分隔符前置方案
        sb.Append($"({separator}bin|{separator}obj)");
        sb.Append($"({separator}x86|{separator}x64)?");
        sb.Append($"({separator}Debug|{separator}Release)");

        return sb.ToString();
    }

    /// <summary>
    /// 修剪路径中存在的开发相对路径部分（如：prefix\bin\[x64\]Debug => prefix）。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <returns>返回修剪后的路径字符串。</returns>
    public static string TrimDevelopmentRelativeSubpath(this string path)
    {
        var regex = new Regex(_developmentRelativeSubpath);
        if (regex.IsMatch(path))
        {
            var match = regex.Match(path);
            return path.Substring(0, match.Index);
        }

        return path;
    }

    #endregion

}
