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

internal class InternalSettingValues<TSetting> : ISettingValues<TSetting>
    where TSetting : ISetting
{
    private readonly ISettingProvider<TSetting> _provider;

    private Lazy<TSetting> _lazySetting;


    public InternalSettingValues(ISettingProvider<TSetting> provider)
    {
        _provider = provider;

        _lazySetting = new(_provider.LoadOrSave);
    }


    public TSetting GetSingletonValue()
        => _lazySetting.Value;

}
