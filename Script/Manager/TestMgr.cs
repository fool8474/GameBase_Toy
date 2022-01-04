using System.Collections.Generic;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.UI.Controllers;
using UnityEngine;

namespace Script.Manager
{
    public class TestMgr : MonoMgr
    {
        private PopupMgr _popupMgr;
        private UITransitionMgr _uiTransitionMgr;
        
        public override void Inject()
        {
            _popupMgr = Injector.GetInstance<PopupMgr>();
            _uiTransitionMgr = Injector.GetInstance<UITransitionMgr>();
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
            
            // else if (Input.GetKeyDown(KeyCode.D))
            // {
            //     _popupMgr.AddPopup(PopupUIType.TEST);
            // }
            
            // else if (Input.GetKeyDown(KeyCode.G))
            // {
            //     _popupMgr.QuitPopup();
            // }

            else if (Input.GetKeyDown(KeyCode.H))
            {
                _uiTransitionMgr.MoveEvent.Execute(UIContentType.MAIN_GAME);
            }
        }
    }
}