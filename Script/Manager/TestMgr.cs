using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.UI.Controllers;
using UniRx;
using UnityEngine;

namespace Script.Manager
{
    public class TestMgr : MonoMgr
    {
        private UITransitionMgr _uiTransitionMgr;
        private CanvasMgr _canvasMgr;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public override void Initialize()
        {
            _canvasMgr.FinishLoadingCanvas
                .Subscribe(ShowStartedMainUI)
                .AddTo(_disposables);
        }

        private void ShowStartedMainUI()
        {
            _uiTransitionMgr.MoveEvent.Execute(UIContentType.TEST1);
        }

        public override void Inject()
        {
            _uiTransitionMgr = Injector.GetInstance<UITransitionMgr>();
            _canvasMgr = Injector.GetInstance<CanvasMgr>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                _uiTransitionMgr.MoveEvent.Execute(UIContentType.TEST1);
            }
            
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _uiTransitionMgr.SetInitData(new TestControllerPopupInitData(new List<string>
                {
                    Random.Range(0, 100).ToString(),
                    Random.Range(0, 100).ToString(),
                    Random.Range(0, 100).ToString(),
                    Random.Range(0, 100).ToString(),
                    Random.Range(0, 100).ToString(),
                    Random.Range(0, 100).ToString()
                })).MoveEvent.Execute(UIContentType.TEST2);
            }
            
            else if (Input.GetKeyDown(KeyCode.F))
            {
                _uiTransitionMgr.BackEvent.Execute();
            }
            
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                _uiTransitionMgr.MoveEvent.Execute(UIContentType.TEST4);
            }
            
            else if (Input.GetKeyDown(KeyCode.H))
            {
                _uiTransitionMgr.MoveEvent.Execute(UIContentType.MAIN_GAME);
            }
        }
    }
}