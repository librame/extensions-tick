#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cron;

/// <summary>
/// 定义抽象的 CRON 表达式（支持标准的 6 位或 7 位格式，具体格式为：秒 分 时 日 月 周 [年]）。
/// </summary>
/// <remarks>
///     <para>参考：https://mp.weixin.qq.com/s/GzbycJjudnm6XoZ_aJc3Fw</para>
///     <para>支持的通配符有 , - * / ? 等五个，其中：</para>
///     <para>逗号表示多个时间点并集，如月的位置使用 1,2 表示一二月都要执行；</para>
///     <para>短横线表示时间段的连续触发范围，如秒的位置使用 10-30 表示 10 秒到 30 秒之间每秒执行；</para>
///     <para>星号表示所有值，即每单位连续触发，如分的位置使用 * 表示每分执行；</para>
///     <para>正斜杠表示间隔触发，即开始时间/时间间隔，如秒的位置使用 5/20 表示从第 5 秒开始，每 20 秒执行；</para>
///     <para>问号只用于周和日的域，用于周和日之间进行互斥（避免冲突），通常在周和日设置一个的值，另外一个用 ? 表示不指定值，如 0 0 0 1 * ? 表示每月一号执行，忽略周几；</para>
///     <para>C 只用于周和日的域，表示周或月的第一天，如果 C 前有数字，则表示第 N 天，如 5C 表示月的第五天或月的星期四（1'SUN'-7'SAT'，也可以用 THUC 表示）（注：使用此参数不要指定列表或范围）；</para>
///     <para>L 只用于周和日的域，表示周或月的最后一天，如果 L 前有数字，则表示倒数第 N 天，如 6L 表示月的倒数第六天或月的最后一个星期五（1'SUN'-7'SAT'，也可以用 FRIL 表示）（注：使用此参数不要指定列表或范围）；</para>
///     <para>W 只用于日域，表示指定天（周一到周五）的工作日，如 15W 表示接近月的第十五个工作日，具体为月的第十五天为周六，那么在第十四天周五执行，如果第十五天为周日，则第十六天周一执行，除外在工作日当天执行，LW 表示月最后一个工作日（注：不能跨月只限当月，使用此参数不要指定列表或范围）。</para>
/// </remarks>
/// <typeparam name="TDateTime">指定的时间类型。</typeparam>
public abstract class AbstractCronExpression<TDateTime>
    where TDateTime : struct
{
    private static readonly Regex _lwRegex = new("^L-[0-9]*[W]?", RegexOptions.Compiled);

    private static readonly StringComparison _defaultComparison = StringComparison.Ordinal;

    private static readonly string[] _daysOfWeekDescriptions =
        [ "星期天", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" ];

    private ConcurrentDictionary<CronSyntax, string> _domainDescriptions;


    /// <summary>
    /// 构造一个 <see cref="DefaultCronExpressionOffset"/>。
    /// </summary>
    /// <param name="expression">给定的 CRON 表达式字符串。</param>
    /// <param name="options">给定的 CRON 选项。</param>
    protected AbstractCronExpression(string expression, CronOptions options)
    {
        expression = expression.Trim();
        ArgumentException.ThrowIfNullOrEmpty(expression, nameof(expression));

        Options = options;
        Descriptor = new(expression);

        _domainDescriptions = new ConcurrentDictionary<CronSyntax, string>();

        ParseExpression();
    }


    /// <summary>
    /// CRON 选项。
    /// </summary>
    public CronOptions Options { get; init; }

    /// <summary>
    /// CRON 描述符。
    /// </summary>
    public CronDescriptor Descriptor { get; init; }


    /// <summary>
    /// 获取最大年份。
    /// </summary>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <returns>返回整数值。</returns>
    protected abstract int GetMaxYear(TDateTime? baseTime);


    /// <summary>
    /// 获取基础时间之后的多次触发时间。
    /// </summary>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <param name="count">给定的次数。</param>
    /// <returns>返回 <see cref="IEnumerable{TDateTime}"/>。</returns>
    public virtual IEnumerable<TDateTime?> GetNextOccurrenceTimes(TDateTime baseTime, int count)
    {
        for (var i = 0; i < count; i++)
        {
            var currentTime = GetNextOccurrenceTime(baseTime);
            if (currentTime is not null)
                baseTime = currentTime.Value;

            yield return baseTime;
        }
    }

    /// <summary>
    /// 获取基础时间之后的下一次触发时间。
    /// </summary>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <returns>返回 <see cref="Nullable{TDateTime}"/>。</returns>
    public abstract TDateTime? GetNextOccurrenceTime(TDateTime baseTime);


    private void ParseExpression()
    {
        try
        {
            var syntax = CronSyntax.Second;

            var domainExpressions = Descriptor.UpperExpression.Split(Options.DomainSeparator,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var expr in domainExpressions)
            {
                var domain = expr.Trim();

                if (domain.Length == 0)
                    continue;

                if (syntax > CronSyntax.Year)
                    break;

                if (syntax == CronSyntax.DaysOfMonth && domain.Contains('L', _defaultComparison) && domain.Contains(','))
                    throw new FormatException("不支持在月份的其他日期指定 'L' 和 'LW'。");

                if (syntax == CronSyntax.DaysOfWeek && domain.Contains('L', _defaultComparison) && domain.Contains(','))
                    throw new FormatException("不支持在一周的其他日期指定 'L'。");

                if (syntax == CronSyntax.DaysOfWeek && domain.Contains("##"))
                    throw new FormatException("不支持指定多个第 N 天。");

                var domainSegments = domain.Split(Options.UnionSeparator);

                foreach (var segment in domainSegments)
                {
                    StoreExpressionValues(0, segment, syntax);
                }

                var description = GetDomainDescription(syntax, domain);
                _domainDescriptions.AddOrUpdate(syntax, key => description, (key, oldValue) => description);

                syntax++;
            }

            if (syntax <= CronSyntax.DaysOfWeek)
                throw new FormatException("表达式意料之外的结束。");

            if (syntax <= CronSyntax.Year)
                StoreExpressionValues(0, "*", CronSyntax.Year);

            var dayOfMSpec = !GetSet(CronSyntax.DaysOfMonth).Contains(Options.NoSpec);
            var dayOfWSpec = !GetSet(CronSyntax.DaysOfWeek).Contains(Options.NoSpec);

            if (dayOfMSpec && !dayOfWSpec)
            {
                // skip
            }
            else if (dayOfWSpec && !dayOfMSpec)
            {
                // skip
            }
            else
            {
                throw new FormatException("不支持同时指定星期和日参数。");
            }

            var allNotEmptyDescriptions = _domainDescriptions.Values
                .Where(static s => !string.IsNullOrEmpty(s))
                .Reverse();

            Descriptor.Description = string.Join(" ", FormatDomainDescriptions(allNotEmptyDescriptions));
        }
        catch (FormatException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FormatException($"无效的 cron 表达式格式 '{ex.GetInnermostMessage()}'。", ex);
        }
    }


    /// <summary>
    /// 格式化各域的描述符集合。
    /// </summary>
    /// <param name="descriptions">给定各域的描述符集合。</param>
    /// <returns>返回 <see cref="IEnumerable{String}"/>。</returns>
    protected virtual IEnumerable<string> FormatDomainDescriptions(IEnumerable<string> descriptions)
    {
        // 尝试跳过可能存在的多组每周期描述语言习惯（如：每月 每天...）
        //var lastEvery = descriptions.SelectPairsWith(s => s.StartsWith('每')).LastOrDefault();
        var lastEvery = descriptions.Where(s => s.StartsWith('每'))
            .Select((s, i) => new int?(i))
            .LastOrDefault();

        if (lastEvery is null)
            return descriptions;

        return descriptions.Skip(lastEvery.Value);
    }


    /// <summary>
    /// 获取指定域的描述。
    /// </summary>
    /// <param name="syntax">给定的 <see cref="CronSyntax"/>。</param>
    /// <param name="domain">给定的域表达式。</param>
    /// <returns>返回描述字符串。</returns>
    protected virtual string GetDomainDescription(CronSyntax syntax, string domain)
    {
        return syntax switch
        {
            CronSyntax.Second => domain == "*" ? "每秒" : $"{domain} 秒",

            CronSyntax.Minute => domain == "*" ? "每分" : $"{domain} 分",

            CronSyntax.Hour => domain == "*" ? "每时" : $"{domain} 时",

            CronSyntax.DaysOfMonth => domain == "*" || domain == "?" ? "每天" : $"{domain} 日",

            CronSyntax.Month => domain == "*" ? "每月" : $"{domain} 月",

            CronSyntax.DaysOfWeek => domain == "*" || domain == "?" ? string.Empty
                : string.Join(string.Empty, domain.ToCharArray().Select(GetDaysOfWeekDescription)),

            CronSyntax.Year => domain == "*" ? "每年" : $"{domain} 年",

            _ => string.Empty
        };


        static string GetDaysOfWeekDescription(char c)
        {
            if (c.IsDigit())
                return _daysOfWeekDescriptions[c];

            if (c == '-')
                return "至";

            return c.ToString();
        }
    }


    private int StoreExpressionValues(int pos, string domainOrSegment, CronSyntax syntax)
    {
        var incr = 0;

        var i = SkipWhiteSpace(pos, domainOrSegment);
        if (i >= domainOrSegment.Length)
            return i;

        var c = domainOrSegment[i];
        if (c >= 'A' && c <= 'Z' && !domainOrSegment.Equals("L") && !domainOrSegment.Equals("LW")
            && !_lwRegex.IsMatch(domainOrSegment))
        {
            var sub = domainOrSegment.Substring(i, 3);
            int sval;
            var eval = -1;

            if (syntax == CronSyntax.Month)
            {
                sval = GetMonthNumber(sub) + 1;
                if (sval <= 0)
                    throw new FormatException($"无效的月份值：'{sub}'。");

                if (domainOrSegment.Length > i + 3)
                {
                    c = domainOrSegment[i + 3];
                    if (c == '-')
                    {
                        i += 4;
                        sub = domainOrSegment.Substring(i, 3);
                        eval = GetMonthNumber(sub) + 1;
                        if (eval <= 0)
                            throw new FormatException($"无效的月份值：'{sub}'。");
                    }
                }
            }
            else if (syntax == CronSyntax.DaysOfWeek)
            {
                sval = GetDaysOfWeekNumber(sub);
                if (sval < 0)
                    throw new FormatException($"无效的星期值：'{sub}'。");

                if (domainOrSegment.Length > i + 3)
                {
                    c = domainOrSegment[i + 3];
                    if (c == '-')
                    {
                        i += 4;
                        sub = domainOrSegment.Substring(i, 3);
                        eval = GetDaysOfWeekNumber(sub);

                        if (eval < 0)
                            throw new FormatException($"无效的星期几值：'{sub}'。");
                    }
                    else if (c == '#')
                    {
                        try
                        {
                            i += 4;
                            Descriptor.NthdayOfWeek = Convert.ToInt32(domainOrSegment[i..], CultureInfo.InvariantCulture);

                            if (Descriptor.NthdayOfWeek is < 1 or > 5)
                                throw new FormatException("周的第 n 天小于 1 或大于 5。");
                        }
                        catch (Exception)
                        {
                            throw new FormatException("1 到 5 之间的数值必须跟在 '#' 选项后面。");
                        }
                    }
                    else if (c == '/')
                    {
                        try
                        {
                            i += 4;
                            Descriptor.EveryNthWeek = Convert.ToInt32(domainOrSegment[i..], CultureInfo.InvariantCulture);

                            if (Descriptor.EveryNthWeek is < 1 or > 5)
                                throw new FormatException("每个星期 <1 或 >5。");
                        }
                        catch (Exception)
                        {
                            throw new FormatException("1 到 5 之间的数值必须跟在 '/' 选项后面。");
                        }
                    }
                    else if (c == 'L')
                    {
                        Descriptor.LastdayOfWeek = true;
                        i++;
                    }
                    else
                    {
                        throw new FormatException($"此位置的非法字符：'{sub}'。");
                    }
                }
            }
            else
            {
                throw new FormatException($"此位置的非法字符：'{sub}'。");
            }

            if (eval != -1)
                incr = 1;

            AddToSet(sval, eval, incr, syntax);
            return i + 3;
        }

        if (c == '?')
        {
            i++;

            if (i + 1 < domainOrSegment.Length && domainOrSegment[i] != ' ' && domainOrSegment[i + 1] != '\t')
            {
                throw new FormatException($"'?' 后的非法字符: {domainOrSegment[i]}。");
            }
            if (syntax != CronSyntax.DaysOfWeek && syntax != CronSyntax.DaysOfMonth)
            {
                throw new FormatException("'?' 只能为月日或周日指定。");
            }
            if (syntax == CronSyntax.DaysOfWeek && !Descriptor.LastdayOfMonth)
            {
                var val = Descriptor.DaysOfMonth.LastOrDefault();
                if (val == Options.NoSpec)
                    throw new FormatException("'?' 只能为月日或周日指定。");
            }

            AddToSet(Options.NoSpec, -1, 0, syntax);
            return i;
        }

        var startsWithAsterisk = c == '*';
        if (startsWithAsterisk || c == '/')
        {
            if (startsWithAsterisk && i + 1 >= domainOrSegment.Length)
            {
                AddToSet(Options.AllSpec, -1, incr, syntax);
                return i + 1;
            }
            if (c == '/' && (i + 1 >= domainOrSegment.Length || domainOrSegment[i + 1] == ' ' || domainOrSegment[i + 1] == '\t'))
            {
                throw new FormatException("'/' 后面必须跟一个整数。");
            }
            if (startsWithAsterisk)
            {
                i++;
            }
            c = domainOrSegment[i];
            if (c == '/')
            {
                // is an increment specified?
                i++;
                if (i >= domainOrSegment.Length)
                {
                    throw new FormatException("字符串意外结束。");
                }

                incr = GetNumericValue(domainOrSegment, i);

                i++;
                if (incr > 10)
                {
                    i++;
                }
                CheckIncrementRange(incr, syntax);
            }
            else
            {
                if (startsWithAsterisk)
                {
                    throw new FormatException("星号后的非法字符：" + domainOrSegment);
                }
                incr = 1;
            }

            AddToSet(Options.AllSpec, -1, incr, syntax);
            return i;
        }

        if (c == 'L')
        {
            i++;
            if (syntax == CronSyntax.DaysOfMonth)
            {
                Descriptor.LastdayOfMonth = true;
            }
            if (syntax == CronSyntax.DaysOfWeek)
            {
                AddToSet(7, 7, 0, syntax);
            }
            if (syntax == CronSyntax.DaysOfMonth && domainOrSegment.Length > i)
            {
                c = domainOrSegment[i];
                if (c == '-')
                {
                    ValueSet vs = GetValueSet(0, domainOrSegment, i + 1);

                    Descriptor.LastdayOffset = vs.TheValue;
                    if (Descriptor.LastdayOffset > 30)
                        throw new FormatException("与最后一天的偏移量必须 <= 30。");

                    i = vs.Pos;
                }
                if (domainOrSegment.Length > i)
                {
                    c = domainOrSegment[i];
                    if (c == 'W')
                    {
                        Descriptor.NearestWeekday = true;
                        i++;
                    }
                }
            }
            return i;
        }

        if (c >= '0' && c <= '9')
        {
            var val = Convert.ToInt32(c.ToString(), CultureInfo.InvariantCulture);

            i++;
            if (i >= domainOrSegment.Length)
            {
                AddToSet(val, -1, -1, syntax);
            }
            else
            {
                c = domainOrSegment[i];
                if (c >= '0' && c <= '9')
                {
                    ValueSet vs = GetValueSet(val, domainOrSegment, i);
                    val = vs.TheValue;
                    i = vs.Pos;
                }

                i = CheckNext(i, domainOrSegment, val, syntax);
                return i;
            }
        }
        else
        {
            throw new FormatException($"意外字符：{c}");
        }

        return i;
    }


    private int CheckNext(int pos, string domainOrSegment, int val, CronSyntax syntax)
    {
        var end = -1;
        var i = pos;

        if (i >= domainOrSegment.Length)
        {
            AddToSet(val, end, -1, syntax);
            return i;
        }

        var c = domainOrSegment[pos];

        if (c == 'L')
        {
            if (syntax == CronSyntax.DaysOfWeek)
            {
                if (val < 1 || val > 7)
                    throw new FormatException("星期日值必须介于1和7之间。");

                Descriptor.LastdayOfWeek = true;
            }
            else
            {
                throw new FormatException($"'L' 选项在这里无效（位置={i}）。");
            }

            var data = GetSet(syntax);
            data.Add(val);
            i++;

            return i;
        }

        if (c == 'W')
        {
            if (syntax == CronSyntax.DaysOfMonth)
                Descriptor.NearestWeekday = true;
            else
                throw new FormatException($"'W' 选项在这里无效（位置={i}）。");

            if (val > 31)
                throw new FormatException("'W' 选项对于大于 31 的值（一个月中的最大天数）没有意义。");

            var data = GetSet(syntax);
            data.Add(val);

            i++;
            return i;
        }

        if (c == '#')
        {
            if (syntax != CronSyntax.DaysOfWeek)
                throw new FormatException($"'#' 选项在这里无效（位置={i}）。");

            i++;
            try
            {
                Descriptor.NthdayOfWeek = Convert.ToInt32(domainOrSegment.Substring(i), CultureInfo.InvariantCulture);

                if (Descriptor.NthdayOfWeek is < 1 or > 5)
                    throw new FormatException("周的第 n 天小于 1 或大于 5。");
            }
            catch (Exception)
            {
                throw new FormatException("1 到 5 之间的数值必须跟在'#'选项后面。");
            }

            var data = GetSet(syntax);
            data.Add(val);

            i++;
            return i;
        }

        if (c == 'C')
        {
            if (syntax == CronSyntax.DaysOfWeek)
            {
                //
            }
            else if (syntax == CronSyntax.DaysOfMonth)
            {
                //
            }
            else
            {
                throw new FormatException($"'C' 选项在这里无效（位置={i}）。");
            }

            var data = GetSet(syntax);
            data.Add(val);

            i++;
            return i;
        }

        if (c == '-')
        {
            i++;
            c = domainOrSegment[i];

            var v = Convert.ToInt32(c.ToString(), CultureInfo.InvariantCulture);
            end = v;

            i++;
            if (i >= domainOrSegment.Length)
            {
                AddToSet(val, end, 1, syntax);
                return i;
            }

            c = domainOrSegment[i];
            if (c >= '0' && c <= '9')
            {
                var vs = GetValueSet(v, domainOrSegment, i);
                var v1 = vs.TheValue;
                end = v1;
                i = vs.Pos;
            }

            if (i < domainOrSegment.Length && domainOrSegment[i] == '/')
            {
                i++;
                c = domainOrSegment[i];

                var v2 = Convert.ToInt32(c.ToString(), CultureInfo.InvariantCulture);

                i++;
                if (i >= domainOrSegment.Length)
                {
                    AddToSet(val, end, v2, syntax);
                    return i;
                }

                c = domainOrSegment[i];
                if (c >= '0' && c <= '9')
                {
                    var vs = GetValueSet(v2, domainOrSegment, i);
                    var v3 = vs.TheValue;

                    AddToSet(val, end, v3, syntax);

                    i = vs.Pos;
                    return i;
                }

                AddToSet(val, end, v2, syntax);
                return i;
            }

            AddToSet(val, end, 1, syntax);
            return i;
        }

        if (c == '/')
        {
            if (i + 1 >= domainOrSegment.Length || domainOrSegment[i + 1] == ' ' || domainOrSegment[i + 1] == '\t')
                throw new FormatException("\'/\' 后面必须跟一个整数。");

            i++;
            c = domainOrSegment[i];

            var v2 = Convert.ToInt32(c.ToString(), CultureInfo.InvariantCulture);

            i++;
            if (i >= domainOrSegment.Length)
            {
                CheckIncrementRange(v2, syntax);
                AddToSet(val, end, v2, syntax);
                return i;
            }

            c = domainOrSegment[i];
            if (c >= '0' && c <= '9')
            {
                var vs = GetValueSet(v2, domainOrSegment, i);
                var v3 = vs.TheValue;

                CheckIncrementRange(v3, syntax);
                AddToSet(val, end, v3, syntax);

                i = vs.Pos;
                return i;
            }

            throw new FormatException($"意外的字符 '{c}' 后 '/'。");
        }

        AddToSet(val, end, 0, syntax);

        i++;
        return i;
    }

    private void AddToSet(int val, int end, int incr, CronSyntax syntax)
    {
        var syntaxSet = GetSet(syntax);

        CheckValueRange(val, end, syntax);

        if ((incr == 0 || incr == -1) && val != Options.AllSpec)
        {
            syntaxSet.Add(val != -1 ? val : Options.NoSpec);
            return;
        }

        var startAt = val;
        var stopAt = end;

        if (val == Options.AllSpec && incr <= 0)
        {
            incr = 1;
            syntaxSet.Add(Options.AllSpec);
        }

        if (syntax == CronSyntax.Second || syntax == CronSyntax.Minute)
        {
            if (stopAt == -1)
                stopAt = 59;

            if (startAt == -1 || startAt == Options.AllSpec)
                startAt = 0;
        }
        else if (syntax == CronSyntax.Hour)
        {
            if (stopAt == -1)
                stopAt = 23;

            if (startAt == -1 || startAt == Options.AllSpec)
                startAt = 0;
        }
        else if (syntax == CronSyntax.DaysOfMonth)
        {
            if (stopAt == -1)
                stopAt = 31;

            if (startAt == -1 || startAt == Options.AllSpec)
                startAt = 1;
        }
        else if (syntax == CronSyntax.Month)
        {
            if (stopAt == -1)
                stopAt = 12;

            if (startAt == -1 || startAt == Options.AllSpec)
                startAt = 1;
        }
        else if (syntax == CronSyntax.DaysOfWeek)
        {
            if (stopAt == -1)
                stopAt = 7;

            if (startAt == -1 || startAt == Options.AllSpec)
                startAt = 1;
        }
        else if (syntax == CronSyntax.Year)
        {
            if (stopAt == -1)
                stopAt = GetMaxYear(baseTime: null);

            if (startAt == -1 || startAt == Options.AllSpec)
                startAt = 1970;
        }

        var max = -1;
        if (stopAt < startAt)
        {
            max = GetMaxValue(syntax);
            stopAt += max;
        }

        for (var i = startAt; i <= stopAt; i += incr)
        {
            if (max == -1)
            {
                syntaxSet.Add(i);
            }
            else
            {
                var i2 = i % max;
                if (i2 == 0 && (syntax == CronSyntax.Month
                    || syntax == CronSyntax.DaysOfWeek || syntax == CronSyntax.DaysOfMonth))
                {
                    i2 = max;
                }

                syntaxSet.Add(i2);
            }
        }
    }

    private void CheckValueRange(int val, int end, CronSyntax syntax)
    {
        if ((syntax == CronSyntax.Second || syntax == CronSyntax.Minute)
            && (val < 0 || val > 59 || end > 59)
            && val != Options.AllSpec)
        {
            throw new FormatException("分钟和秒值必须介于 0 和 59 之间");
        }
        else if (syntax == CronSyntax.Hour
            && (val < 0 || val > 23 || end > 23)
            && val != Options.AllSpec)
        {
            throw new FormatException("小时值必须介于 0 和 23 之间");
        }
        else if (syntax == CronSyntax.DaysOfMonth
            && (val < 1 || val > 31 || end > 31)
            && val != Options.AllSpec && val != Options.NoSpec)
        {
            throw new FormatException("月日值必须介于 1 和 31 之间");
        }
        else if (syntax == CronSyntax.Month
            && (val < 1 || val > 12 || end > 12)
            && val != Options.AllSpec)
        {
            throw new FormatException("月份值必须介于 1 和 12 之间");
        }
        else if (syntax == CronSyntax.DaysOfWeek
            && (val == 0 || val > 7 || end > 7)
            && val != Options.AllSpec && val != Options.NoSpec)
        {
            throw new FormatException("星期日值必须介于 1 和 7 之间。");
        }
    }

    private SortedSet<int> GetSet(CronSyntax syntax)
    {
        return syntax switch
        {
            CronSyntax.Second => Descriptor.Seconds,

            CronSyntax.Minute => Descriptor.Minutes,

            CronSyntax.Hour => Descriptor.Hours,

            CronSyntax.DaysOfMonth => Descriptor.DaysOfMonth,

            CronSyntax.Month => Descriptor.Months,

            CronSyntax.DaysOfWeek => Descriptor.DaysOfWeek,

            CronSyntax.Year => Descriptor.Years,

            _ => throw new ArgumentOutOfRangeException()
        };
    }


    private static int GetMaxValue(CronSyntax syntax)
    {
        return syntax switch
        {
            CronSyntax.Second => 60,

            CronSyntax.Minute => 60,

            CronSyntax.Hour => 24,

            CronSyntax.DaysOfMonth => 31,

            CronSyntax.Month => 12,

            CronSyntax.DaysOfWeek => 7,

            CronSyntax.Year => throw new ArgumentException("开始年份必须小于停止年份。"),

            _ => throw new ArgumentException("遇到意外的类型")
        };
    }

    private static void CheckIncrementRange(int incr, CronSyntax syntax)
    {
        if (incr > 59 && (syntax == CronSyntax.Second || syntax == CronSyntax.Minute))
            throw new FormatException($"增量 > 60：{incr}。");

        if (incr > 23 && syntax == CronSyntax.Hour)
            throw new FormatException($"增量 > 24：{incr}。");

        if (incr > 31 && syntax == CronSyntax.DaysOfMonth)
            throw new FormatException($"增量 > 31：{incr}。");

        if (incr > 7 && syntax == CronSyntax.DaysOfWeek)
            throw new FormatException($"增量 > 7：{incr}。");

        if (incr > 12 && syntax == CronSyntax.Month)
            throw new FormatException($"增量 > 12：{incr}。");
    }

    private static int SkipWhiteSpace(int pos, string domainOrSegment)
    {
        for (; pos < domainOrSegment.Length && (domainOrSegment[pos] == ' ' || domainOrSegment[pos] == '\t'); pos++)
        {
        }

        return pos;
    }

    private static int GetNumericValue(string domainOrSegment, int i)
    {
        var endOfVal = FindNextWhiteSpace(i, domainOrSegment);

        var val = domainOrSegment[i..endOfVal];

        return Convert.ToInt32(val, CultureInfo.InvariantCulture);

        // 查找下一个空格
        static int FindNextWhiteSpace(int i, string s)
        {
            for (; i < s.Length && (s[i] != ' ' || s[i] != '\t'); i++)
            {
            }

            return i;
        }
    }

    private static int GetMonthNumber(string monthKey)
    {
        if (CronExpressionExtensions.MonthsOfDate.TryGetValue(monthKey, out var value))
            return value;

        return -1;
    }

    private static int GetDaysOfWeekNumber(string daysOfWeekKey)
    {
        if (CronExpressionExtensions.DaysOfWeek.TryGetValue(daysOfWeekKey, out var value))
            return value;

        return -1;
    }

    private static ValueSet GetValueSet(int v, string domainOrSegment, int i)
    {
        var c = domainOrSegment[i];

        var s1 = new StringBuilder(v.ToString(CultureInfo.InvariantCulture));

        while (c >= '0' && c <= '9')
        {
            s1.Append(c);

            i++;
            if (i >= domainOrSegment.Length)
                break;

            c = domainOrSegment[i];
        }

        var val = new ValueSet();

        if (i < domainOrSegment.Length)
            val.Pos = i;
        else
            val.Pos = i + 1;

        val.TheValue = Convert.ToInt32(s1.ToString(), CultureInfo.InvariantCulture);

        return val;
    }


    private class ValueSet
    {
        public int TheValue { get; set; }

        public int Pos { get; set; }
    }

}
