#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义实现 <see cref="AbstractFluent{TSelf, TChain}"/> 的流畅 URI。
/// </summary>
public class FluentUrl : AbstractFluent<FluentUrl, Uri>, IEquatable<FluentUrl>
{
    /// <summary>
    /// 定义 URI 路径界定符。
    /// </summary>
    public const char PathDelimiter = '/';

    /// <summary>
    /// 定义 URI 查询参数界定符。
    /// </summary>
    public const char QueryDelimiter = '?';

    /// <summary>
    /// 定义 URI 查询参数键值对分隔符。
    /// </summary>
    public const char QueryPairSeparator = '=';

    /// <summary>
    /// 定义 URI 查询参数键值对集合界定符。
    /// </summary>
    public const char QueryPairsDelimiter = '&';

    /// <summary>
    /// 定义 URI 锚点片段界定符。
    /// </summary>
    public const char FragmentDelimiter = '#';


    private UriBuilder _builder;
    private ConcurrentDictionary<string, string>? _queries;


    /// <summary>
    /// 构造一个 <see cref="FluentUrl"/> 类的新实例。
    /// </summary>
    /// <param name="initialUri">给定的初始化 URI。</param>
    public FluentUrl(Uri initialUri)
        : this(initialUri, queries: null)
    {
        EditQuery(QueryString);
    }

    /// <summary>
    /// 构造一个 <see cref="FluentUrl"/> 类的新实例。
    /// </summary>
    /// <param name="initialUri">给定的初始化 URI。</param>
    /// <param name="queries">给定的查询参数集合。</param>
    /// <exception cref="UriFormatException">
    /// 给定的初始化 URI 不是有效的 URI。
    /// </exception>
    protected FluentUrl(Uri initialUri, ConcurrentDictionary<string, string>? queries)
        : base(initialUri)
    {
        _builder = new UriBuilder(initialUri);
        _queries = queries;
    }


    /// <summary>
    /// 获取 URI 方案名称。
    /// </summary>
    public string Scheme
    {
        get => _builder.Scheme;
        private set => _builder.Scheme = value;
    }

    /// <summary>
    /// 获取 URI 用户名。
    /// </summary>
    public string UserName
    {
        get => _builder.UserName;
        private set => _builder.UserName = value;
    }

    /// <summary>
    /// 获取 URI 密码。
    /// </summary>
    public string Password
    {
        get => _builder.Password;
        private set => _builder.Password = value;
    }

    /// <summary>
    /// 获取 URI 主机。
    /// </summary>
    public string Host
    {
        get => _builder.Host;
        private set => _builder.Host = value;
    }

    /// <summary>
    /// 获取 URI 端口。
    /// </summary>
    public int Port
    {
        get => _builder.Port;
        private set => _builder.Port = value;
    }

    /// <summary>
    /// 获取 URI 路径。
    /// </summary>
    public string Path
    {
        get => _builder.Path;
        private set => _builder.Path = value;
    }

    /// <summary>
    /// 获取 URI 查询字符串。
    /// </summary>
    public string QueryString
    {
        get => _builder.Query;
        private set => _builder.Query = value;
    }

    /// <summary>
    /// 获取 URI 锚点片段。
    /// </summary>
    public string Fragment
    {
        get => _builder.Fragment;
        private set => _builder.Fragment = value;
    }


    /// <summary>
    /// 获取当前 URI 查询参数键集合。
    /// </summary>
    /// <value>
    /// 返回 <see cref="ReadOnlyCollection{String}"/> 类型实例。
    /// </value>
    public ICollection<string>? QueryKeys => _queries?.Keys;

    /// <summary>
    /// 获取当前 URI 查询参数值集合。
    /// </summary>
    /// <value>
    /// 返回 <see cref="ReadOnlyCollection{String}"/> 类型实例。
    /// </value>
    public ICollection<string>? QueryValues => _queries?.Values;


