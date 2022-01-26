using System;

namespace Script.Event
{
    #region Event
    public interface IEventObservable
    {
        bool IsDisposed { get; }
    }

    public interface IEventObservable<out T>
    {
        bool IsDisposed { get; }
    }
    #endregion

    #region Subscribe
    public interface ISubscribable
    {
        IDisposable Subscribe(Action action, bool allowSameEvent = false);
    }

    public interface ISubscribable<out T>
    {
        IDisposable Subscribe(Action<T> action, bool allowSameEvent = false);
    }
    #endregion
}