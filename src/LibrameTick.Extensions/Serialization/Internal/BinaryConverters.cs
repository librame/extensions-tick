#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization.Internal;

internal static class BinaryConverters
{

    private static string InternalConverterNamed(string typeName)
        => $"Native{typeName}";


    public static List<IBinaryConverter> InitializeConverters()
    {
        // 如果使用泛型表达式，需严格要求类型与可空类型
        var converters = new List<IBinaryConverter>
        {
            // byte[]
            new BinaryArrayConverter<byte[]>(static (r, m, a) => r.ReadBytes(a.Count),
                static (w, v, m, a) => w.Write(v, a.Index, a.Count), InternalConverterNamed),

            new BinaryArrayConverter<byte[]?>(static (r, m, a) => r.ReadBytes(a.Count),
                static (w, v, m, a) => w.Write(v ?? [], a.Index, a.Count), InternalConverterNamed),

            // char[]
            new BinaryArrayConverter<char[]>(static (r, m, a) => r.ReadChars(a.Count),
                static (w, v, m, a) => w.Write(v, a.Index, a.Count), InternalConverterNamed),

            new BinaryArrayConverter<char[]?>(static (r, m, a) => r.ReadChars(a.Count),
                static (w, v, m, a) => w.Write(v ?? [], a.Index, a.Count), InternalConverterNamed),

            // Guid
            new BinaryConverterFactory<Guid>(static (r, m) => new Guid(r.ReadBytes(16)),
                static (w, v, m) => w.Write( v.ToByteArray() ), InternalConverterNamed),

            new BinaryConverterFactory<Guid?>(static (r, m) => new Guid(r.ReadBytes(16)),
                static (w, v, m) => w.Write( (v ?? Guid.Empty).ToByteArray() ), InternalConverterNamed),

            // DateTime
            new BinaryConverterFactory<DateTime>(static (r, m) => new DateTime(r.ReadInt64()),
                static (w, v, m) => w.Write(v.Ticks), InternalConverterNamed),

            new BinaryConverterFactory<DateTime?>(static (r, m) => new DateTime(r.ReadInt64()),
                static (w, v, m) => w.Write( (v ?? DateTime.MinValue).Ticks ), InternalConverterNamed),
            
            // DateTimeOffset
            new BinaryConverterFactory<DateTimeOffset>(static (r, m) => new DateTimeOffset(r.ReadInt64(), new TimeSpan(r.ReadInt64())),
                static (w, v, m) =>
                {
                    w.Write(v.Ticks);
                    w.Write(v.Offset.Ticks);
                },
                InternalConverterNamed),

            new BinaryConverterFactory<DateTimeOffset?>(static (r, m) => new DateTimeOffset(r.ReadInt64(), new TimeSpan(r.ReadInt64())),
                static (w, v, m) =>
                {
                    w.Write((v ?? DateTimeOffset.MinValue).Ticks);
                    w.Write((v ?? DateTimeOffset.MinValue).Offset.Ticks);
                },
                InternalConverterNamed),
            
            // DateOnly
            new BinaryConverterFactory<DateOnly>(static (r, m) => DateOnly.Parse(r.ReadString()),
                static (w, v, m) => w.Write( v.ToString() ), InternalConverterNamed),

            new BinaryConverterFactory<DateOnly?>(static (r, m) => DateOnly.Parse(r.ReadString()),
                static (w, v, m) => w.Write( (v ?? DateOnly.MinValue).ToString() ), InternalConverterNamed),
            
            // TimeOnly
            new BinaryConverterFactory<TimeOnly>(static (r, m) => new TimeOnly(r.ReadInt64()),
                static (w, v, m) => w.Write(v.Ticks), InternalConverterNamed),

            new BinaryConverterFactory<TimeOnly?>(static (r, m) => new TimeOnly(r.ReadInt64()),
                static (w, v, m) => w.Write( (v ?? TimeOnly.MinValue).Ticks ), InternalConverterNamed),
            
            // bool
            new BinaryConverterFactory<bool>(static (r, m) => r.ReadBoolean(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<bool?>(static (r, m) => r.ReadBoolean(),
                static (w, v, m) => w.Write(v ?? false), InternalConverterNamed),
            
            // byte
            new BinaryConverterFactory<byte>(static (r, m) => r.ReadByte(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<byte?>(static (r, m) => r.ReadByte(),
                static (w, v, m) => w.Write(v ?? byte.MinValue), InternalConverterNamed),
            
            // char
            new BinaryConverterFactory<char>(static (r, m) => r.ReadChar(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<char?>(static (r, m) => r.ReadChar(),
                static (w, v, m) => w.Write(v ?? char.MinValue), InternalConverterNamed),

            // decimal
            new BinaryConverterFactory<decimal>(static (r, m) => r.ReadDecimal(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<decimal?>(static (r, m) => r.ReadDecimal(),
                static (w, v, m) => w.Write(v ?? decimal.MinValue), InternalConverterNamed),

            // double
            new BinaryConverterFactory<double>(static (r, m) => r.ReadDouble(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<double?>(static (r, m) => r.ReadDouble(),
                static (w, v, m) => w.Write(v ?? double.MinValue), InternalConverterNamed),

            // Half
            new BinaryConverterFactory<Half>(static (r, m) => r.ReadHalf(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<Half?>(static (r, m) => r.ReadHalf(),
                static (w, v, m) => w.Write(v ?? Half.MinValue), InternalConverterNamed),

            // short
            new BinaryConverterFactory<short>(static (r, m) => r.ReadInt16(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<short?>(static (r, m) => r.ReadInt16(),
                static (w, v, m) => w.Write(v ?? short.MinValue), InternalConverterNamed),

            // int
            new BinaryConverterFactory<int>(static (r, m) => r.ReadInt32(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<int?>(static (r, m) => r.ReadInt32(),
                static (w, v, m) => w.Write(v ?? int.MinValue), InternalConverterNamed),

            // long
            new BinaryConverterFactory<long>(static (r, m) => r.ReadInt64(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<long?>(static (r, m) => r.ReadInt64(),
                static (w, v, m) => w.Write(v ?? long.MinValue), InternalConverterNamed),

            // sbyte
            new BinaryConverterFactory<sbyte>(static (r, m) => r.ReadSByte(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<sbyte?>(static (r, m) => r.ReadSByte(),
                static (w, v, m) => w.Write(v ?? sbyte.MinValue), InternalConverterNamed),

            // float
            new BinaryConverterFactory<float>(static (r, m) => r.ReadSingle(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<float?>(static (r, m) => r.ReadSingle(),
                static (w, v, m) => w.Write(v ?? float.MinValue), InternalConverterNamed),

            // string
            new BinaryConverterFactory<string>(static (r, m) => r.ReadString(),
                static (w, v, m) => w.Write(v ?? string.Empty), InternalConverterNamed),

            new BinaryConverterFactory<string?>(static (r, m) => r.ReadString(),
                static (w, v, m) => w.Write(v ?? string.Empty), InternalConverterNamed),

            // ushort
            new BinaryConverterFactory<ushort>(static (r, m) => r.ReadUInt16(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<ushort?>(static (r, m) => r.ReadUInt16(),
                static (w, v, m) => w.Write(v ?? ushort.MinValue), InternalConverterNamed),

            // uint
            new BinaryConverterFactory<uint>(static (r, m) => r.ReadUInt32(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<uint?>(static (r, m) => r.ReadUInt32(),
                static (w, v, m) => w.Write(v ?? uint.MinValue), InternalConverterNamed),

            // ulong
            new BinaryConverterFactory<ulong>(static (r, m) => r.ReadUInt64(),
                static (w, v, m) => w.Write(v), InternalConverterNamed),

            new BinaryConverterFactory<ulong?>(static (r, m) => r.ReadUInt64(),
                static (w, v, m) => w.Write(v ?? ulong.MinValue), InternalConverterNamed)
        };

        return converters;
    }


}
