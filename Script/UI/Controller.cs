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
    public interface IController
    {
        void Initialize();
        void InitializeWithVisible();
        void InitializeWithData();
        UniTask SetVisible(bool isVisible, bool immediate = false);
        void OnQuit();
        UIData GetUIData();
        UniTask ShowUIVisibleAnimation(bool isVisible);
        void SetInitData(UIInitData initData);
        bool IsVisible();
    }
    
    public class Controller<TModel> : IDisposable, IController where TModel : Model 
    {
        protected IView _view;
        protected TModel _model;

        protected string prefabId;
        
        protected ObjPoolMgr _objPoolMgr;
        protected UIMgr _uiMgr;

        protected readonly CompositeDisposable _disposable = new CompositeDisposable();
        protected readonly CompositeDisposable _viewDisposable = new CompositeDisposable();

        protected UIInitData _initData;
        private List<IController> _childUIList;
        private bool _completeVisible = true;

        protected Controller(string id, TModel model)
        {
            prefabId = id;
            _model = model;       
            
            Inject();
            Initialize();
        }

        public virtual void Initialize()
        {
            _childUIList = new List<IController>();
            
            _model.Initialize();
            
            _model.VisibleEvent
                    .Subscribe(isVisible =>
                    {
                        if (isVisible)
                        {
                            InitializeWithVisible();
                        }
                    }).AddTo(_disposable);

            _model.FinalizeView
                .Subscribe(OnQuit)
                .AddTo(_disposable);
        }

        private async UniTask InitPrefab()
        {
            _view = await _objPoolMgr.GetObject<IView>(prefabId);
            InitializeView();
        }
        
        private void InitializeView()
        {
            _view.Inject(_model);
            _view.Initialize();
        }

        protected virtual void Inject()
        {
            _uiMgr = Injector.GetInstance<UIMgr>();
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
        }
        
        public virtual async UniTask SetVisible(bool isVisible, bool immediate = false)
        {
            switch (isVisible)
            {
                case true:
                {
                    await ShowUI(true, immediate);
                    await SetVisibleChild(true, immediate);
                }
                break;
                case false:
                {
                    await SetVisibleChild(false, immediate);
                    await ShowUI(false, immediate);
                }
                break;
            }
        }
        
        private async UniTask SetVisibleChild(bool isVisible, bool immediate)
        {
            foreach (var _childUI in _childUIList)
            {
                await _childUI.SetVisible(isVisible, immediate);
            }
        }
        
        protected virtual async UniTask ShowUI(bool isVisible, bool immediate = false)
        {
            if (isVisible == _model.IsVisible || _completeVisible == false)
            {
                return;
            }

            _completeVisible = false;
            if (isVisible)
            {
                if (_view == null)
                {
                    if (_objPoolMgr.HaveObject(_model.UIData.Id) == false)
                    {
                        await InitPrefab();
                    }

                    else
                    {
                        _view = await _objPoolMgr.GetObject<IView>(prefabId);
                    }
                }
                
                AfterInitializeView();
                InitializeWithData();
                ClearInitData();
            }

            if (_view != null)
            {
                await _view.SetVisible(isVisible, immediate);
            }
            
            _completeVisible = true;
        }

        private void ClearInitData()
        {
            _initData = null;
        }
        
        public virtual void OnQuit()
        {
            _viewDisposable?.Dispose();
        }

        public async UniTask ShowUIVisibleAnimation(bool isVisible)
        {
            if (_view != null)
            {
                if (isVisible)
                {
                    await _view.ShowUIVisibleAnimation(true);
                    await ShowChildUIVisibleAnimation(true);
                }

                else
                {
                    await ShowChildUIVisibleAnimation(false);
                    await _view.ShowUIVisibleAnimation(false);
                }   
            }
        }

        private async UniTask ShowChildUIVisibleAnimation(bool isVisible)
        {
            foreach (var currChild in _childUIList)
            {
                await currChild.ShowUIVisibleAnimation(isVisible);
            }
        }

        public void AddChildUI<T>(T controller) where T : IController
        {
            _uiMgr.RegisterUI(controller);
            _childUIList.Add(controller);
        }

        public void SetInitData(UIInitData initData)
        {
            _initData = initData;
        }

        public UIData GetUIData()
        {
            return _model.UIData;
        }
        
        public bool IsVisible()
        {
            return _model.IsVisible;        
        }

        protected virtual void AfterInitializeView() { }
        public virtual void InitializeWithVisible() { }
        public virtual void InitializeWithData() { }
        public virtual void Dispose()
        {
            _disposable?.Dispose();
            _viewDisposable?.Dispose();
        }
    }
}