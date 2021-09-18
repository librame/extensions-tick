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

class InternalInstanceContainer : IInstanceContainer
{
    private readonly ConcurrentDictionary<TypeNamedKey, object> _instances;


    public InternalInstanceContainer()
    {
        _instances = new();
    }


    public int Count
        => _instances.Count;

    public ICollection<TypeNamedKey> Keys
        => _instances.Keys;


    public bool ContainsKey(TypeNamedKey key)
        => _instances.ContainsKey(key);


    public TInstance Register<TInstance>(TypeNamedKey<TInstance> key, Func<TypeNamedKey, TInstance> instanceFunc)
    {
        var func = ToObjectFunc(instanceFunc);

        return (TInstance)_instances.AddOrUpdate(key, func, (key, old) => func.Invoke(key));
    }

    public object Register(TypeNamedKey key, Func<TypeNamedKey, object> instanceFunc)
        => _instances.AddOrUpdate(key, instanceFunc, (key, old) => instanceFunc.Invoke(key));


    public TInstance Register<TInstance>(TypeNamedKey<TInstance> key, TInstance instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        return (TInstance)_instances.AddOrUpdate(key, instance, (key, old) => instance);
    }

    public object Register(TypeNamedKey key, object instance)
        => _instances.AddOrUpdate(key, instance, (key, old) => instance);


    public TInstance Resolve<TInstance>(TypeNamedKey<TInstance> key, Func<TypeNamedKey, TInstance> instanceFunc)
        => (TInstance)_instances.GetOrAdd(key, ToObjectFunc(instanceFunc));

    public object Resolve(TypeNamedKey key, Func<TypeNamedKey, object> instanceFunc)
        => _instances.GetOrAdd(key, instanceFunc);


    public IEnumerator<KeyValuePair<TypeNamedKey, object>> GetEnumerator()
        => _instances.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    private Func<TypeNamedKey, object> ToObjectFunc<TInstance>(Func<TypeNamedKey, TInstance> instanceFunc)
    {
        return key =>
        {
            var instance = instanceFunc.Invoke(key);

            if (instance is null)
                throw new ArgumentException($"The result of the specified method '{nameof(instanceFunc)}' call is null.");

            return instance;
        };
    }

}
