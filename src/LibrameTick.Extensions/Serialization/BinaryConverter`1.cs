﻿#region License

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
/// 定义抽象实现 <see cref="IBinaryConverter"/> 的泛型二进制转换器。
/// </summary>
/// <typeparam name="T">指定要转换的目标类型。</typeparam>
public abstract class BinaryConverter<T> : IBinaryConverter
{
    /// <summary>
    /// 获取要转换的目标类型。
    /// </summary>
    /// <value>
    /// 返回类型实例。
    /// </value>
    public virtual Type TargetType => typeof(T);

    /// <summary>
    /// 获取当前转换器类型。
    /// </summary>
    /// <value>
    /// 返回类型实例。
    /// </value>
    public virtual Type CurrentType => GetType();

    /// <summary>
    /// 获取当前转换器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public virtual string CurrentName => CurrentType.Name;

    /// <summary>
    /// 获取是否处理空值。
    /// </summary>
    /// <remarks>
    /// 因可空值类型与引用类型（可空引用类型只是编译器特性，实际是引用类型本身）可以为空，所以需要被处理。
    /// </remarks>
    /// <value>
    /// 返回是否处理空值的布尔值。
    /// </value>
    public virtual bool HandleNull
        => TargetType.IsNullable() || TargetType.IsClass;


    /// <summary>
    /// 获取读取方法。
    /// </summary>
    /// <returns>返回 <see cref="MethodInfo"/>。</returns>
    public virtual MethodInfo GetReadMethod()
        => CurrentType.GetMethod(nameof(Read))!;

    /// <summary>
    /// 获取写入方法。
    /// </summary>
    /// <returns>返回 <see cref="MethodInfo"/>。</returns>
    public virtual MethodInfo GetWriteMethod()
        => CurrentType.GetMethod(nameof(Write))!;


    /// <summary>
    /// 判断类型可以转换。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <returns>
    /// 返回能转换的布尔值。
    /// </returns>
    public virtual bool CanConvert(Type typeToConvert)
        => typeToConvert == TargetType;


    /// <summary>
    /// 通过读取器读取指定类型实例。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。注：此处 ? 表示为虚可空类型，实际是否为可空类型由 <typeparamref name="T"/> 决定。</returns>
    public virtual T? Read(BinaryReader reader, Type typeToConvert, BinaryMemberInfo member)
    {
        if (!CanConvert(typeToConvert))
        {
            throw new NotSupportedException($"Unsupported convert type '{typeToConvert}'.");
        }

        if (HandleNull && reader.ReadBoolean())
        {
            return default;
        }
        else
        {
            return ReadCore(reader, member);
        }
    }

    /// <summary>
    /// 通过读取器读取指定类型实例核心。
    /// </summary>
    /// <param name="reader">给定的 <see cref="BinaryReader"/>。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    protected abstract T ReadCore(BinaryReader reader, BinaryMemberInfo member);


    /// <summary>
    /// 通过写入器写入指定类型实例。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <param name="value">给定要写入的 <typeparamref name="T"/>。注：此处 ? 表示为虚可空类型，实际是否为可空类型由 <typeparamref name="T"/> 决定。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    public virtual void Write(BinaryWriter writer, Type typeToConvert, T? value, BinaryMemberInfo member)
    {
        if (!CanConvert(typeToConvert))
        {
            throw new NotSupportedException($"Unsupported convert type '{typeToConvert}'.");
        }

        if (HandleNull)
        {
            writer.Write(value is null);
        }

        if (value is not null)
        {
            WriteCore(writer, value, member);
        }
    }

    /// <summary>
    /// 通过写入器写入指定类型实例核心。
    /// </summary>
    /// <param name="writer">给定的 <see cref="BinaryWriter"/>。</param>
    /// <param name="value">给定要写入的 <typeparamref name="T"/>。</param>
    /// <param name="member">给定的 <see cref="BinaryMemberInfo"/>。</param>
    protected abstract void WriteCore(BinaryWriter writer, T value, BinaryMemberInfo member);
}
