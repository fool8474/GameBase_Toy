using System;
using Script.Inject;
using UniRx;
using UnityEngine;

namespace Script.UI
{
    public class ScrollItem : MonoBehaviour, IDisposable
    {
        protected CompositeDisposable _disposable;

        public virtual void Init(ScrollData data)
        {
            _disposable = new CompositeDisposable();
        }
        
        public virtual void UpdateData(ScrollData data)
        {
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}