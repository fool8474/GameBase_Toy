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
        ANDROID,
    }
    
    public class UITransitionMgr : ScriptMgr, IDisposable
    {
        private Dictionary<UIContentType, IController> _uiContentDic;
        private List<IController> _uiCallStack;  
        private IController _currController;

        private UIMgr _uiMgr;
        private PopupMgr _popupMgr;
        
        private UIInitData _initData;
        private bool _isEventBlock;
        public HashSet<int> _animationUISet;

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
            _animationUISet = new HashSet<int>();
            
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
            _popupMgr = Injector.GetInstance<PopupMgr>();
        }

        private void RegisterContent()
        {
            RegisterContent<TestController>(UIContentType.TEST1);
            RegisterContent<TestControllerPopup>(UIContentType.TEST2);
            RegisterContent<TestControllerPopup3>(UIContentType.TEST3);
            RegisterContent<TestHeavyController>(UIContentType.TEST4);
            RegisterContent<InGameUIController>(UIContentType.MAIN_GAME);
            RegisterContent<AndroidTestUIController>(UIContentType.ANDROID);
        }

        public UITransitionMgr SetInitData(UIInitData initData)
        {
            _initData = initData;
            return this;
        }
        
        private void RegisterContent<T>(UIContentType uiType) where T : class, IController, new()
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
    
        public void RegisterAnimationUI(int code)
        {
            if(_animationUISet.Contains(code))
            {
                return;
            }

            _animationUISet.Add(code);
        }

        public void FinishAnimationUI(int code)
        {
            if(_animationUISet.Contains(code) == false)
            {
                return;
            }

            _animationUISet.Remove(code);
        }

        private async void DoMove(UIContentType uiType)
        {
            if (_isEventBlock || _animationUISet.Count > 0)
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

            if (_initData != null)
            {
                target.SetInitData(_initData);
            }

            // PopUp 제거 -> Loading 표기 -> 기존 UI 제거(즉시) -> 새 UI 표시(즉시) -> Loading 제거 -> 새 UI의 애니메이션 재생
            await _popupMgr.ClearPopup();
            await _uiMgr.SetVisibleUI<LoadingUIController>(true);
            if (_currController != null)
            {
                await _currController.SetVisible(false, true);
            }

            await target.SetVisible(true, true);
            await _uiMgr.SetVisibleUI<LoadingUIController>(false);
            await target.ShowUIVisibleAnimation(true);

            _isEventBlock = false;
            _currController = target;
            _moveFinished.Execute();
        }
        
        private async void DoBack()
        {
            if (_isEventBlock || _animationUISet.Count > 0)
            {
                return;
            }
            
            if (_uiCallStack.Count <= 1)
            {
                return;
            }

            var targetUI = _uiCallStack[_uiCallStack.Count - 1];

            if(_popupMgr.HasPopup())
            {
                _popupMgr.BackEvent.Execute();
                return;
            }

            if (_currController != targetUI)
            {
                return;
            }

            _isEventBlock = true;
            _uiCallStack.RemoveAt(_uiCallStack.Count-1);

            // Loading 표기 -> 기존 UI 제거(즉시) -> 새 UI 표시(즉시) -> Loading 제거 -> 새 UI의 애니메이션 재생
            await _uiMgr.SetVisibleUI<LoadingUIController>(true);
            await targetUI.SetVisible(false, true);
            
            _currController = _uiCallStack[_uiCallStack.Count - 1];
            await _currController.SetVisible(true, true);
            await _uiMgr.SetVisibleUI<LoadingUIController>(false);
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