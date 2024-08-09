#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Compression.Internal;

internal sealed class CompressionAdapterResolver(CompressionOptions options) : ICompressionAdapterResolver
{
    public CompressionOptions Options { get; init; } = options;


    public CompressionAdapter<TAdapted, TCompressed>? Resolve<TAdapted, TCompressed>(
        CompressionAdapterAttribute? attribute)
    {
        var typeToAdapt = typeof(TAdapted);
        var typeToCompress = typeof(TCompressed);

        if (attribute?.AdaptedType is not null)
        {
            typeToAdapt = attribute.AdaptedType;
        }

        if (attribute?.CompressedType is not null)
        {
            typeToCompress = attribute.CompressedType;
        }

        return (CompressionAdapter<TAdapted, TCompressed>?)Resolve(typeToAdapt, typeToCompress, attribute?.Name);
    }

    public CompressionAdapter<TAdapted, TCompressed>? Resolve<TAdapted, TCompressed>(string? name = null)
        => (CompressionAdapter<TAdapted, TCompressed>?)Resolve(typeof(TAdapted), typeof(TCompressed), name);

    public ICompressionAdapter? Resolve(Type typeToAdapt, Type typeToCompress, CompressionAdapterAttribute? attribute)
    {
        if (attribute?.AdaptedType is not null)
        {
            typeToAdapt = attribute.AdaptedType;
        }

        if (attribute?.CompressedType is not null)
        {
            typeToCompress = attribute.CompressedType;
        }

        return Resolve(typeToAdapt, typeToCompress, attribute?.Name);
    }

    public ICompressionAdapter? Resolve(Type typeToAdapt, Type typeToCompress, string? name = null)
    {
        // 优先解析适配类型
        var adapters = Options.Adapters.Where(c => c.BeAdaptedType == typeToAdapt && c.BeCompressedType == typeToCompress);
        if (name is not null)
        {
            // 如果存在多个适配器，则优先解析名称，否则直接解析适配器名称
            adapters = (adapters.Count() > 1 ? adapters : Options.Adapters)
                .Where(c => c.AdapterName == name);
        }

        var adapter = adapters.FirstOrDefault();

        return adapter;
    }

}
