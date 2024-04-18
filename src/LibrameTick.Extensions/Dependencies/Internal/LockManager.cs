#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies.Internal;

internal sealed class LockManager(int maxLockersCount, TimeSpan deadlockDuration) : ILockManager
{
    // 定义锁对象列表
    private readonly List<object> _lockers = new(maxLockersCount);


    #region Monitor

    public int MaxLockersCount { get; init; } = maxLockersCount;

    public TimeSpan DeadlockDuration { get; init; } = deadlockDuration;


    public void Lock(Action<int> action)
    {
        lock (GetLocker(out int index))
        {
            action(index);
        }
    }

    public TResult Lock<TResult>(Func<int, TResult> func)
    {
        TResult result;

        lock (GetLocker(out int index))
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
            // 使用 _lockers.FirstOrDefault(static obj => !Monitor.IsEntered(obj))
            // 可能会抛出 IndexOutOfRangeException 异常（特别是存在死锁的情形下）
            var locker = _lockers.Where(static obj => !Monitor.IsEntered(obj)).FirstOrDefault();

            if (locker is not null)
            {
                index = _lockers.FindIndex(obj => ReferenceEquals(obj, locker));
                return locker;
            }

            if (_lockers.Count < MaxLockersCount)
                return AddLocker(out index);

            if (stopwatch is null)
                stopwatch = Stopwatch.StartNew();

            if (!stopwatch.IsRunning)
                stopwatch.Restart();

            if (stopwatch.Elapsed >= DeadlockDuration)
            {
                _lockers.ForEach(Monitor.Exit);
                _lockers.Clear();

                stopwatch.Stop();

                throw new OverflowException($"The lockers is depleted, please check the code or modify the max lockers count '{MaxLockersCount}'.");
            }
        }

        // 添加锁对象
        object AddLocker(out int index)
        {
            var locker = new object();
            _lockers.Add(locker);

            index = _lockers.Count - 1;
            return locker;
        }
    }

    #endregion


    #region SpinLock

#pragma warning disable IDE0044 // 添加只读修饰符

    // 自旋虽然是值类型，但是内部状态会改变，因此不要声明为只读字段。
    private Lazy<SpinLock> _spinLock
        = new(static () => new SpinLock(enableThreadOwnerTracking: false));

#pragma warning restore IDE0044 // 添加只读修饰符


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

    #endregion


    public void Dispose()
    {
        _lockers.Clear();
    }

}
