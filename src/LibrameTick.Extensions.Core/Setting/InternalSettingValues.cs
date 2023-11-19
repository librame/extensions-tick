#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Setting;

internal sealed class InternalSettingValues<TSettingRoot> : ISettingValues<TSettingRoot>
    where TSettingRoot : ISettingRoot
{
    private readonly ISettingProvider<TSettingRoot> _provider;

    private Lazy<TSettingRoot> _lazySetting;


    public InternalSettingValues(ISettingProvider<TSettingRoot> provider)
    {
        _provider = provider;

        _lazySetting = new(_provider.LoadOrSave);
    }


    public TSettingRoot GetSingletonValue()
        => _lazySetting.Value;

}
