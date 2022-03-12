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
/// 定义实现 <see cref="IOptions"/> 的引用键选项。
/// </summary>
public class RefKeyOptions : IOptions
{
    /// <summary>
    /// 用于刷新当前所有引用键集合的动作。
    /// </summary>
    [JsonIgnore]
    public Action? RefreshAction { get; set; }

    /// <summary>
    /// 定义引用键的正则表达式列表集合。
    /// </summary>
    [JsonIgnore]
    public List<Regex> Regexes { get; set; } = InitialRegexes();


    /// <summary>
    /// 从指定模板字符串中查找所有引用键列表集合。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <returns>返回 <see cref="List{RefKey}"/>。</returns>
    public virtual List<RefKey> FindAll(string template)
    {
        var keys = new List<RefKey>();

        Search(template, (temp, key, match) =>
        {
            if (!keys.Contains(key))
                keys.Add(key);

            return temp;
        });

        return keys;
    }

    /// <summary>
    /// 格式化指定模板字符串中包含的引用键。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <param name="refKeyValueFunc">给定用于格式化引用键的值方法。</param>
    /// <returns>返回经过格式化的模板字符串。</returns>
    public virtual string Format(string template,
        Func<RefKey, string?> refKeyValueFunc)
    {
        return Search(template, (temp, key, match) =>
        {
            var value = refKeyValueFunc(key);
            // 空字符串表示空值
            if (value is null)
                return temp; // 如果为空则表示不替换

            return temp.Replace(key.NamePattern, value);
        });
    }

    /// <summary>
    /// 查找指定模板字符串中包含的引用键。
    /// </summary>
    /// <param name="template">给定的模板字符串。</param>
    /// <param name="matchFunc">给定的模板匹配方法。</param>
    /// <returns>返回模板字符串。</returns>
    public virtual string Search(string template,
        Func<string, RefKey, Match, string> matchFunc)
    {
        if (string.IsNullOrWhiteSpace(template))
            return template;

        foreach (var regex in Regexes)
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


    /// <summary>
    /// 初始化引用键的正则表达式列表集合（默认支持“${Key}”格式键）。
    /// </summary>
    /// <returns>返回 <see cref="List{Regex}"/>。</returns>
    public static List<Regex> InitialRegexes()
    {
        return new List<Regex>()
        {
            new Regex(@"\$\{(\w+)\}")
        };
    }

    /// <summary>
    /// 从正则表达式匹配组集合中解析引用键列表集合。
    /// </summary>
    /// <param name="matchGroups">给定的 <see cref="GroupCollection"/>。</param>
    /// <returns>返回 <see cref="List{RefKey}"/>。</returns>
    public static List<RefKey> Parse(GroupCollection matchGroups)
    {
        ArgumentNullException.ThrowIfNull(matchGroups, nameof(matchGroups));

        if (matchGroups.Count % 2 != 0)
            throw new InvalidOperationException($"Invalid {nameof(matchGroups)}.");

        var keys = new List<RefKey>();

        for (var i = 0; i < matchGroups.Count; i = i + 2)
        {
            // matchGroups[0].Value = "${Key}" // Use InitialRegexes()
            // matchGroups[1].Value = "Key"
            var key = new RefKey(matchGroups[i].Value, matchGroups[i + 1].Value);
            if (!keys.Contains(key))
                keys.Add(key);
        }

        return keys;
    }

}