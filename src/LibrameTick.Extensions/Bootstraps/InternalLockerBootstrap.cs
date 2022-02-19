#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Bootstraps;

class InternalLockerBootstrap : AbstsractBootstrap, ILockerBootstrap
{
    // 禁止只读限制
    private Lazy<SpinLock> _spinLock
        = new Lazy<SpinLock>(() => new SpinLock(enableThreadOwnerTracking: false));

    // 定义最大锁定器数（默认为处理器线程数）
    private readonly int _maxLockersCount = Environment.ProcessorCount;

    // 定义断定为死锁的持续时长（默认为 3 秒，表示持续 3 秒即为死锁）
    private readonly TimeSpan _deadlockDuration = TimeSpan.FromSeconds(3);

    // 定义锁定器队列
    private readonly List<object> _lockers = new();


    public void SpinLock(Action action)
    {
        var lockTaken = false;

        try
        {
            _spinLock.Value.Enter(ref lockTaken);
            action();
        }
        finally
        {
            if (lockTaken)
                _spinLock.Value.Exit(false);
        }
    }

    public TResult SpinLock<TResult>(Func<TResult> func)
    {
        var lockTaken = false;
        TResult result;

        try
        {
            _spinLock.Value.Enter(ref lockTaken);
            result = func();
        }
        finally
        {
            if (lockTaken)
                _spinLock.Value.Exit(false);
        }

        return result;
    }


    public void Lock(Action<int> action)
    {
        int index;

        lock (GetLocker(out index))
        {
            action(index);
        }
    }

    public TResult Lock<TResult>(Func<int, TResult> func)
    {
        int index;
        TResult result;

        lock (GetLocker(out index))
        {
            result = func(index);
        }

        return result;
    }


    private object GetLocker(out int index)
    {
        Stopwatch? stopwatch = null;

        while (true)
        {
            // 使用 _lockers.FirstOrDefault(obj => !Monitor.IsEntered(obj))
            // 可能会抛出 IndexOutOfRangeException 异常（特别是存在死锁的情形下）
            var locker = _lockers.Where(obj => !Monitor.IsEntered(obj)).FirstOrDefault();
            
            if (locker is not null)
            {
                index = _lockers.FindIndex(obj => ReferenceEquals(obj, locker));
                return locker;
            }

            if (_lockers.Count < _maxLockersCount)
                return AddLocker(out index);

            if (stopwatch is null)
                stopwatch = Stopwatch.StartNew();

            if (!stopwatch.IsRunning)
                stopwatch.Restart();

            if (stopwatch.Elapsed >= _deadlockDuration)
            {
                stopwatch.Stop();

                _lockers.ForEach(obj => Monitor.Exit(obj));
                _lockers.Clear();

                throw new OverflowException(string.Format(CultureInfo.InvariantCulture,
                    "The lockers is depleted, please check the code or modify the max lockers count '{0}'.",
                    _maxLockersCount));
            }
        }

        object AddLocker(out int index)
        {
            var locker = new object();
            _lockers.Add(locker);

            index = _lockers.Count - 1;
            return locker;
        }
    }


    protected override bool ReleaseManaged()
    {
        _lockers.Clear();
        return true;
    }

}
