#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Mapping;

/// <summary>
/// 定义一个可映射对象成员的对象映射器。
/// </summary>
public static class ObjectMapper
{
    /// <summary>
    /// 通过映射输入对象的所有公共属性创建输出类型的新对象。
    /// </summary>
    /// <param name="input">给定的输入对象。</param>
    /// <param name="outputType">给定的输出类型。</param>
    /// <returns>返回输出对象。</returns>
    public static object NewByMapAllPublicProperties(object input, Type outputType)
    {
        var output = Activator.CreateInstance(outputType);
        ArgumentNullException.ThrowIfNull(output);

        MapAllPublicProperties(input, output);

        return output;
    }

    /// <summary>
    /// 通过映射输入对象的所有字段（私有字段包含属性实现）创建输出类型的新对象。
    /// </summary>
    /// <param name="input">给定的输入对象。</param>
    /// <param name="outputType">给定的输出类型。</param>
    /// <returns>返回输出对象。</returns>
    public static object NewByMapAllFields(object input, Type outputType)
    {
        var output = Activator.CreateInstance(outputType);
        ArgumentNullException.ThrowIfNull(output);

        MapAllFields(input, output);

        return output;
    }


    /// <summary>
    /// 映射输入对象的所有公共属性到输出对象。
    /// </summary>
    /// <param name="input">给定的输入对象。</param>
    /// <param name="output">给定的输出对象。</param>
    /// <param name="inputType">给定的输入类型（可选；默认从 <paramref name="input"/> 获取）。</param>
    /// <param name="outputType">给定的输出类型（可选；默认从 <paramref name="output"/> 获取）。</param>
    public static void MapAllPublicProperties(object input, object output,
        Type? inputType = null, Type? outputType = null)
    {
        inputType ??= input.GetType();
        outputType ??= output.GetType();

        // 映射所有公共属性
        foreach (var outProp in outputType.GetProperties())
        {
            if (!outProp.CanWrite || outProp.PropertyType.IsSameOrNullableUnderlyingType(outputType))
                continue; // 排除自引用类型

            var inProp = inputType.GetProperty(outProp.Name);
            if (inProp is not null && outProp.PropertyType.IsSameOrNullableUnderlyingType(inProp.PropertyType))
            {
                var value = inProp.GetValue(input);
                outProp.SetValue(output, value);
            }
        }
    }

    /// <summary>
    /// 映射输入对象的所有字段（私有字段包含属性实现）到输出对象。
    /// </summary>
    /// <param name="input">给定的输入对象。</param>
    /// <param name="output">给定的输出对象。</param>
    /// <param name="inputType">给定的输入类型（可选；默认从 <paramref name="input"/> 获取）。</param>
    /// <param name="outputType">给定的输出类型（可选；默认从 <paramref name="output"/> 获取）。</param>
    public static void MapAllFields(object input, object output,
        Type? inputType = null, Type? outputType = null)
    {
        inputType ??= input.GetType();
        outputType ??= output.GetType();

        // 映射所有字段（私有字段包含属性实现）
        foreach (var outField in outputType.GetFields(TypeExtensions.AllMemberFlags))
        {
            if (outField.FieldType.IsSameOrNullableUnderlyingType(outputType))
                continue; // 排除自引用类型

            var inField = inputType.GetField(outField.Name, TypeExtensions.AllMemberFlags);
            if (inField is not null && outField.FieldType.IsSameOrNullableUnderlyingType(inField.FieldType))
            {
                var value = inField.GetValue(input);
                outField.SetValue(output, value);
            }
        }
    }

}
