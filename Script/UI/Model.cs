using System;
using Script.Event;
using Script.Inject;
using UnityEngine;

namespace Script.UI
{
    public class Model : IDisposable, IInitialize
    {
        public UIData UIData;
        public bool IsVisible;

        public IEventCommand<bool> VisibleEvent => _visibleEvent;
        public IEventCommand FinalizeView => _finalizeView;

        private readonly EventCommand<bool> _visibleEvent = new EventCommand<bool>();
        private readonly EventCommand _finalizeView = new EventCommand();

        public virtual void Dispose()
        {
            UIData = null;

            _visibleEvent?.Dispose();
            _finalizeView?.Dispose();
        }

        public virtual void Initialize() {}
    }
}