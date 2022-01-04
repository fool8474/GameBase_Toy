using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.UI;
using Script.UI.Controllers;

namespace Script.Manager
{
    public enum PopupUIType
    {
        TEST,
    }
    
    public class PopupMgr : ScriptMgr
    {
        private UIMgr _uiMgr;

        private List<IController> _popupList;
        
        public override void Initialize()
        {
            base.Initialize();
            
            _popupList = new List<IController>();
        }

        public override void Inject()
        {
            base.Inject();
            _uiMgr = Injector.GetInstance<UIMgr>();
        }

        public void AddPopup(PopupUIType uiType)
        {
            switch (uiType)
            {
                case PopupUIType.TEST:
                    AddPopup<TestControllerPopup3>();    
                    break;
            }
        }
        
        private async void AddPopup<T>() where T : IController, new()
        {
            var controller = new T();
            _popupList.Add(controller);
            await controller.SetVisible(true);
        }

        public async void QuitPopup()
        {
            foreach (var controller in _popupList)
            {
                await controller.SetVisible(false);
            }
            
            _popupList.Clear();
        }
    }
}