using System;
using Script.Inject;
using Script.UI.Presenters;
using UnityEngine;

namespace Script.Manager
{
    public class TestMgr : MonoMgr
    {
        private UIMgr uiMgr;
        
        public override void Inject()
        {
            uiMgr = Injector.GetInstance<UIMgr>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                uiMgr.ShowUI(typeof(TestController), true);
            }
            
            else if (Input.GetKeyDown(KeyCode.S))
            {
                uiMgr.ShowUI(typeof(TestController), false);
            }
            
            if(Input.GetKeyDown(KeyCode.D))
            {
                uiMgr.ShowUI(typeof(TestControllerPopup), true);
            }
            
            else if (Input.GetKeyDown(KeyCode.F))
            {
                uiMgr.ShowUI(typeof(TestControllerPopup), false);
            }

        }
    }
}