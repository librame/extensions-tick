#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="string"/> 静态扩展。
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 0-9 的数字。
    /// </summary>
    public const string Digits = "0123456789";

    /// <summary>
    /// 26 个小写字母。
    /// </summary>
    public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// 26 个大写字母。
    /// </summary>
    public const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// 52 个大小写字母表。
    /// </summary>
    public const string AllLetters = UppercaseLetters + LowercaseLetters;

    /// <summary>
    /// 62 个大小写字母表和阿拉伯数字。
    /// </summary>
    public const string AllLettersAndDigits = AllLetters + Digits;


    /// <summary>
    /// 含有无效字符集合。
    /// </summary>
    /// <param name="value">给定的当前字符串。</param>
    /// <param name="invalidChars">用于检测的无效字符数组。</param>
    /// <returns>返回是否含有的布尔值。</returns>
    public static bool HasInvalidChars(this string value, IEnumerable<char> invalidChars)
        => value.ToCharArray().Any(c => invalidChars.Contains(c));


    #region IfNull

    /// <summary>
    /// 取得不为 NULL 字符串。
    /// </summary>
    /// <param name="current">给定用于判定的当前字符串。</param>
    /// <param name="defaultFunc">给定当 <paramref name="current"/> 为 NULL 时要返回的默认字符串方法。</param>
    /// <returns>返回 <paramref name="current"/> 或 <paramref name="defaultFunc"/> 字符串。</returns>
    public static string IfNull(this string? current, Func<string> defaultFunc)
        => current ?? defaultFunc();

    /// <summary>
    /// 取得不为 NULL 字符串。
    /// </summary>
    /// <param name="current">给定用于判定的当前字符串。</param>
    /// <param name="default">给定当 <paramref name="current"/> 为 NULL 时要返回的默认字符串。</param>
    /// <returns>返回 <paramref name="current"/> 或 <paramref name="default"/> 字符串。</returns>
    public static string IfNull(this string? current, string @default)
        => current ?? @default;


    /// <summary>
    /// 取得不为 NULL 或 Empty 的字符串。
    /// </summary>
    /// <param name="current">给定用于判定的当前字符串。</param>
    /// <param name="defaultFunc">给定当 <paramref name="current"/> 为 NULL 或 Empty 时要返回的默认字符串方法。</param>
    /// <returns>返回 <paramref name="current"/> 或 <paramref name="defaultFunc"/> 字符串。</returns>
    public static string IfNullOrEmpty(this string? current, Func<string> defaultFunc)
        => string.IsNullOrEmpty(current) ? defaultFunc() : current;

    /// <summary>
    /// 取得不为 NULL 或 Empty 的字符串。
    /// </summary>
    /// <param name="current">给定用于判定的当前字符串。</param>
    /// <param name="default">给定当 <paramref name="current"/> 为 NULL 或 Empty 时要返回的默认字符串。</param>
    /// <returns>返回 <paramref name="current"/> 或 <paramref name="default"/> 字符串。</returns>
    public static string IfNullOrEmpty(this string? current, string @default)
        => string.IsNullOrEmpty(current) ? @default : current;


    /// <summary>
    /// 取得不为 NULL、Empty 或空格的字符串。
    /// </summary>
    /// <param name="current">给定用于判定的当前字符串。</param>
    /// <param name="defaultFunc">给定当 <paramref name="current"/> 为 NULL、Empty 或空格时要返回的默认字符串方法。</param>
    /// <returns>返回 <paramref name="current"/> 或 <paramref name="defaultFunc"/> 字符串。</returns>
    public static string IfNullOrWhiteSpace(this string? current, Func<string> defaultFunc)
        => string.IsNullOrWhiteSpace(current) ? defaultFunc() : current;

    /// <summary>
    /// 取得不为 NULL、Empty 或空格的字符串。
    /// </summary>
    /// <param name="current">给定用于判定的当前字符串。</param>
    /// <param name="default">给定当 <paramref name="current"/> 为 NULL、Empty 或空格时要返回的默认字符串。</param>
    /// <returns>返回 <paramref name="current"/> 或 <paramref name="default"/> 字符串。</returns>
    public static string IfNullOrWhiteSpace(this string? current, string @default)
        => string.IsNullOrWhiteSpace(current) ? @default : current;

    #endregion


    #region Append and Insert

    /// <summary>
    /// 向当前字符串末尾附加字符串。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="append">给定的附加字符串。</param>
    /// <returns>返回字符串。</returns>
    public static string Append(this string current, string append)
        => $"{current}{append}";

    /// <summary>
    /// 向当前字符串的索引处插入字符串。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="insert">给定的插入字符串。</param>
    /// <param name="startIndex">给定要插入的开始索引（可选；默认为 0 表示在起始处插入）。</param>
    /// <returns>返回字符串。</returns>
    public static string Insert(this string current, string insert, int startIndex = 0)
    {
        if (startIndex <= 0)
            return $"{insert}{current}";

        if (startIndex >= current.Length - 1)
            return $"{current}{insert}"; // Append

        return $"{current.Substring(0, startIndex)}{insert}{current.Substring(startIndex)}";
    }

    #endregion


    #region Format

    /// <summary>
    /// 格式化字符串参数。
    /// </summary>
    /// <param name="format">给定的格式化字符串。</param>
    /// <param name="args">给定的参数数组。</param>
    /// <returns>返回字符串。</returns>
    public static string Format(this string format, params object[] args)
        => string.Format(CultureInfo.CurrentCulture, format, args);


    /// <summary>
    /// 将字节数组使用指定的时间刻度格式化为短字符串（如：8db56dc7163edf5）。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="timeTicks">给定的时间周期数。</param>
    /// <returns>返回字符串。</returns>
    public static string FormatString(this byte[] buffer, long timeTicks)
    {
        var i = 1L;

        foreach (var b in buffer)
        {
            i *= b + 1;
        }

        return "{0:x}".Format(_ = timeTicks);
    }


    /// <summary>
    /// 将数值格式化为 2 位长度的字符串（如：01）。
    /// </summary>
    /// <param name="number">给定的 32 位带符号整数。</param>
    /// <returns>返回字符串。</returns>
    public static string FormatString(this int number)
        => number.FormatString(2);

    /// <summary>
    /// 将数值格式化为指定长度的字符串。
    /// </summary>
    /// <param name="number">给定的 32 位带符号整数。</param>
    /// <param name="length">指定的长度。</param>
    /// <returns>返回字符串。</returns>
    public static string FormatString(this int number, int length)
    {
        return number.FormatString(length,
            static (f, v) => string.Format(CultureInfo.InvariantCulture, f, v));
    }


    /// <summary>
    /// 将数值格式化为 2 位长度的字符串（如：01）。
    /// </summary>
    /// <param name="number">给定的 64 位带符号整数。</param>
    /// <returns>返回字符串。</returns>
    public static string FormatString(this long number)
        => number.FormatString(2);

    /// <summary>
    /// 将数值格式化为指定长度的字符串。
    /// </summary>
    /// <param name="number">给定的 64 位带符号整数。</param>
    /// <param name="length">指定的长度。</param>
    /// <returns>返回字符串。</returns>
    public static string FormatString(this long number, int length)
    {
        return number.FormatString(length,
            static (f, v) => string.Format(CultureInfo.InvariantCulture, f, v));
    }


    private static string FormatString<TValue>(this TValue value, int length,
        Func<string, TValue, string> formatFactory)
        where TValue : struct
    {
        var valueString = value.ToString();

        if (valueString?.Length >= length)
            return valueString;

        var format = "0:";
        for (var i = 0; i < length; i++)
            format += "0";

        return formatFactory($"{{{format}}}", value);
    }

    #endregion


    #region JoinString

    /// <summary>
    /// 将字符串集合结合为字符串。
    /// </summary>
    /// <param name="values">给定的字符串集合。</param>
    /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
    /// <returns>返回结合字符串。</returns>
    public static string JoinString(this IEnumerable<string?> values, string? separator = null)
        => string.Join(separator ?? string.Empty, values);

    /// <summary>
    /// 将字符串集合结合为字符串。
    /// </summary>
    /// <param name="values">给定的字符串集合。</param>
    /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
    /// <returns>返回结合字符串。</returns>
    public static string JoinString(this IEnumerable<string?> values, char separator)
        => string.Join(separator, values);

    /// <summary>
    /// 将字符串集合结合为字符串。
    /// </summary>
    /// <param name="values">给定的字符串集合。</param>
    /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
    /// <returns>返回结合字符串。</returns>
    public static string JoinString<T>(this IEnumerable<T> values, string? separator = null)
        => string.Join(separator ?? string.Empty, values);

    /// <summary>
    /// 将字符串集合结合为字符串。
    /// </summary>
    /// <param name="values">给定的字符串集合。</param>
    /// <param name="separator">用于连接的分隔符（可选；默认不使用分隔符）。</param>
    /// <returns>返回结合字符串。</returns>
    public static string JoinString<T>(this IEnumerable<T> values, char separator)
        => string.Join(separator, values);

    #endregion


    #region Leading and Trailing

    /// <summary>
    /// 确保当前字符串以指定字符开始。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="leading">给定的开始字符。</param>
    /// <returns>返回字符串。</returns>
    public static string Leading(this string current, char leading)
        => current.StartsWith(leading) ? current : $"{leading}{current}";

    /// <summary>
    /// 确保当前字符串以指定字符开始。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="leading">给定的开始字符串。</param>
    /// <returns>返回字符串。</returns>
    public static string Leading(this string current, string leading)
        => current.StartsWith(leading) ? current : $"{leading}{current}";


    /// <summary>
    /// 确保当前字符串以指定字符结尾。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="trailing">给定的结尾字符。</param>
    /// <returns>返回字符串。</returns>
    public static string Trailing(this string current, char trailing)
        => current.EndsWith(trailing) ? current : $"{current}{trailing}";

    /// <summary>
    /// 确保当前字符串以指定字符结尾。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="trailing">给定的结尾字符串。</param>
    /// <returns>返回字符串。</returns>
    public static string Trailing(this string current, string trailing)
        => current.EndsWith(trailing) ? current : $"{current}{trailing}";

    #endregion


    #region Naming Conventions

    /// <summary>
    /// 将单词转换为对应首字符大写的形式。如将 hello 转换为 Hello。
    /// </summary>
    /// <param name="word">给定的英文单词。</param>
    /// <param name="separator">给定的分隔符。</param>
    /// <returns>返回字符串。</returns>
    public static string AsPascalCasing(this string word, char separator)
        => word.Split(separator).AsPascalCasing().JoinString(separator);

    /// <summary>
    /// 将单词转换为对应首字符大写的形式。如将 hello 转换为 Hello。
    /// </summary>
    /// <param name="word">给定的英文单词。</param>
    /// <param name="separator">给定的分隔符（可选）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsPascalCasing(this string word, string? separator = null)
        => !string.IsNullOrEmpty(separator) && word.Contains(separator)
            ? string.Join(separator, word.Split(separator).AsPascalCasing())
            : char.ToUpperInvariant(word[0]) + word[1..];

    /// <summary>
    /// 将数组的各单词转换为对应首字符大写的形式。如将 hello 转换为 Hello。
    /// </summary>
    /// <param name="words">给定的英文单词数组。</param>
    /// <returns>返回字符串数组。</returns>
    public static string[] AsPascalCasing(this string[] words)
    {
        var array = new string[words.Length];
        words.ForEach((word, i) =>
        {
            // 首字符大写
            array[i] = char.ToUpperInvariant(word[0]) + word[1..];
        });

        return array;
    }


    /// <summary>
    /// 将单词转换为对应首字符小写的形式。如将 Hello 转换为 hello。
    /// </summary>
    /// <param name="word">给定的英文单词。</param>
    /// <param name="separator">给定的分隔符。</param>
    /// <returns>返回字符串。</returns>
    public static string AsCamelCasing(this string word, char separator)
        => word.Split(separator).AsCamelCasing().JoinString(separator);

    /// <summary>
    /// 将单词转换为对应首字符小写的形式。如将 Hello 转换为 hello。
    /// </summary>
    /// <param name="word">给定的英文单词。</param>
    /// <param name="separator">给定的分隔符（可选）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsCamelCasing(this string word, string? separator = null)
        => !string.IsNullOrEmpty(separator) && word.Contains(separator)
            ? string.Join(separator, word.Split(separator).AsCamelCasing())
            : char.ToLowerInvariant(word[0]) + word[1..];

    /// <summary>
    /// 包含一到多个单词，第一个单词小写，其余单词中每一个单词第一个字母大写，其余字母均小写。例如：helloWorld 等。
    /// </summary>
    /// <param name="words">给定的英文单词（多个单词以空格区分）。</param>
    /// <returns>返回字符串。</returns>
    public static string[] AsCamelCasing(this string[] words)
    {
        var array = new string[words.Length];

        // 首单词首字符小写
        var first = words[0];
        array[0] = char.ToLowerInvariant(first[0]) + first[1..];

        if (words.Length > 1)
        {
            for (var i = 1; i < words.Length; i++)
            {
                // 首字符小写
                var word = words[i];
                array[i] = char.ToLowerInvariant(word[0]) + word[1..];
            }
        }

        return array;
    }

    #endregion


    #region Singular & Plural

    private static readonly List<(Regex, string)> _singularRegexList = new()
    {
        (new Regex("(?<keep>nese)$"), "${keep}"), // Prototype
        (new Regex("(?<keep>craft)$"), "${keep}"), // Prototype
        (new Regex("(?<keep>ee)"), "oo"),
        (new Regex("(?<keep>men)"), "man"),
        (new Regex("(?<keep>[lae])ves$"), "${keep}f"),
        (new Regex("(?<keep>[i])ves$"), "${keep}fe"),
        (new Regex("(?<keep>[^aeiou])ies$"), "${keep}y"),
        (new Regex("(?<keep>[aeiou]y)s$"), "${keep}"),
        (new Regex("(?<keep>[sxzh])es$"), "${keep}"),
        (new Regex("(?<keep>[^sxzhyu])s$"), "${keep}")
    };

    private static readonly List<(Regex, string)> _pluralRegexList = new()
    {
        (new Regex("(?<keep>nese)$"), "${keep}"), // Prototype
        (new Regex("(?<keep>craft)$"), "${keep}"), // Prototype
        (new Regex("(?<keep>oo)"), "ee"),
        (new Regex("(?<keep>man)"), "men"),
        (new Regex("(?<keep>[lae])f$"), "${keep}ves"),
        (new Regex("(?<keep>[i])fe$"), "${keep}ves"),
        (new Regex("(?<keep>[^aeiou])y$"), "${keep}ies"),
        (new Regex("(?<keep>[aeiou]y)$"), "${keep}s"),
        (new Regex("(?<keep>[sxzh])$"), "${keep}es"),
        (new Regex("(?<keep>[^sxzhy])$"), "${keep}s")
    };

    private static readonly Dictionary<string, string> _irregularPlurals = new()
    {
        { "child", "children" },
        { "ox", "oxen" },
        { "mouse", "mice" },
        { "person", "people" },
        { "policeman", "police" }
    };


    /// <summary>
    /// 复数单词单数化（支持常见的不规则复数单词）。
    /// </summary>
    /// <param name="plural">给定的复数化英文单词。</param>
    /// <returns>返回字符串。</returns>
    public static string AsSingularize(this string plural)
    {
        foreach (var pair in _irregularPlurals)
        {
            if (pair.Value == plural)
                return pair.Key;
        }

        foreach (var (r, s) in _singularRegexList)
        {
            if (r.IsMatch(plural))
                return r.Replace(plural, s);
        }

        return plural;
    }

    /// <summary>
    /// 单数单词复数化（支持常见的不规则复数单词）。
    /// </summary>
    /// <param name="singular">给定的单数化英文单词。</param>
    /// <returns>返回字符串。</returns>
    public static string AsPluralize(this string singular)
    {
        if (_irregularPlurals.TryGetValue(singular, out var value))
            return value;

        foreach (var (r, s) in _pluralRegexList)
        {
            if (r.IsMatch(singular))
                return r.Replace(singular, s);
        }

        return singular;
    }

    #endregion


    #region TrySplitPair

    /// <summary>
    /// 尝试分拆包含指定分隔符的键值对字符串。
    /// </summary>
    /// <param name="value">给定的字符串值。</param>
    /// <param name="separator">给定的分隔符。</param>
    /// <param name="pair">输出 <see cref="KeyValuePair{String, String}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TrySplitPair(this string value, char separator,
        [MaybeNullWhen(false)] out KeyValuePair<string, string> pair)
    {
        var separatorIndex = value.IndexOf(separator);

        // 分隔符不能位于起始或末尾
        if (separatorIndex > 0 && separatorIndex < value.Length - 1)
        {
            pair = new(value[..separatorIndex], value[(separatorIndex + 1)..]);

            return true;
        }

        pair = new();
        return false;
    }

    /// <summary>
    /// 尝试分拆包含指定分隔符的键值对字符串。
    /// </summary>
    /// <param name="value">给定的字符串值。</param>
    /// <param name="separator">给定的分隔符。</param>
    /// <param name="pair">输出 <see cref="KeyValuePair{String, String}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TrySplitPair(this string value, string separator,
        [MaybeNullWhen(false)] out KeyValuePair<string, string> pair)
    {
        var separatorIndex = value.IndexOf(separator, StringComparison.OrdinalIgnoreCase);

        // 分隔符不能位于起始或末尾
        if (separatorIndex > 0 && separatorIndex < value.Length - separator.Length)
        {
            pair = new(value[..separatorIndex], value[(separatorIndex + separator.Length)..]);

            return true;
        }

        pair = new();
        return false;
    }


    /// <summary>
    /// 尝试分拆最后一个包含指定分隔符的键值对字符串。
    /// </summary>
    /// <param name="value">给定的字符串值。</param>
    /// <param name="separator">给定的分隔符。</param>
    /// <param name="pair">输出 <see cref="KeyValuePair{String, String}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TrySplitPairByLastIndexOf(this string value, char separator,
        [MaybeNullWhen(false)] out KeyValuePair<string, string> pair)
    {
        var separatorIndex = value.LastIndexOf(separator);

        // 分隔符不能位于起始或末尾
        if (separatorIndex > 0 && separatorIndex < value.Length - 1)
        {
            pair = new(value[..separatorIndex], value[(separatorIndex + 1)..]);

            return true;
        }

        pair = new();
        return false;
    }

    /// <summary>
    /// 尝试分拆最后一个包含指定分隔符的键值对字符串。
    /// </summary>
    /// <param name="value">给定的字符串值。</param>
    /// <param name="separator">给定的分隔符。</param>
    /// <param name="pair">输出 <see cref="KeyValuePair{String, String}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TrySplitPairByLastIndexOf(this string value, string separator,
        [MaybeNullWhen(false)] out KeyValuePair<string, string> pair)
    {
        var separatorIndex = value.LastIndexOf(separator, StringComparison.OrdinalIgnoreCase);

        // 分隔符不能位于起始或末尾
        if (separatorIndex > 0 && separatorIndex < value.Length - separator.Length)
        {
            pair = new(value[..separatorIndex], value[(separatorIndex + separator.Length)..]);

            return true;
        }

        pair = new();
        return false;
    }

    #endregion


    #region SystemString

    /// <summary>
    /// 将长整数转换为进制字符串（可逆）。
    /// </summary>
    /// <param name="number">给定的长整数。</param>
    /// <param name="system">给定的进制（可选；进制越低，位数越长；默认为 52 进制）。</param>
    /// <param name="mapCharset">映射字符集（可选；默认使用 <see cref="AllLetters"/>）。</param>
    /// <returns>返回字符串。</returns>
    public static string AsSystemString(this long number, int? system = null,
        string? mapCharset = null)
    {
        (var sys, var mc) = ValidSystemParameters(system, mapCharset);

        var values = new List<string>();

        var n = number;
        while (n > 0)
        {
            var mod = n % sys;
            n = Math.Abs(n / sys);

            var c = mc[Convert.ToInt32(mod)];
            values.Insert(0, c.ToString());
        }

        return values.JoinString();
    }

    /// <summary>
    /// 将进制字符串还原为长整数（可逆）。
    /// </summary>
    /// <param name="systemString">给定的进制字符串。</param>
    /// <param name="system">给定的进制（可选；进制越低，位数越长；默认为 52 进制）。</param>
    /// <param name="mapCharset">映射字符集（可选；默认使用 <see cref="AllLetters"/>）。</param>
    /// <returns>返回长整数。</returns>
    public static long FromSystemString(this string systemString, int? system = null,
        string? mapCharset = null)
    {
        (var sys, var mc) = ValidSystemParameters(system, mapCharset);

        var value = 0L;
        systemString.ToCharArray().Reverse().ForEach((c, i) =>
        {
            var indexOf = mc.IndexOf(c);
            if (indexOf >= 0)
            {
                value += indexOf * (long)Math.Pow(sys, i);
            }
        });

        return value;
    }

    private static (int system, string mapCharset) ValidSystemParameters(int? system = null,
        string? mapCharset = null)
    {
        if (string.IsNullOrEmpty(mapCharset))
            mapCharset = AllLetters;

        if (system.HasValue)
        {
            system.Value.NotOutOfRange(2, mapCharset.Length, paramName: nameof(system));

            if (mapCharset.Length > system.Value)
                mapCharset = mapCharset[..system.Value];
        }
        else
        {
            system = mapCharset.Length;
        }

        return (system.Value, mapCharset);
    }

    #endregion


    #region Trim

    /// <summary>
    /// 修剪首尾指定字符串。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="trim">要修剪的字符串（如果为空则直接返回）。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的字符串。</returns>
    public static string Trim(this string current, string trim, bool isLoops = true)
        => current.TrimStart(trim, isLoops).TrimEnd(trim, isLoops);

    /// <summary>
    /// 修剪首部指定字符串。
    /// </summary>
    /// <param name="current">给定的当前字符串。</param>
    /// <param name="trim">要修剪的字符串（如果为空则直接返回）。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的字符串。</returns>
    public static string TrimStart(this string current, string trim, bool isLoops = true)
    {
        if (current.Length > 0 && current.StartsWith(trim))
        {
            current = current[trim.Length..];

            if (isLoops)
                current = current.TrimStart(trim, isLoops);
        }

        return current;
    }

    /// <summary>
    /// 修剪尾部指定字符串。
    /// </summary>
    /// <param name="current">指定的当前字符串。</param>
    /// <param name="trim">要修剪的字符串（如果为空则直接返回）。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的字符串。</returns>
    public static string TrimEnd(this string current, string trim, bool isLoops = true)
    {
        if (current.Length > 0 && current.EndsWith(trim))
        {
            current = current[..^trim.Length];

            if (isLoops)
                current = current.TrimEnd(trim, isLoops);
        }

        return current;
    }

    #endregion

}
