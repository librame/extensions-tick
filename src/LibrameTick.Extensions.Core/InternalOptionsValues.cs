#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

internal class InternalOptionsValues<TOptions> : IOptionsValues<TOptions>
    where TOptions : class
{
    private readonly IOptions<TOptions> _options;
    private readonly IOptionsSnapshot<TOptions> _optionsSnapshot;
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;

    private Lazy<TOptions> _lazyOptions;
    private Lazy<TOptions> _lazyOptionsSnapshot;
    private Lazy<TOptions> _lazyOptionsMonitor;


    public InternalOptionsValues(IOptions<TOptions> options,
        IOptionsSnapshot<TOptions> optionsSnapshot,
        IOptionsMonitor<TOptions> optionsMonitor)
    {
        _options = options;
        _optionsSnapshot = optionsSnapshot;
        _optionsMonitor = optionsMonitor;

        _lazyOptions = new(() => _options.Value);
        _lazyOptionsSnapshot = new(() => _optionsSnapshot.Value);
        _lazyOptionsMonitor = new(() => _optionsMonitor.CurrentValue);
    }


    public TOptions GetSingletonValue()
        => _lazyOptions.Value;

    public TOptions GetScopeValue()
        => _lazyOptionsSnapshot.Value;

    public TOptions GetTransientValue()
        => _lazyOptionsMonitor.Value;

}
