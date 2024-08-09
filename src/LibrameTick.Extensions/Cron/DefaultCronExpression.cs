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
/// 定义默认实现 <see cref="CronExpression{TDateTime}"/> 的 CRON 表达式。
/// </summary>
/// <param name="expression">给定的 CRON 表达式字符串。</param>
/// <param name="options">给定的 CRON 选项。</param>
public class DefaultCronExpression(string expression, CronOptions options)
    : CronExpression<DateTime>(expression, options)
{

    /// <summary>
    /// 获取最大年份。
    /// </summary>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <returns>返回整数值。</returns>
    protected override int GetMaxYear(DateTime? baseTime)
        => (baseTime ?? DateTime.Now).Year + 100;


    /// <summary>
    /// 在给定时间之后获取下一次触发时间。
    /// </summary>
    /// <param name="baseTime">给定的基础时间。</param>
    /// <returns>返回 <see cref="Nullable{DateTime}"/>。</returns>
    public override DateTime? GetNextOccurrenceTime(DateTime baseTime)
    {
        // 向前移动一秒钟，因为我们正在计算时间*之后*
        baseTime = baseTime.AddSeconds(1);

        // 不处理毫秒
        var date = baseTime.WithoutMillisecond();

        var gotOne = false;

        // 循环直到我们计算出下一次，或者我们已经过了 endTime
        while (!gotOne)
        {
            SortedSet<int> set;
            int t;

            var sec = date.Second;
            set = Descriptor.Seconds.GetViewBetween(sec, 9999999);

            if (set.Count > 0)
            {
                sec = set.First();
            }
            else
            {
                sec = Descriptor.Seconds.First();
                date = date.AddMinutes(1);
            }

            date = date.With(newSecond: sec);

            var min = date.Minute;
            var hr = date.Hour;

            t = -1;

            set = Descriptor.Minutes.GetViewBetween(min, 9999999);
            if (set.Count > 0)
            {
                t = min;
                min = set.First();
            }
            else
            {
                min = Descriptor.Minutes.First();
                hr++;
            }

            if (min != t)
            {
                date = date.With(newMinute: min, newSecond: 0);
                date = SetCalendarHour(date, hr);
                continue;
            }

            date = date.With(newMinute: min);

            hr = date.Hour;
            var day = date.Day;

            t = -1;

            set = Descriptor.Hours.GetViewBetween(hr, 9999999);
            if (set.Count > 0)
            {
                t = hr;
                hr = set.First();
            }
            else
            {
                hr = Descriptor.Hours.First();
                day++;
            }

            if (hr != t)
            {
                var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                if (day > daysInMonth)
                {
                    date = date.With(newDay: daysInMonth, newMinute: 0, newSecond: 0).AddDays(day - daysInMonth);
                }
                else
                {
                    date = date.With(newDay: day, newMinute: 0, newSecond: 0);
                }

                date = SetCalendarHour(date, hr);
                continue;
            }

            date = date.With(newHour: hr);

            day = date.Day;
            var mon = date.Month;

            t = -1;
            var tmon = mon;

            var dayOfMSpec = !Descriptor.DaysOfMonth.Contains(Options.NoSpec);
            var dayOfWSpec = !Descriptor.DaysOfWeek.Contains(Options.NoSpec);

            if (dayOfMSpec && !dayOfWSpec)
            {
                // 逐月获取规则
                set = Descriptor.DaysOfMonth.GetViewBetween(day, 9999999);
                var found = set.Any();

                if (Descriptor.LastdayOfMonth)
                {
                    if (!Descriptor.NearestWeekday)
                    {
                        t = day;

                        day = DateTime.DaysInMonth(date.Year, mon);
                        day -= Descriptor.LastdayOffset;

                        if (t > day)
                        {
                            mon++;
                            if (mon > 12)
                            {
                                mon = 1;
                                tmon = 3333; // 确保下面的 mon != tmon 测试失败
                                date = date.AddYears(1);
                            }

                            day = 1;
                        }
                    }
                    else
                    {
                        t = day;

                        day = DateTime.DaysInMonth(date.Year, mon);
                        day -= Descriptor.LastdayOffset;

                        var tcal = new DateTime(date.Year, mon, day, 0, 0, 0, date.Kind);

                        var ldom = DateTime.DaysInMonth(date.Year, mon);
                        var dow = tcal.DayOfWeek;

                        if (dow == DayOfWeek.Saturday && day == 1)
                            day += 2;
                        else if (dow == DayOfWeek.Saturday)
                            day -= 1;
                        else if (dow == DayOfWeek.Sunday && day == ldom)
                            day -= 2;
                        else if (dow == DayOfWeek.Sunday)
                            day += 1;

                        var nTime = new DateTime(tcal.Year, mon, day, hr, min, sec, date.Millisecond, date.Kind);
                        if (nTime < baseTime) // nTime.ToUniversalTime()
                        {
                            day = 1;
                            mon++;
                        }
                    }
                }
                else if (Descriptor.NearestWeekday)
                {
                    t = day;
                    day = Descriptor.DaysOfMonth.First();

                    var tcal = new DateTime(date.Year, mon, day, 0, 0, 0, date.Kind);

                    var ldom = DateTime.DaysInMonth(date.Year, mon);
                    var dow = tcal.DayOfWeek;

                    if (dow == DayOfWeek.Saturday && day == 1)
                        day += 2;
                    else if (dow == DayOfWeek.Saturday)
                        day -= 1;
                    else if (dow == DayOfWeek.Sunday && day == ldom)
                        day -= 2;
                    else if (dow == DayOfWeek.Sunday)
                        day += 1;

                    tcal = new(tcal.Year, mon, day, hr, min, sec, tcal.Kind);
                    if (tcal < baseTime) // tcal.ToUniversalTime()
                    {
                        day = Descriptor.DaysOfMonth.First();
                        mon++;
                    }
                }
                else if (found)
                {
                    t = day;
                    day = set.First();

                    // 确保我们不会在短时间内跑得过快，比如二月
                    var lastDay = DateTime.DaysInMonth(date.Year, mon);
                    if (day > lastDay)
                    {
                        day = Descriptor.DaysOfMonth.First();
                        mon++;
                    }
                }
                else
                {
                    day = Descriptor.DaysOfMonth.First();
                    mon++;
                }

                if (day != t || mon != tmon)
                {
                    if (mon > 12)
                    {
                        date = new DateTime(date.Year, 12, day, 0, 0, 0, date.Kind).AddMonths(mon - 12);
                    }
                    else
                    {
                        //这是为了避免从一个月移动时出现错误
                        //有 30 或 31 天到一个月更少。 导致实例化无效的日期时间。
                        var lDay = DateTime.DaysInMonth(date.Year, mon);
                        if (day <= lDay)
                        {
                            date = new(date.Year, mon, day, 0, 0, 0, date.Kind);
                        }
                        else
                        {
                            date = new DateTime(date.Year, mon, lDay, 0, 0, 0, date.Kind).AddDays(day - lDay);
                        }
                    }
                    continue;
                }
            }
            else if (dayOfWSpec && !dayOfMSpec)
            {
                // 获取星期几规则
                if (Descriptor.LastdayOfWeek)
                {
                    var dow = Descriptor.DaysOfWeek.First();

                    var cDow = (int)date.DayOfWeek + 1;
                    var daysToAdd = 0;

                    if (cDow < dow)
                        daysToAdd = dow - cDow;

                    else if (cDow > dow)
                        daysToAdd = dow + (7 - cDow);

                    var lDay = DateTime.DaysInMonth(date.Year, mon);

                    if (day + daysToAdd > lDay)
                    {
                        if (mon == 12)
                            date = new DateTime(date.Year, mon - 11, 1, 0, 0, 0, date.Kind).AddYears(1);
                        else
                            date = new(date.Year, mon + 1, 1, 0, 0, 0, date.Kind);

                        continue;
                    }

                    // 查找本月这一天最后一次出现的日期...
                    while (day + daysToAdd + 7 <= lDay)
                    {
                        daysToAdd += 7;
                    }

                    day += daysToAdd;

                    if (daysToAdd > 0)
                    {
                        date = new(date.Year, mon, day, 0, 0, 0, date.Kind);
                        continue;
                    }
                }
                else if (Descriptor.NthdayOfWeek != 0)
                {
                    var dow = Descriptor.DaysOfWeek.First();

                    var cDow = (int)date.DayOfWeek + 1;
                    var daysToAdd = 0;

                    if (cDow < dow)
                        daysToAdd = dow - cDow;

                    else if (cDow > dow)
                        daysToAdd = dow + (7 - cDow);

                    var dayShifted = daysToAdd > 0;

                    day += daysToAdd;
                    var weekOfMonth = day / 7;

                    if (day % 7 > 0)
                        weekOfMonth++;

                    daysToAdd = (Descriptor.NthdayOfWeek - weekOfMonth) * 7;
                    day += daysToAdd;

                    if (daysToAdd < 0 || day > DateTime.DaysInMonth(date.Year, mon))
                    {
                        if (mon == 12)
                            date = new DateTime(date.Year, mon - 11, 1, 0, 0, 0, date.Kind).AddYears(1);
                        else
                            date = new(date.Year, mon + 1, 1, 0, 0, 0, date.Kind);

                        continue;
                    }

                    if (daysToAdd > 0 || dayShifted)
                    {
                        date = new(date.Year, mon, day, 0, 0, 0, date.Kind);
                        continue;
                    }
                }
                else if (Descriptor.EveryNthWeek != 0)
                {
                    var cDow = (int)date.DayOfWeek + 1;
                    var dow = Descriptor.DaysOfWeek.First();

                    set = Descriptor.DaysOfWeek.GetViewBetween(cDow, 9999999);
                    if (set.Count > 0)
                        dow = set.First();

                    var daysToAdd = 0;
                    if (cDow < dow)
                        daysToAdd = dow - cDow + 7 * (Descriptor.EveryNthWeek - 1);

                    else if (cDow > dow)
                        daysToAdd = dow + (7 - cDow) + 7 * (Descriptor.EveryNthWeek - 1);

                    if (daysToAdd > 0)
                    {
                        date = new(date.Year, mon, day, 0, 0, 0, date.Kind);
                        date = date.AddDays(daysToAdd);
                        continue;
                    }
                }
                else
                {
                    var cDow = (int)date.DayOfWeek + 1;
                    var dow = Descriptor.DaysOfWeek.First();

                    set = Descriptor.DaysOfWeek.GetViewBetween(cDow, 9999999);
                    if (set.Count > 0)
                        dow = set.First();

                    var daysToAdd = 0;

                    if (cDow < dow)
                        daysToAdd = dow - cDow;

                    else if (cDow > dow)
                        daysToAdd = dow + (7 - cDow);

                    var lDay = DateTime.DaysInMonth(date.Year, mon);

                    if (day + daysToAdd > lDay)
                    {
                        if (mon == 12)
                            date = new DateTime(date.Year, mon - 11, 1, 0, 0, 0, date.Kind).AddYears(1);
                        else
                            date = new(date.Year, mon + 1, 1, 0, 0, 0, date.Kind);

                        continue;
                    }

                    if (daysToAdd > 0)
                    {
                        date = new(date.Year, mon, day + daysToAdd, 0, 0, 0, date.Kind);
                        continue;
                    }
                }
            }
            else
            {
                throw new FormatException("不支持同时指定星期日和月日参数。");
            }

            date = date.WithoutMillisecond(newDay: day);
            mon = date.Month;

            var year = date.Year;
            t = -1;

            if (year > GetMaxYear(date))
                return null;

            set = Descriptor.Months.GetViewBetween(mon, 9999999);
            if (set.Count > 0)
            {
                t = mon;
                mon = set.First();
            }
            else
            {
                mon = Descriptor.Months.First();
                year++;
            }

            if (mon != t)
            {
                date = new(year, mon, 1, 0, 0, 0, date.Kind);
                continue;
            }

            date = date.WithoutMillisecond(newMonth: mon);
            year = date.Year;
            t = -1;

            set = Descriptor.Years.GetViewBetween(year, 9999999);
            if (set.Count > 0)
            {
                t = year;
                year = set.First();
            }
            else
            {
                return null;
            }

            if (year != t)
            {
                date = new(year, 1, 1, 0, 0, 0, date.Kind);
                continue;
            }

            date = date.WithoutMillisecond(newYear: year);

            gotOne = true;
        }

        return date;
    }


    private static DateTime SetCalendarHour(DateTime date, int hour)
    {
        var hourToSet = hour;

        if (hourToSet == 24)
            hourToSet = 0;

        var hourDate = date.With(newHour: hourToSet);

        if (hour == 24)
            hourDate = hourDate.AddDays(1);

        return hourDate;
    }

}