    /// <summary>
    /// 获取处理方法失败的动作。
    /// </summary>
    /// <remarks>
    /// 传入参数依次为异常实例、当前重试次数和当前重试间隔。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="Action{Exception}"/> 类型实例。
    /// </value>
    public Action<Exception, int, TimeSpan?>? HandleFailAction { private get; set; }


    /// <summary>
    /// 获取当前 URI 指定查询参数的值。
    /// </summary>
    /// <param name="key">给定的查询参数键。</param>
    /// <returns>返回字符串。</returns>
    public string? GetQueryValue(string key)
        => _queries?[key];

    /// <summary>
    /// 获取当前 URI 必要的指定查询参数的值。
    /// </summary>
    /// <param name="key">给定的查询参数键。</param>
    /// <returns>返回字符串。</returns>
    /// <exception cref="InvalidOperationException">
    /// The current URI does not contain query parameters.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// The current URI does not contain the query parameter key.
    /// </exception>
    public string GetRequiredQueryValue(string key)
    {
        if (_queries is null)
        {
            throw new InvalidOperationException($"The current URI '{CurrentValue}' does not contain query parameters.");
        }

        if (!_queries.TryGetValue(key, out var value))
        {
            throw new KeyNotFoundException($"The current URI '{CurrentValue}' does not contain the query parameter '{key}'.");
        }

        return value;
    }


    /// <summary>
    /// 设置方案名称。
    /// </summary>
    /// <param name="newScheme">给定的新方案名称。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetScheme(string newScheme)
        => Switch(builder => builder.Scheme = newScheme);

    /// <summary>
    /// 设置用户名。
    /// </summary>
    /// <param name="newUserName">给定的新用户名。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetUserName(string newUserName)
        => Switch(builder => builder.UserName = newUserName);

    /// <summary>
    /// 设置密码。
    /// </summary>
    /// <param name="newPassword">给定的新密码。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetPassword(string newPassword)
        => Switch(builder => builder.Password = newPassword);

    /// <summary>
    /// 设置主机。
    /// </summary>
    /// <param name="newHost">给定的新主机。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetHost(string newHost)
        => Switch(builder => builder.Host = newHost);

    /// <summary>
    /// 设置端口。
    /// </summary>
    /// <param name="newPort">给定的新端口。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetPort(int newPort)
        => Switch(builder => builder.Port = newPort);

    /// <summary>
    /// 设置路径。
    /// </summary>
    /// <param name="newPath">给定的新路径。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetPath(string newPath)
        => Switch(builder => builder.Path = newPath.Leading(PathDelimiter));

    /// <summary>
    /// 设置查询参数字符串。
    /// </summary>
    /// <remarks>
    /// 此方法会清除当前已缓存的查询参数（如果有），并重新解析并缓存给定的查询参数字符串。
    /// </remarks>
    /// <param name="newQueryString">给定的新查询参数字符串。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetQueryString(string newQueryString)
    {
        // 保持与缓存的查询参数集合一致
        ClearQuery();

        EditQuery(newQueryString);

        FlushQuery();

        return this;
    }

    /// <summary>
    /// 设置锚点片段。
    /// </summary>
    /// <param name="newFragment">给定的新锚点片段。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetFragment(string newFragment)
        => Switch(builder => builder.Fragment = newFragment.Leading(FragmentDelimiter));

    /// <summary>
    /// 切换当前 URI。
    /// </summary>
    /// <param name="newUriAction">给定设置新 URI 的构建器动作。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    protected virtual FluentUrl Switch(Action<UriBuilder> newUriAction)
    {
        base.Switch(fluent =>
        {
            newUriAction(_builder);
            return _builder.Uri;
        });

        return this;
    }


    #region Query

    /// <summary>
    /// 清除查询参数集合。
    /// </summary>
    /// <remarks>
    /// 此方法仅会清除缓存的查询参数，不会同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>；如需同步刷新，请调用 <see cref="FlushQuery"/> 方法。
    /// </remarks>
    /// <returns>返回当前 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl ClearQuery()
        => EditQuery(queries => queries.Clear());


