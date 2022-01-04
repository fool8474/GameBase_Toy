using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Inject;
using Script.Manager;
using Script.Manager.Util.Log;
using UniRx;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;

namespace Script.UI
{
    public interface IView
    {
        void Inject(Model model);
        void Initialize();
        void SetCanvas();
        UniTask ShowUIVisibleAnimation(bool isVisible);
        UniTask SetVisible(bool isVisible, bool immediate);
    }
    
    public class View<TModel> : MonoBehaviour, IView, IDisposable where TModel : Model
    {
        protected TModel _model;
        protected UIAnimDirector _director;

        protected Dictionary<KeyCode, Action> _inputDictionary;

        protected ObjPoolMgr _objPoolMgr;
        private CanvasMgr _canvasMgr;

        protected CompositeDisposable _disposable = new CompositeDisposable();

        public virtual void Inject(Model model)
        {
            _model = model as TModel;
            
            _director = GetComponentInChildren<UIAnimDirector>();
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
            _canvasMgr = Injector.GetInstance<CanvasMgr>();
        }
        
        public virtual void Initialize()
        {
            _inputDictionary = new Dictionary<KeyCode, Action>();
            
            _director.InitView();
            
            _model.VisibleEvent
                .Subscribe(VisibleEvent)
                .AddTo(_disposable);
        }

        protected void Update()
        {
            InputCheck();
        }

        private void InputCheck()
        {
            foreach (var kvp in _inputDictionary)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    kvp.Value.Invoke();
                }
            }
        }

        protected void AddInputKey(KeyCode key, Action action, bool isOverWrite = false)
        {
            if (_inputDictionary.ContainsKey(key))
            {
                if (isOverWrite)
                {
                    _inputDictionary.Remove(key);
                    _inputDictionary.Add(key, action);
                }

                else
                {
                    Log.DF(LogCategory.INPUT, "Key {0} for {1} is already valid", key.ToString(), gameObject);                    
                }

                return;
            }
            
            _inputDictionary.Add(key, action);
        }

        protected virtual async void VisibleEvent(bool isVisible)
        {
            _model.IsVisible = isVisible;

            if (isVisible)
            {
                await InitializeWithVisible();
            }

            else
            {
                FinalizeHide();
            }
        }
        
        protected virtual async UniTask InitializeWithVisible()
        {
            SetCanvas();
        }

        protected virtual void FinalizeHide()
        {
            _model.FinalizeView.Execute();
            _objPoolMgr.ReturnObject(_model.UIData.Id, gameObject);
        }
        
        public async UniTask SetVisible(bool isVisible, bool isImmediate = false)
        {
            if (_director == null || _model.IsVisible == isVisible)
            {
                return;
            }

            if (isVisible)
            {
                _director.InitView();
                _model.VisibleEvent.Execute(true);

                if (isImmediate == false)
                {
                    await _director.SetVisible(true);
                }
            }

            else
            {
                if (isImmediate == false)
                {
                    await _director.SetVisible(false);
                }
                
                _model.VisibleEvent.Execute(false);
            }
        }

        public async UniTask ShowUIVisibleAnimation(bool isVisible)
        {
            await _director.SetVisible(isVisible);
        }

        public void SetCanvas()
        {
            _canvasMgr.MoveObjToCanvas(_model.UIData.UIType, gameObject);
        }
        
        public virtual void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
