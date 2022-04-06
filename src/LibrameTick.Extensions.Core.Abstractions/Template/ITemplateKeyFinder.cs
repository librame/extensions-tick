#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Template;

/// <summary>
/// 定义模板键查找器接口。
/// </summary>
public interface ITemplateKeyFinder
{
    /// <summary>
    /// 所有名称集合。
    /// </summary>
    ICollection<string> AllNames { get; }


    /// <summary>
    /// 填充所有引用键集合。
    /// </summary>
    void Populate();


    /// <summary>
    /// 包含指定引用键。
    /// </summary>
    /// <param name="value">给定的 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    bool Contains(TemplateKeyDescriptor value);

    /// <summary>
    /// 包含指定引用键。
    /// </summary>
    /// <param name="name">给定的引用键名称。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    bool Contains(string name);


    /// <summary>
    /// 添加或更新指定引用键。
    /// </summary>
    /// <param name="value">给定要添加或更新的 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回字符串。</returns>
    TemplateKeyDescriptor AddOrUpdate(TemplateKeyDescriptor value);

    /// <summary>
    /// 添加或更新指定引用键。
    /// </summary>
    /// <param name="name">给定的引用键名称。</param>
    /// <param name="value">给定要添加或更新的 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回字符串。</returns>
    TemplateKeyDescriptor AddOrUpdate(string name, TemplateKeyDescriptor value);


    /// <summary>
    /// 格式化模板中包含的引用键值。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <returns>返回经过格式化的字符串。</returns>
    string Format(string template);

    /// <summary>
    /// 格式化模板中包含的引用键值。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <param name="replaced">输出是否已替换的布尔值。</param>
    /// <returns>返回经过格式化的字符串。</returns>
    string Format(string template, out bool replaced);


    /// <summary>
    /// 尝试获取指定引用键。
    /// </summary>
    /// <param name="name">给定的引用键名称。</param>
    /// <param name="value">输出 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    bool TryGetValue(string name, [MaybeNullWhen(false)] out TemplateKeyDescriptor value);
}