    /// <summary>
    /// 将当前 URI 的查询参数集合同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>。
    /// </summary>
    /// <returns>返回当前 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl FlushQuery()
    {
        string newQueryString;

        if (_queries is null || _queries.IsEmpty)
        {
            newQueryString = string.Empty;
        }
        else
        {
            newQueryString = string.Join(QueryPairsDelimiter,
                _queries.Select(pair => $"{pair.Key}{QueryPairSeparator}{pair.Value}"));
        }

        return Switch(builder => builder.Query = newQueryString.Leading(QueryDelimiter));
    }


    /// <summary>
    /// 编辑查询参数。
    /// </summary>
    /// <remarks>
    /// 此方法仅会添加或更新缓存的查询参数，不会同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>；如需同步刷新，请调用 <see cref="FlushQuery"/> 方法。
    /// </remarks>
    /// <param name="key">给定的参数名称。</param>
    /// <param name="value">给定的参数值。</param>
    /// <param name="updateValueFactory">如果指定 <paramref name="key"/> 存在的更新参数值工厂方法（可选；默认直接更新为 <paramref name="value"/>）。</param>
    /// <returns>返回当前 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl EditQuery(string key, string value,
        Func<string, string, string>? updateValueFactory = null)
    {
        updateValueFactory ??= (k, v) => value;

        return EditQuery(queries => queries.AddOrUpdate(key, value, updateValueFactory));
    }

    /// <summary>
    /// 编辑查询参数集合。
    /// </summary>
    /// <remarks>
    /// 此方法仅会添加或更新缓存的查询参数集合，不会同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>；如需同步刷新，请调用 <see cref="FlushQuery"/> 方法。
    /// </remarks>
    /// <param name="values">给定的查询参数对象（支持如“key1=value1&amp;key2=value2”形式）。</param>
    /// <returns>返回当前 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl EditQuery(string? values)
    {
        if (string.IsNullOrWhiteSpace(values)) return this;

        values = values.TrimStart(QueryDelimiter);

        if (values.Length <= 0) return this;

        return EditQuery(queries =>
        {
            var pairs = values.Split(QueryPairsDelimiter);
            for (var i = 0; i < pairs.Length; i++)
            {
                var pair = pairs[i].Split(QueryPairSeparator);
                var value = pair.Length > 1 ? pair[1] : string.Empty;

                queries.AddOrUpdate(pair[0], value, (k, v) => value);
            }
        });
    }

    /// <summary>
    /// 编辑查询参数集合。
    /// </summary>
    /// <remarks>
    /// 此方法仅会添加或更新缓存的查询参数集合，不会同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>；如需同步刷新，请调用 <see cref="FlushQuery"/> 方法。
    /// </remarks>
    /// <param name="values">给定的查询参数对象（支持如“new { key = value }”形式）。</param>
    /// <returns>返回当前 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl EditQuery(object values)
    {
        return EditQuery(queries =>
        {
            var props = TypeDescriptor.GetProperties(values);
            foreach (PropertyDescriptor prop in props)
            {
                var val = prop.GetValue(values)?.ToString() ?? string.Empty;

                queries.AddOrUpdate(prop.Name, val, (k, v) => val);
            }
        });
    }

    /// <summary>
    /// 编辑查询参数集合。
    /// </summary>
    /// <remarks>
    /// 此方法仅会添加或更新缓存的查询参数集合，不会同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>；如需同步刷新，请调用 <see cref="FlushQuery"/> 方法。
    /// </remarks>
    /// <param name="editAction">给定的编辑动作。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    protected virtual FluentUrl EditQuery(Action<ConcurrentDictionary<string, string>> editAction)
    {
        _queries ??= new();

        editAction(_queries);

        return this;
    }


    /// <summary>
    /// 移除查询参数集合。
    /// </summary>
    /// <remarks>
    /// 此方法仅会移除缓存的查询参数集合，不会同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>；如需同步刷新，请调用 <see cref="FlushQuery"/> 方法。
    /// </remarks>
    /// <param name="keys">给定的键集合。</param>
    /// <returns>返回当前 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl RemoveQuery(params string[] keys)
    {
        if (_queries is not null)
        {
            for (var i = 0; i < keys.Length; i++)
            {
                _queries.Remove(keys[i], out _);
            }
        }

        return this;
    }

    /// <summary>
    /// 移除查询参数。
    /// </summary>
    /// <remarks>
    /// 此方法仅会移除缓存的查询参数，不会同步刷新到 <see cref="QueryString"/> 与 <see cref="AbstractFluent{TSelf, TChain}.CurrentValue"/>；如需同步刷新，请调用 <see cref="FlushQuery"/> 方法。
    /// </remarks>
    /// <returns>返回当前 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl RemoveQuery(string key, [MaybeNullWhen(false)] out string? value)
    {
        if (_queries?.Remove(key, out value) == true)
        {
            return this;
        }

        value = null;
        return this;
    }

    #endregion


    #region Handle

    /// <summary>
    /// 设置处理失败的动作。
    /// </summary>
    /// <param name="handleFailAction">给定处理失败的动作。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public virtual FluentUrl SetHandleFail(Action<Exception, int, TimeSpan?> handleFailAction)
    {
        HandleFailAction = handleFailAction;
        return this;
    }


    /// <summary>
    /// 处理 URI 动作。
    /// </summary>
    /// <remarks>
    /// 支持发生异常时重试，重试次数由 <paramref name="failRetries"/> 参数指定，还可手动指定单次重试间隔及是否逐次递增重试间隔。
    /// </remarks>
    /// <param name="action">给定的处理动作。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    public virtual void Handle(Action<Uri> action,
        int failRetries = 0, TimeSpan? failRetryInterval = null,
        bool failRetryIntervalIncrement = true)
    {
        var uri = CurrentValue;

        var currentFailRetries = 0;
        var currentFailRetryInterval = failRetryInterval;

        LocalHandle();


        void LocalHandle()
        {
            while (currentFailRetries < failRetries)
            {
                ++currentFailRetries;

                try
                {
                    action(uri);
                }
                catch (Exception ex)
                {
                    if (currentFailRetryInterval.HasValue)
                    {
                        if (currentFailRetries > 1 && failRetryIntervalIncrement)
                        {
                            currentFailRetryInterval = currentFailRetryInterval.Value.Add(failRetryInterval!.Value);
                        }

                        Thread.Sleep(currentFailRetryInterval.Value);
                    }

                    HandleFailAction?.Invoke(ex, currentFailRetries, currentFailRetryInterval);

                    LocalHandle();
                }
            }
        }
    }

    /// <summary>
    /// 异步处理 URI 动作。
    /// </summary>
    /// <remarks>
    /// 支持发生异常时重试，重试次数由 <paramref name="failRetries"/> 参数指定，还可手动指定单次重试间隔及是否逐次递增重试间隔。
    /// </remarks>
    /// <param name="action">给定的处理动作。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual async Task HandleAsync(Action<Uri> action,
        int failRetries = 0, TimeSpan? failRetryInterval = null,
        bool failRetryIntervalIncrement = true)
    {
        var uri = CurrentValue;

        var currentFailRetries = 0;
        var currentFailRetryInterval = failRetryInterval;

        await LocalHandleAsync().ConfigureAwait(false);


        async Task LocalHandleAsync()
        {
            while (currentFailRetries < failRetries)
            {
                ++currentFailRetries;

                try
                {
                    action(uri);
                }
                catch (Exception ex)
                {
                    if (currentFailRetryInterval.HasValue)
                    {
                        if (currentFailRetries > 1 && failRetryIntervalIncrement)
                        {
                            currentFailRetryInterval = currentFailRetryInterval.Value.Add(failRetryInterval!.Value);
                        }

                        await Task.Delay(currentFailRetryInterval.Value).ConfigureAwait(false);
                    }

                    HandleFailAction?.Invoke(ex, currentFailRetries, currentFailRetryInterval);

                    await LocalHandleAsync().ConfigureAwait(false);
                }
            }
        }
    }


    /// <summary>
    /// 处理 URI 返回结果。
    /// </summary>
    /// <remarks>
    /// 支持发生异常时重试，重试次数由 <paramref name="failRetries"/> 参数指定，还可手动指定单次重试间隔及是否逐次递增重试间隔。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定处理返回结果的方法。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public virtual TResult? HandleResult<TResult>(Func<Uri, TResult> func,
        int failRetries = 0, TimeSpan? failRetryInterval = null,
        bool failRetryIntervalIncrement = true)
    {
        var uri = CurrentValue;

        var currentFailRetries = 0;
        var currentFailRetryInterval = failRetryInterval;

        var result = LocalHandleResult();

        return result;


        TResult? LocalHandleResult()
        {
            while (currentFailRetries < failRetries)
            {
                ++currentFailRetries;

                try
                {
                    return func(uri);
                }
                catch (Exception ex)
                {
                    if (currentFailRetries != failRetries && failRetryInterval.HasValue)
                    {
                        if (currentFailRetries > 1 && failRetryIntervalIncrement)
                        {
                            failRetryInterval = failRetryInterval.Value.Add(failRetryInterval.Value);
                        }

                        Thread.Sleep(failRetryInterval.Value);
                    }

                    HandleFailAction?.Invoke(ex, currentFailRetries, failRetryInterval);

                    return LocalHandleResult();
                }
            }

            return default;
        }
    }

    /// <summary>
    /// 异步处理 URI 返回结果。
    /// </summary>
    /// <remarks>
    /// 支持发生异常时重试，重试次数由 <paramref name="failRetries"/> 参数指定，还可手动指定单次重试间隔及是否逐次递增重试间隔。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定处理返回结果的方法。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public virtual async Task<TResult?> HandleResultAsync<TResult>(Func<Uri, Task<TResult>> func,
        int failRetries = 0, TimeSpan? failRetryInterval = null,
        bool failRetryIntervalIncrement = true)
    {
        var uri = CurrentValue;

        var currentFailRetries = 0;
        var currentFailRetryInterval = failRetryInterval;

        var result = await LocalHandleResultAsync().ConfigureAwait(false);

        return result;


        async Task<TResult?> LocalHandleResultAsync()
        {
            while (currentFailRetries < failRetries)
            {
                ++currentFailRetries;

                try
                {
                    return await func(uri).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    if (currentFailRetryInterval.HasValue)
                    {
                        if (currentFailRetries > 1 && failRetryIntervalIncrement)
                        {
                            currentFailRetryInterval = currentFailRetryInterval.Value.Add(failRetryInterval!.Value);
                        }

                        await Task.Delay(currentFailRetryInterval.Value).ConfigureAwait(false);
                    }

                    HandleFailAction?.Invoke(ex, currentFailRetries, currentFailRetryInterval);

                    return await LocalHandleResultAsync().ConfigureAwait(false);
                }
            }

            return default;
        }
    }

    #endregion


    /// <summary>
    /// 创建一个当前流畅 URI 的副本。
    /// </summary>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    protected override FluentUrl Create()
        => new(CurrentValue, _queries is null ? null : new(_queries));


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => CurrentValue.GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回当前 URI 值字符串。</returns>
    public override string ToString()
        => CurrentValue.ToString();


    /// <summary>
    /// 相等的流畅 URI。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as FluentUrl);

    /// <summary>
    /// 相等的流畅 URI。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="FluentUrl"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(FluentUrl? other)
        => CurrentValue.Equals(other?.CurrentValue);


    /// <summary>
    /// 将当前 <see cref="FluentUrl"/> 隐式转换为字符串形式。
    /// </summary>
    /// <param name="uri">给定的 <see cref="FluentUrl"/>。</param>
    public static implicit operator string(FluentUrl uri)
        => uri.CurrentValue.ToString();

}
