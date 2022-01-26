using System;
using System.Collections.Generic;
using UniRx;

namespace Script.Event
{
    // 외부에서의 접근은 IEventProperty를 통해서만
    public interface IEventProperty<T>
    {
        T Value { get; set; }
        void SetValueAndForceNotify(T value);
        IDisposable Subscribe(Action<T> action, bool allowSameEvent = false);
    }
    
    // 해당 EventProperty의 Value가 변경될 때 subscriber들에게 broadCast
    public class EventProperty<T> : IEventProperty<T>, ISubscribable<T>, IDisposable
    {
        private T _value;

        private readonly List<Action<T>> _eventList;
        private bool _isDisposed;
        
        // 값 변경 시 이벤트 발생
        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                {
                    return;
                }
                
                _value = value;
                if (_isDisposed)
                {
                    return;
                }
                    
                OnRaiseEvent(_value);
            }
        }

        public EventProperty(T value = default)
        {
            _value = value;
            _eventList = new List<Action<T>>();
        }
        
        public event Action<T> RaiseEvent
        {
            add
            {
                _eventList.Add(value);
                OnRaiseEvent(_value);
            }

            remove => _eventList.Remove(value);
        }

        private void OnRaiseEvent(T value)
        {
            foreach(var currEvent in _eventList)
            {
                currEvent.Invoke(value);
            }
        }

        // Value를 변경하나 event 발생 X
        public void SetValueAndIgnoreNotify(T value)
        {
            _value = value;

            if (_isDisposed)
            {
                return;
            }
        }
        
        // Value가 변경되지 않더라도 event 발생
        public void SetValueAndForceNotify(T value)
        {
            _value = value;

            if (_isDisposed)
            {
                return;
            }
            
            OnRaiseEvent(value);
        }

        public IDisposable Subscribe(Action<T> action, bool allowSameEvent)
        {
            if (_isDisposed)
            {
                return Disposable.Empty;
            }

            if (allowSameEvent == false && _eventList.Contains(action))
            {
                return Disposable.Empty;
            }
            
            _eventList.Add(action);

            return new Subscription(this, action);
        }
        
        public void Dispose()
        {
            _value = default;

            for (var i = 0; i < _eventList.Count; i++)
            {
                _eventList[i] = null;
            }
            
            _eventList.RemoveRange(0, _eventList.Count);
            _isDisposed = true;
        }

        // 개별 dispose를 위해 필요
        private class Subscription : IDisposable
        {
            private readonly EventProperty<T> _parent;
            private readonly Action<T> _action;

            public Subscription(EventProperty<T> parent, Action<T> action)
            {
                _parent = parent;
                _action = action;
            }

            public void Dispose()
            {
                _parent.RaiseEvent -= _action;
            }
        }
    }
}
