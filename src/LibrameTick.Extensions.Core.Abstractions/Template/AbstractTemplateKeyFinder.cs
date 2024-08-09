#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.Template;

/// <summary>
/// 定义抽象实现 <see cref="ITemplateKeyFinder"/> 的查找器。
/// </summary>
public abstract class AbstractTemplateKeyFinder : ITemplateKeyFinder
{
    /// <summary>
    /// 构造一个 <see cref="AbstractTemplateKeyFinder"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="TemplateOptions"/>。</param>
    protected AbstractTemplateKeyFinder(TemplateOptions options)
    {
        Options = options;
        Options.PopulateKeysAction = Populate;
    }


    /// <summary>
    /// 模板选项。
    /// </summary>
    protected TemplateOptions Options { get; init; }

    /// <summary>
    /// 所有名称集合。
    /// </summary>
    public ICollection<string> AllNames
        => Options.Keys.Keys;


    /// <summary>
    /// 填充所有模板键集合。
    /// </summary>
    /// <param name="options">给定的选项。</param>
    public virtual void Populate(IOptions options)
    {
        Bootstrapper.GetLocker().Lock(i =>
        {
            if (Options.Keys.Count != 0)
                Options.Keys.Clear();

            PopulateCore();
        });
    }

    /// <summary>
    /// 填充核心。
    /// </summary>
    protected abstract void PopulateCore();


    /// <summary>
    /// 包含指定模板键。
    /// </summary>
    /// <param name="value">给定的 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool Contains(TemplateKeyDescriptor value)
        => Options.Keys.ContainsKey(value.Name);

    /// <summary>
    /// 包含指定模板键。
    /// </summary>
    /// <param name="name">给定的模板键名称。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool Contains(string name)
        => Options.Keys.ContainsKey(name);


    /// <summary>
    /// 添加或更新指定模板键。
    /// </summary>
    /// <param name="value">给定要添加或更新的 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回字符串。</returns>
    public virtual TemplateKeyDescriptor AddOrUpdate(TemplateKeyDescriptor value)
        => AddOrUpdate(value.Name, value);

    /// <summary>
    /// 添加或更新指定模板键。
    /// </summary>
    /// <param name="name">给定的模板键名称。</param>
    /// <param name="value">给定要添加或更新的 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回字符串。</returns>
    public virtual TemplateKeyDescriptor AddOrUpdate(string name, TemplateKeyDescriptor value)
    {
        Bootstrapper.GetLocker().Lock(i =>
        {
            if (Options.Keys.ContainsKey(name))
                Options.Keys[name] = value;
            else
                Options.Keys.Add(name, value);
        });

        return value;
    }


    /// <summary>
    /// 格式化模板中包含的模板键值。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <returns>返回经过格式化的字符串。</returns>
    public virtual string Format(string? template)
        => Format(template, out _);

    /// <summary>
    /// 格式化模板中包含的模板键值。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <param name="replaced">输出是否已替换的布尔值。</param>
    /// <returns>返回经过格式化的字符串。</returns>
    public virtual string Format(string? template, out bool replaced)
    {
        var _replaced = false;

        template = Options.Format(template, key =>
        {
            if (Options.Keys.TryGetValue(key.Name, out var result)
                && result.Value is not null)
            {
                if (!_replaced)
                    _replaced = true;

                return result.Value;
            }

            return null;
        });

        replaced = _replaced;
        return template;
    }


    /// <summary>
    /// 尝试获取指定模板键。
    /// </summary>
    /// <param name="name">给定的模板键名称。</param>
    /// <param name="value">输出 <see cref="TemplateKeyDescriptor"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool TryGetValue(string name, [MaybeNullWhen(false)] out TemplateKeyDescriptor value)
        => Options.Keys.TryGetValue(name, out value);

}