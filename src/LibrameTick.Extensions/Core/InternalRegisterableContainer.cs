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

class InternalRegisterableContainer : IRegisterableContainer
{
    private readonly ConcurrentDictionary<TypeNamedKey, IRegisterable> _instances;


    public InternalRegisterableContainer()
    {
        _instances = new();
    }


    public int Count
        => _instances.Count;

    public ICollection<TypeNamedKey> Keys
        => _instances.Keys;


    public bool ContainsKey(TypeNamedKey key)
        => _instances.ContainsKey(key);


    public TRegisterable Register<TRegisterable>(TypeNamedKey key, Func<TypeNamedKey, TRegisterable> addOrUpdateFunc)
        where TRegisterable : IRegisterable
        => (TRegisterable)_instances.AddOrUpdate(key, key => addOrUpdateFunc(key), (key, old) => addOrUpdateFunc(key));

    public TRegisterable Register<TRegisterable>(TypeNamedKey key, TRegisterable addOrUpdate)
        where TRegisterable : IRegisterable
        => (TRegisterable)_instances.AddOrUpdate(key, addOrUpdate, (key, old) => addOrUpdate);


    public TRegisterable Resolve<TRegisterable>(TypeNamedKey key, Func<TypeNamedKey, TRegisterable> initialFunc)
        where TRegisterable : IRegisterable
        => (TRegisterable)_instances.GetOrAdd(key, key => initialFunc(key));

    public TRegisterable Resolve<TRegisterable>(TypeNamedKey key, TRegisterable initial)
        where TRegisterable : IRegisterable
        => (TRegisterable)_instances.GetOrAdd(key, key => initial);


    public IEnumerator<KeyValuePair<TypeNamedKey, IRegisterable>> GetEnumerator()
        => _instances.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}
