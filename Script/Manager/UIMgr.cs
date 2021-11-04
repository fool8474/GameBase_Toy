using System;
using System.Collections.Generic;
using System.Linq;
using Script.Inject;
using Script.UI;
using Script.UI.Models;
using Script.UI.Presenters;
using UnityEngine;

namespace Script.Manager
{
    public enum UIType
    {
        MAIN = 0,
        FIXED = 100,
        POPUP = 200
    }
    
    public class UIMgr : ScriptMgr
    {
        private Dictionary<Type, IController> _uiSingleDic;
        private Dictionary<Type, List<IController>> _uiMultiDic;
        
        private PrefabMgr _prefabMgr;
        private ObjPoolMgr _objPoolMgr;
        
        public override void Initialize()
        {
            AddCanvas();
            RegisterUI();
        }

        private void AddCanvas()
        {
            _prefabMgr.InstantiateObjPath<Canvas>(PrefabPath.UI_PATH + PrefabPath.MAIN_CANVAS_NAME);
        }

        private void RegisterUI()
        {
            _uiSingleDic = new Dictionary<Type, IController>
            {
                {typeof(TestController), new TestController(ScriptableObject.CreateInstance<TestModel>())}
            };

            _uiMultiDic = new Dictionary<Type, List<IController>>
            {
                {typeof(TestControllerPopup), new List<IController> {new TestControllerPopup(ScriptableObject.CreateInstance<TestModelPopup>())}} 
            };
        }

        public override void Inject()
        {
            _prefabMgr = Injector.GetInstance<PrefabMgr>();
            _objPoolMgr = Injector.GetInstance<ObjPoolMgr>();
        }

        private void ShowPopup()
        {
            
        }

        private void RefreshLayer()
        {
            
        }
        
        public bool ShowUI(Type showType, bool isVisible)
        {
            UIData data = null;
            if (_uiSingleDic.TryGetValue(showType, out var controller))
            {
                data = controller.GetUIData();
                
                if (data.IsSingle && controller.IsVisible())
                {
                    Debug.LogFormat("UI is already visible {0}", data.Name);
                    return false;
                }

                ShowUI(data, controller, isVisible);
                return true;
            }

            if (_uiMultiDic.TryGetValue(showType, out var controllers))
            {
                if (controllers.Any(currCtrl => currCtrl.IsVisible() == false))
                {
                    ShowUI(data, controller, isVisible);
                    return true;
                }
                
                // TODO : Generate New Controller Set
            }
            
            Debug.LogErrorFormat("Cannot find presenter from uiDic {0}", showType.Name);
            return false;
        }

        private void ShowUI(UIData data, IController controller, bool isVisible)
        {
               
            switch (data.UIType)
            {
                case UIType.MAIN:
                    controller.SetVisible(isVisible);
                    break;
                case UIType.FIXED:
                    controller.SetVisible(isVisible);
                    break;
                case UIType.POPUP:
                    controller.SetVisible(isVisible);
                    break;
                default:
                    break;
            }
        }
    }
}