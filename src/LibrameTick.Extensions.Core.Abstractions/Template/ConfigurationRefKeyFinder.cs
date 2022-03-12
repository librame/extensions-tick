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
/// 定义继承 <see cref="AbstractRefKeyFinder"/> 针对配置对象的 <see cref="RefKey"/> 查找器。
/// </summary>
public class ConfigurationRefKeyFinder : AbstractRefKeyFinder
{
    private TemplateOptions _templateOptions;


    /// <summary>
    /// 构造一个 <see cref="ConfigurationRefKeyFinder"/>。
    /// </summary>
    /// <param name="templateOptions">给定的 <see cref="TemplateOptions"/>。</param>
    public ConfigurationRefKeyFinder(TemplateOptions templateOptions)
        : base(templateOptions.RefKeys)
    {
        _templateOptions = templateOptions;
    }


    /// <summary>
    /// 填充核心。
    /// </summary>
    protected override void PopulateCore()
    {
        if (_templateOptions.Source is null)
            return;

        // 提取配置对象的所有引用键集合（支持键、值引用键）
        var keys = _templateOptions.Source.GetChildren()
            .SelectMany(s => Options.FindAll(s.Key).Concat(Options.FindAll(s.Value)))
            .ToList();

        foreach (var key in keys.DistinctBy(ks => ks.Name))
        {
            // 更新引用键值
            key.Value = _templateOptions.Source[key.Name];

            // 添加到缓存
            AddOrUpdate(key);
        }
    }

}