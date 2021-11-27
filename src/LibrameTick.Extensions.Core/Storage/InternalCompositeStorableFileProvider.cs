#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Storage;

sealed class InternalCompositeStorableFileProvider : IStorableFileProvider
{
    private readonly IStorableFileProvider[] _providers;


    public InternalCompositeStorableFileProvider(IEnumerable<IStorableFileProvider> providers)
    {
        _providers = providers.ToArray();
    }


    #region Private

    private void ChainingProvidersByException(Action<IStorableFileProvider> action, int accessorIndex = 0)
    {
        try
        {
            action(_providers[accessorIndex]);
        }
        catch (Exception)
        {
            if (accessorIndex < _providers.Length - 1)
                ChainingProvidersByException(action, accessorIndex++); // 链式处理

            throw; // 所有访问器均出错时则抛出异常
        }
    }

    private TResult ChainingProvidersByException<TResult>(Func<IStorableFileProvider, TResult> func, int accessorIndex = 0)
    {
        try
        {
            return func(_providers[accessorIndex]);
        }
        catch (Exception)
        {
            if (accessorIndex < _providers.Length - 1)
                return ChainingProvidersByException(func, accessorIndex++); // 链式处理

            throw; // 所有访问器均出错时则抛出异常
        }
    }

    #endregion


    public IStorableFileInfo GetFileInfo(string subpath)
        => ChainingProvidersByException(p => p.GetFileInfo(subpath));

    public IStorableDirectoryContents GetDirectoryContents(string subpath)
        => ChainingProvidersByException(p => p.GetDirectoryContents(subpath));

    public IChangeToken Watch(string filter)
        => ChainingProvidersByException(p => p.Watch(filter));


    IFileInfo IFileProvider.GetFileInfo(string subpath)
        => GetFileInfo(subpath);

    IDirectoryContents IFileProvider.GetDirectoryContents(string subpath)
        => GetDirectoryContents(subpath);

}
