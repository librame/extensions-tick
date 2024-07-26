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

internal sealed class JsonDependency : IJsonDependency
{
    public JsonDependency()
    {
        Options = new(() =>
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            options.Converters.Add(new JsonStringEncodingConverter());
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        });
    }


    public Lazy<JsonSerializerOptions> Options { get; init; }

}
