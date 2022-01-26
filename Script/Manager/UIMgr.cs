using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.UI;
using Script.UI.Controllers;
using UnityEngine;

namespace Script.Manager
{
    public enum UIType
    {
        MAIN = 0,
        FIXED = 100,
        POPUP = 200,
        LOADING = 300,
    }
    
    public class UIMgr : ScriptMgr
    {
        private Dictionary<Type, List<IController>> _uiDictionary;
        private GameObject _uiContainer;

        public override void Initialize()
        {
            _uiContainer = GameObject.Find("UIContainer");
            RegisterUI();
        }

        public void ReturnView(GameObject viewObject)
        {
            viewObject.transform.SetParent(_uiContainer.transform);
        }
        
        public async UniTask SetVisibleUI<T>(bool isVisible) where T : class, IController, new()
        {
            var type = typeof(T);
            if (_uiDictionary.TryGetValue(type, out var controllerList) == false)
            {
                Log.EF(LogCategory.UI, "Cannot find presenter from uiDic {0}", type.Name);
                return;
            }
            
            if (controllerList.Count == 0)
            {
                Log.EF(LogCategory.UI, "No Controller in list {0}", type.Name);
                return;
            }

            var controller = GetValidController<T>(controllerList, isVisible);
            await controller.SetVisible(isVisible);
        }

        private T GetValidController<T>(IEnumerable<IController> ctrlList, bool isVisible) where T : class, IController, new()
        {
            foreach(var ctrl in ctrlList)
            {
                if(ctrl.IsVisible() != isVisible)
                {
                    return ctrl as T;
                }
            }

            // No Valid Controller Case
            var controller = new T();
            RegisterUI(controller);
            return controller;
        }
        
        public T GetController<T>() where T : class, IController, new()
        {
            if (TryGetController<T>(out var controller) == false)
            {
                Log.EF(LogCategory.UI, "Cannot get controller from dic, {0}", typeof(T).Name);
                return null;
            }

            return controller;
        }
        
        public bool TryGetController<T>(out T controller) where T : class, IController, new()
        {
            controller = default;
            
            if(_uiDictionary.TryGetValue(typeof(T), out var ctrlList) == false)
            {
                return false;
            }

            controller = GetValidController<T>(ctrlList, true);

            return true;
        }
        
        public void RegisterUI<T>(T value) where T : IController
        {
            var type = typeof(T);
            if (_uiDictionary.TryGetValue(type, out var ctrlList) == false)
            {
                ctrlList = new List<IController>();
                _uiDictionary.Add(type, ctrlList);
            }

            ctrlList.Add(value);
        }
        
        // ** 새로운 UI 추가 시 이곳에 Register해야 함
        private void RegisterUI()
        {
            _uiDictionary = new Dictionary<Type, List<IController>>();

            RegisterUI(new TestController());
            RegisterUI(new TestControllerPopup());
            RegisterUI(new TestControllerPopup2());
            RegisterUI(new TestControllerPopup3());
            RegisterUI(new LoadingUIController());
            RegisterUI(new TestHeavyController());
            RegisterUI(new InGameUIController());
            RegisterUI(new LobbyNavigationBarController());
            RegisterUI(new AndroidTestUIController());
            RegisterUI(new BottomFixedPlayerStatUIController());
        }
    }
}