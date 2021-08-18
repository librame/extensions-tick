#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
{
    sealed class InternalPropertyNotifier : IPropertyNotifier
    {
        private readonly ConcurrentDictionary<string, object> _propertyValues;
        private readonly ConcurrentDictionary<string, Func<object>> _propertyFuncs;

        
        public InternalPropertyNotifier(object sender, IPropertyNotifier? parentNotifier = null,
            ConcurrentDictionary<string, object>? propertyValues = null,
            ConcurrentDictionary<string, Func<object>>? propertyFuncs = null)
        {
            _propertyValues = propertyValues ?? new ConcurrentDictionary<string, object>();
            _propertyFuncs = propertyFuncs ?? new ConcurrentDictionary<string, Func<object>>();

            Sender = sender;
            SenderType = sender.GetType();
            ParentNotifier = parentNotifier;
        }


        public object Sender { get; init; }

        public Type SenderType { get; init; }


        public IPropertyNotifier? ParentNotifier { get; init; }


        public event PropertyChangingEventHandler? PropertyChanging;

        public event PropertyChangedEventHandler? PropertyChanged;


        private string BuildKey(string propertyName)
            => propertyName.Leading($"{SenderType.FullName}:");


        private void HandleNotice(string? propertyName,
            object? changingValue, object? oldValue, bool isUpdate, Action action)
        {
            // 调用属性改变时事件处理程序
            PropertyChanging?.Invoke(this,
                new NotifyPropertyChangingEventArgs(propertyName, changingValue, oldValue, isUpdate));

            action.Invoke();

            // 调用属性改变后事件处理程序
            PropertyChanged?.Invoke(this,
                new NotifyPropertyChangedEventArgs(propertyName, changingValue, oldValue, isUpdate));
        }


        public object AddOrUpdate(string propertyName, object propertyValue, Action<object>? addedOrUpdatedAction = null)
        {
            if (propertyValue == null)
                throw new ArgumentException($"The value of the property '{propertyName}' to be added cannot be null.");

            var key = BuildKey(propertyName);

            if (_propertyValues.ContainsKey(key))
            {
                var oldValue = _propertyValues[key];

                HandleNotice(propertyName, propertyValue, oldValue, isUpdate: true, () =>
                {
                    _propertyValues[key] = propertyValue;

                    addedOrUpdatedAction?.Invoke(propertyValue);
                });
            }
            else
            {
                HandleNotice(propertyName, propertyValue, oldValue: null, isUpdate: false, () =>
                {
                    _propertyValues.TryAdd(key, propertyValue);

                    addedOrUpdatedAction?.Invoke(propertyValue);
                });
            }

            return propertyValue;
        }

        public object AddOrUpdate(string propertyName, Func<object> propertyValueFunc, bool isInitializeValue = false,
            Action<object>? addedOrUpdatedAction = null)
        {
            if (propertyValueFunc == null)
                throw new ArgumentException($"The value func of the property '{propertyName}' to be added cannot be null.");

            var key = BuildKey(propertyName);

            if (!isInitializeValue)
            {
                if (_propertyFuncs.ContainsKey(key))
                {
                    var oldValueFunc = _propertyFuncs[key];

                    HandleNotice(propertyName, propertyValueFunc, oldValueFunc, isUpdate: true, () =>
                    {
                        _propertyFuncs[key] = propertyValueFunc;

                        addedOrUpdatedAction?.Invoke(propertyValueFunc);
                    });
                }
                else
                {
                    HandleNotice(propertyName, propertyValueFunc, oldValue: null, isUpdate: false, () =>
                    {
                        _propertyFuncs.TryAdd(key, propertyValueFunc);

                        addedOrUpdatedAction?.Invoke(propertyValueFunc);
                    });
                }

                return propertyValueFunc;
            }

            return AddOrUpdate(propertyName, propertyValueFunc.Invoke());
        }


        public TValue GetOrAdd<TValue>(string propertyName, TValue addPropertyValue,
            Action<TValue>? addedAction = null, Action<TValue>? gotAction = null)
        {
            var key = BuildKey(propertyName);

            if (_propertyValues.ContainsKey(key))
            {
                var currentValue = (TValue)_propertyValues[key];
                gotAction?.Invoke(currentValue);

                return currentValue;
            }

            if (_propertyFuncs.ContainsKey(key))
            {
                var currentValue = (TValue)_propertyFuncs[key].Invoke();
                gotAction?.Invoke(currentValue);

                return currentValue;
            }

            if (addPropertyValue == null)
                throw new ArgumentException($"The value of the property '{propertyName}' to be added cannot be null.");

            _propertyValues.TryAdd(key, addPropertyValue);

            addedAction?.Invoke(addPropertyValue);

            return addPropertyValue;
        }

        public object GetOrAdd(string propertyName, object addPropertyValue,
            Action<object>? addedAction = null, Action<object>? gotAction = null)
        {
            var key = BuildKey(propertyName);

            if (_propertyValues.ContainsKey(key))
            {
                var currentValue = _propertyValues[key];
                gotAction?.Invoke(currentValue);

                return currentValue;
            }

            if (_propertyFuncs.ContainsKey(key))
            {
                var currentValue = _propertyFuncs[key].Invoke();
                gotAction?.Invoke(currentValue);

                return currentValue;
            }

            if (addPropertyValue == null)
                throw new ArgumentException($"The value of the property '{propertyName}' to be added cannot be null.");

            _propertyValues.TryAdd(key, addPropertyValue);

            addedAction?.Invoke(addPropertyValue);

            return addPropertyValue;
        }


        public bool TryGetValue(string propertyName, [MaybeNullWhen(false)] out object propertyValue)
        {
            var key = BuildKey(propertyName);

            if (_propertyValues.TryGetValue(key, out propertyValue))
                return true;

            if (_propertyFuncs.TryGetValue(key, out var factory))
            {
                propertyValue = factory.Invoke();
                return true;
            }

            return false;
        }


        public bool TryRemoveValue(string propertyName, [MaybeNullWhen(false)] out object propertyValue)
        {
            var key = BuildKey(propertyName);

            if (_propertyValues.TryRemove(key, out propertyValue))
                return true;

            if (_propertyFuncs.TryRemove(key, out var factory))
            {
                propertyValue = factory.Invoke();
                return true;
            }

            return false;
        }


        public IPropertyNotifier WithSender(object newSender)
            => new InternalPropertyNotifier(newSender, this, _propertyValues, _propertyFuncs);

        //public IPropertyNotifier WithSender(Type newSenderType)
        //    => new InternalPropertyNotifier(newSenderType, this, _propertyValues, _propertyFuncs);

    }
}
