using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Inject;
using Script.Manager;
using Script.Manager.Util.Log;
using UniRx;
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
        protected CanvasMgr _canvasMgr;
        protected UIMgr _uiMgr;
        protected UITransitionMgr _uiTransitionMgr;

        protected FixedUISetter[] _fixedUISetters;

        protected CompositeDisposable _disposable = new CompositeDisposable();

        public virtual void Inject(Model model)
        {
            _model = model as TModel;
            
            _director = GetComponentInChildren<UIAnimDirector>();
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
            _canvasMgr = Injector.GetInstance<CanvasMgr>();
            _uiTransitionMgr = Injector.GetInstance<UITransitionMgr>();
            _uiMgr = Injector.GetInstance<UIMgr>();
        }
        
        public virtual void Initialize()
        {
            _inputDictionary = new Dictionary<KeyCode, Action>();
            _director.InitView();
            InitializeFixedUI();

            _model.VisibleEvent
                .Subscribe(VisibleEvent)
                .AddTo(_disposable);
        }
        
        private void InitializeFixedUI()
        {
            _fixedUISetters = GetComponents<FixedUISetter>();

            if(_fixedUISetters == null || _fixedUISetters.Length == 0)
            {
                return;
            }

            foreach(var fixedUISetter in _fixedUISetters)
            {
                fixedUISetter.Initialize();
            }
        }

        protected void Update()
        {
            InputCheck();
        }

        private void InputCheck()
        {
            if(_inputDictionary == null)
            {
                return;
            }

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
            gameObject.SetActive(isVisible);

            if (isVisible)
            {
                await InitializeWithVisible();
            }

            else
            {
                FinalizeHide();
            }
        }
        
        protected virtual UniTask InitializeWithVisible()
        {
            SetCanvas();
            return default;
        }

        protected virtual void FinalizeHide()
        {
            _uiMgr.ReturnView(gameObject);
            _model.FinalizeView.Execute();
        }

        private async UniTask SetVisibleFixedUI(bool isVisible, bool isImmediate)
        {
            if(_fixedUISetters == null || _fixedUISetters.Length == 0)
            {
                return;
            }

            foreach(var fixedUISetter in _fixedUISetters)
            {
                await fixedUISetter.ShowUIWithInitData(isVisible, isImmediate);
            }
        }

        public async UniTask SetVisible(bool isVisible, bool isImmediate = false)
        {
            if (_director == null || _model.IsVisible == isVisible)
            {
                return;
            }

            _uiTransitionMgr.RegisterAnimationUI(gameObject.GetHashCode());
            await SetVisibleFixedUI(isVisible, isImmediate);

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
            _uiTransitionMgr.FinishAnimationUI(gameObject.GetHashCode());
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
            _objPoolMgr.ReturnObject(_model.UIData.Id, gameObject);
            _disposable.Dispose();
        }
    }
}
