using System;
using System.Collections.Generic;
using Script.Manager.Util.Log;
using UniRx;

namespace Script.Event
{
    // 외부에서의 접근은 IEventCommand를 통해서만
    public interface IEventCommand : IEventObservable
    {
        bool Execute();
    }

    // EventCommand를 이용해 특정 이벤트의 발생을 broadCast하여 외부에서의 행동을 촉진
    public sealed class EventCommand : IEventCommand, ISubscribable, IDisposable
    {
        public bool IsDisposed { get; private set; }
        private readonly List<Action> RaiseEventList;

        public EventCommand()
        {
            RaiseEventList = new List<Action>();
        }

        public bool Execute()
        {
            if (RaiseEventList == null || RaiseEventList?.Count == 0)
            {
                return false;
            }

            foreach (var RaiseEvent in RaiseEventList)
            {
                RaiseEvent.Invoke();
            }

            return true;
        }

        IDisposable ISubscribable.Subscribe(Action action, bool allowSameEvent)
        {
            if (IsDisposed)
            {
                return Disposable.Empty;
            }

            if (allowSameEvent == false && RaiseEventList.Contains(action))
            {
                return Disposable.Empty;
            }

            RaiseEventList.Add(action);
            return new Subscription(this, action);
        }

        public void Dispose()
        {
            for (var i = 0; i < RaiseEventList.Count; i++)
            {
                RaiseEventList[i] = null;
            }
            
            RaiseEventList.Clear();
            IsDisposed = true;
        }

        // 개별 dispose를 위해 필요
        private class Subscription : IDisposable
        {
            private readonly EventCommand _parent;
            private readonly Action _action;

            public Subscription(EventCommand parent, Action action)
            {
                _parent = parent;
                _action = action;
            }

            public void Dispose()
            {
                _parent.RaiseEventList.Remove(_action);
            }
        }
    }

    // 외부에서의 접근은 IEventCommand를 통해서만
    public interface IEventCommand<T> : IEventObservable<T>
    {
        bool Execute(T param);
    }

    // EventCommand를 이용해 특정 이벤트의 발생을 broadCast하여 외부에서의 행동을 촉진
    public sealed class EventCommand<T> : IEventCommand<T>, ISubscribable<T>, IDisposable
    {
        public bool IsDisposed { get; private set; }
        private readonly List<Action<T>> RaiseEventList;

        public EventCommand()
        {
            RaiseEventList = new List<Action<T>>();
        }

        public bool Execute(T param)
        {
            if (RaiseEventList == null || RaiseEventList?.Count == 0)
            {
                return false;
            }

            foreach (var RaiseEvent in RaiseEventList)
            {
                RaiseEvent.Invoke(param);
            }

            return true;
        }
        
        IDisposable ISubscribable<T>.Subscribe(Action<T> action, bool allowSameEvent)
        {
            if (IsDisposed)
            {
                return Disposable.Empty;
            }

            if (allowSameEvent == false && RaiseEventList.Contains(action))
            {
                return Disposable.Empty; 
            }
            
            RaiseEventList.Add(action);
            return new Subscription(this, action);
        }

        public void Dispose()
        {
            for (var i = 0; i < RaiseEventList.Count; i++)
            {
                RaiseEventList[i] = null;
            }
            
            RaiseEventList.Clear();
            IsDisposed = true;
        }

        // 개별 dispose를 위해 필요
        private class Subscription : IDisposable
        {
            private readonly EventCommand<T> _parent;
            private readonly Action<T> _action;

            public Subscription(EventCommand<T> parent, Action<T> action)
            {
                _parent = parent;
                _action = action;
            }

            public void Dispose()
            {
                _parent.RaiseEventList.Remove(_action);
            }
        }
    }
}