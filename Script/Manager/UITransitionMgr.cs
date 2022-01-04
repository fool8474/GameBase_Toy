using System;
using System.Collections.Generic;
using Script.Event;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.UI;
using Script.UI.Controllers;
using UniRx;

namespace Script.Manager
{
    public enum UIContentType
    {
        NONE,
        TEST1,
        TEST2,
        TEST3,
        TEST4,
        MAIN_GAME,
    }
    
    public class UITransitionMgr : ScriptMgr, IDisposable
    {
        private Dictionary<UIContentType, IController> _uiContentDic;
        private List<IController> _uiCallStack;  
        private IController _currController;
        
        private UIMgr _uiMgr;
        
        private bool _isEventBlock;
        private UIInitData _initData;
        
        #region Event
        public IEventCommand BackEvent => _backEvent;
        private readonly EventCommand _backEvent = new EventCommand();
        public IEventCommand<UIContentType> MoveEvent => _moveEvent;
        private readonly EventCommand<UIContentType> _moveEvent = new EventCommand<UIContentType>();
        
        public IEventCommand MoveFinished => _moveFinished;
        private readonly EventCommand _moveFinished = new EventCommand();

        public IEventCommand BackFinished => _backFinished;
        private readonly EventCommand _backFinished = new EventCommand();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        #endregion  

        public override void Initialize()
        {
            _uiContentDic = new Dictionary<UIContentType, IController>();
            _uiCallStack = new List<IController>();
            
            RegisterContent();

            BackEvent
                .Subscribe(DoBack)
                .AddTo(_disposable);

            MoveEvent
                .Subscribe(DoMove)
                .AddTo(_disposable);
        }
        
        public override void Inject()
        {
            _uiMgr = Injector.GetInstance<UIMgr>();
        }

        private void RegisterContent()
        {
            RegisterContent<TestController>(UIContentType.TEST1);
            RegisterContent<TestControllerPopup>(UIContentType.TEST2);
            RegisterContent<TestControllerPopup3>(UIContentType.TEST3);
            RegisterContent<TestHeavyController>(UIContentType.TEST4);
            RegisterContent<InGameUIController>(UIContentType.MAIN_GAME);
        }

        public UITransitionMgr SetInitData(UIInitData initData)
        {
            _initData = initData;
            return this;
        }
        
        private void RegisterContent<T>(UIContentType uiType) where T : class, IController
        {
            var controller = _uiMgr.GetController<T>();
            
            if(controller != null)
            {
                _uiContentDic.Add(uiType, controller);
            }
        }

        public bool IsTransitionBlocked()
        {
            return _isEventBlock;
        }
    
        private async void DoMove(UIContentType uiType)
        {
            if (_isEventBlock)
            {
                return;
            }
            
            if (_uiContentDic.TryGetValue(uiType, out var target) == false)
            {
                Log.EF(LogCategory.UI, "Cannot get controller from dictionary, {0}", uiType.ToString());
                return;
            }
            
            if (target == _currController)
            {
                return;
            }

            if (_uiCallStack.Contains(target))
            {
                _uiCallStack.Remove(target);
            }
            
            _uiCallStack.Add(target);

            _isEventBlock = true;

            if (_currController != null)
            {
                await _currController.SetVisible(false);
            }

            if (_initData != null)
            {
                target.SetInitData(_initData);
            }
            
            await _uiMgr.SetVisibleUI(typeof(LoadingUIController), true);
            await target.SetVisible(true, true);
            await _uiMgr.SetVisibleUI(typeof(LoadingUIController), false);
            await target.ShowUIVisibleAnimation(true);

            _isEventBlock = false;
            _currController = target;
            _moveFinished.Execute();
        }
        
        private async void DoBack()
        {
            if (_isEventBlock)
            {
                return;
            }
            
            if (_uiCallStack.Count <= 1)
            {
                return;
            }

            var targetUI = _uiCallStack[_uiCallStack.Count - 1];

            if (_currController != targetUI)
            {
                return;
            }

            _isEventBlock = true;
            _uiCallStack.RemoveAt(_uiCallStack.Count-1);

            await targetUI.SetVisible(false);
            await _uiMgr.SetVisibleUI(typeof(LoadingUIController), true);
            
            _currController = _uiCallStack[_uiCallStack.Count - 1];
            await _currController.SetVisible(true, true);
            await _uiMgr.SetVisibleUI(typeof(LoadingUIController), false);
            await _currController.ShowUIVisibleAnimation(true);
                
            _isEventBlock = false;
            _backFinished.Execute();
        }

        public void Dispose()
        {
            _backEvent?.Dispose();
            _disposable?.Dispose();
        }

        public void ClearBackList()
        {
            _uiCallStack.Clear();
        }
    }
}