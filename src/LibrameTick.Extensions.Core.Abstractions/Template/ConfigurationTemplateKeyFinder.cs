#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Template;

/// <summary>
/// 定义继承 <see cref="AbstractTemplateKeyFinder"/> 且用于配置对象的 <see cref="TemplateKeyDescriptor"/> 查找器。
/// </summary>
public class ConfigurationTemplateKeyFinder : AbstractTemplateKeyFinder
{
    private ConfigurationTemplateOptions _templateOptions;


    /// <summary>
    /// 构造一个 <see cref="ConfigurationTemplateKeyFinder"/>。
    /// </summary>
    /// <param name="templateOptions">给定的 <see cref="ConfigurationTemplateOptions"/>。</param>
    public ConfigurationTemplateKeyFinder(ConfigurationTemplateOptions templateOptions)
        : base(templateOptions)
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

        // 提取配置对象的所有模板键集合（支持键、值模板键）
        var keys = _templateOptions.Source.GetChildren()
            .SelectMany(s => Options.FindAll(s.Key).Concat(Options.FindAll(s.Value)))
            .DistinctBy(ks => ks.Name)
            .ToList();

        foreach (var key in keys.DistinctBy(ks => ks.Name))
        {
            // 更新模板键值
            key.Value = _templateOptions.Source[key.Name];

            // 添加到缓存
            AddOrUpdate(key);
        }
    }

}