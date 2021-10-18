#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Plugins;

/// <summary>
/// 定义抽象实现 <see cref="IPluginResolver"/> 的插件解析器。
/// </summary>
public abstract class AbstractPluginResolver : IPluginResolver
{
    private readonly PluginOptions _options;

    private List<IPluginInfo>? _infos;


    /// <summary>
    /// 构造一个 <see cref="AbstractPluginResolver"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="PluginOptions"/>。</param>
    protected AbstractPluginResolver(PluginOptions options)
    {
        _options = options;
    }


    /// <summary>
    /// 解析插件信息列表。
    /// </summary>
    /// <returns>返回 <see cref="IReadOnlyList{IPluginInfo}"/>。</returns>
    /// <exception cref="DllNotFoundException"></exception>
    public IReadOnlyList<IPluginInfo> ResolveInfos()
    {
        if (_infos is null)
        {
            var infoTypes = AssemblyLoader.LoadConcreteTypes(typeof(IPluginInfo), _options.AssemblyLoading);

            if (infoTypes is null || infoTypes.Length < 1)
                throw new DllNotFoundException($"The plugin assembly implementing {nameof(IPluginInfo)} was not found, please confirm that any plugin package is installed.");

            _infos = infoTypes.Select(s => (IPluginInfo)s.NewByExpression()).ToList();

            if (_infos.Count > 1)
                _infos.Sort();
        }

        return _infos;
    }

}
