#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Template;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的模板选项。
/// </summary>
public class TemplateOptions : IOptions
{
    /// <summary>
    /// 填充模板键集合的动作。
    /// </summary>
    [JsonIgnore]
    public Action<IOptions>? PopulateKeysAction { get; set; }

    /// <summary>
    /// 模板键的正则表达式列表集合。
    /// </summary>
    [JsonIgnore]
    public List<Regex> KeyRegexes { get; set; } = InitialRegexes();

    /// <summary>
    /// 模板键的字典集合。
    /// </summary>
    [JsonIgnore]
    public Dictionary<string, TemplateKeyDescriptor> Keys { get; set; } = new();


    /// <summary>
    /// 从指定模板字符串中查找所有模板键列表集合。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <returns>返回 <see cref="List{TemplateKeyDescriptor}"/>。</returns>
    public virtual List<TemplateKeyDescriptor> FindAll(string? template)
    {
        var keys = new List<TemplateKeyDescriptor>();

        Search(template, (temp, key, match) =>
        {
            if (!keys.Contains(key))
                keys.Add(key);

            return temp;
        });

        return keys;
    }

    /// <summary>
    /// 格式化指定模板字符串中包含的模板键。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <param name="valueFunc">给定用于格式化模板键的值方法。</param>
    /// <returns>返回经过格式化的模板字符串。</returns>
    public virtual string Format(string? template,
        Func<TemplateKeyDescriptor, string?> valueFunc)
    {
        return Search(template, (temp, key, match) =>
        {
            var value = valueFunc(key);
            // 空字符串表示空值
            if (value is null)
                return temp; // 如果为空则表示不替换

            return temp.Replace(key.NamePattern, value);
        });
    }

    /// <summary>
    /// 查找指定模板字符串中包含的模板键。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <param name="matchFunc">给定的模板匹配方法。</param>
    /// <returns>返回模板字符串。</returns>
    public virtual string Search(string? template,
        Func<string, TemplateKeyDescriptor, Match, string> matchFunc)
    {
        if (string.IsNullOrWhiteSpace(template))
            return template ?? string.Empty;

        foreach (var regex in KeyRegexes)
        {
            if (regex.IsMatch(template))
            {
                foreach (Match match in regex.Matches(template))
                {
                    var keys = Parse(match.Groups);
                    foreach (var key in keys)
                    {
                        key.UsedRegex = regex;
                        template = matchFunc(template, key, match);
                    }
                }
            }

            if (string.IsNullOrEmpty(template))
                break;
        }

        return template;
    }

    private static List<TemplateKeyDescriptor> Parse(GroupCollection matchGroups)
    {
        if (matchGroups.Count % 2 != 0)
            throw new InvalidOperationException($"Invalid {nameof(matchGroups)}.");

        var keys = new List<TemplateKeyDescriptor>();

        for (var i = 0; i < matchGroups.Count; i = i + 2)
        {
            // matchGroups[0].Value = "${Key}" // Use InitialRegexes()
            // matchGroups[1].Value = "Key"
            var key = new TemplateKeyDescriptor(matchGroups[i].Value, matchGroups[i + 1].Value);
            if (!keys.Contains(key))
                keys.Add(key);
        }

        return keys;
    }


    /// <summary>
    /// 初始化模板键的正则表达式列表集合（默认支持“${Key}”格式键）。
    /// </summary>
    /// <returns>返回 <see cref="List{Regex}"/>。</returns>
    public static List<Regex> InitialRegexes()
    {
        return new List<Regex>()
        {
            new Regex(@"\$\{(\w+)\}")
        };
    }

}