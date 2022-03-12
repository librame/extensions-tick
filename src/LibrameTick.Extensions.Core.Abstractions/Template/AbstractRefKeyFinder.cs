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
/// 定义抽象实现 <see cref="IRefKeyFinder"/> 的查找器。
/// </summary>
public abstract class AbstractRefKeyFinder : IRefKeyFinder
{
    private readonly ConcurrentDictionary<string, RefKey> _keys = new();


    /// <summary>
    /// 构造一个 <see cref="AbstractRefKeyFinder"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="RefKeyOptions"/>。</param>
    protected AbstractRefKeyFinder(RefKeyOptions options)
    {
        Options = options;
        Options.RefreshAction = Populate;
    }


    /// <summary>
    /// 引用键选项。
    /// </summary>
    protected RefKeyOptions Options { get; init; }

    /// <summary>
    /// 所有名称集合。
    /// </summary>
    public ICollection<string> AllNames
        => _keys.Keys;


    /// <summary>
    /// 填充所有引用键集合。
    /// </summary>
    public virtual void Populate()
    {
        if (_keys.Count != 0)
            _keys.Clear();

        PopulateCore();
    }

    /// <summary>
    /// 填充核心。
    /// </summary>
    protected abstract void PopulateCore();


    /// <summary>
    /// 包含指定引用键。
    /// </summary>
    /// <param name="value">给定的 <see cref="RefKey"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool Contains(RefKey value)
        => _keys.ContainsKey(value.Name);

    /// <summary>
    /// 包含指定引用键。
    /// </summary>
    /// <param name="name">给定的引用键名称。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool Contains(string name)
        => _keys.ContainsKey(name);


    /// <summary>
    /// 添加或更新指定引用键。
    /// </summary>
    /// <param name="value">给定要添加或更新的 <see cref="RefKey"/>。</param>
    /// <returns>返回字符串。</returns>
    public virtual RefKey AddOrUpdate(RefKey value)
        => AddOrUpdate(value.Name, value);

    /// <summary>
    /// 添加或更新指定引用键。
    /// </summary>
    /// <param name="name">给定的引用键名称。</param>
    /// <param name="value">给定要添加或更新的 <see cref="RefKey"/>。</param>
    /// <returns>返回字符串。</returns>
    public virtual RefKey AddOrUpdate(string name, RefKey value)
        => _keys.AddOrUpdate(name, value, (key, value) => value);


    /// <summary>
    /// 格式化模板中包含的引用键值。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <returns>返回经过格式化的字符串。</returns>
    public virtual string Format(string template)
        => Format(template, out _);

    /// <summary>
    /// 格式化模板中包含的引用键值。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <param name="replaced">输出是否已替换的布尔值。</param>
    /// <returns>返回经过格式化的字符串。</returns>
    public virtual string Format(string template, out bool replaced)
    {
        var _replaced = false;

        template = Options.Format(template, key =>
        {
            if (_keys.TryGetValue(key.Name, out var result)
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
    /// 尝试获取指定引用键。
    /// </summary>
    /// <param name="name">给定的引用键名称。</param>
    /// <param name="value">输出 <see cref="RefKey"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool TryGetValue(string name, [MaybeNullWhen(false)] out RefKey value)
        => _keys.TryGetValue(name, out value);

}