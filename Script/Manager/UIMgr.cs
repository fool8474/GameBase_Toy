using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.UI;
using Script.UI.Controllers;

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
        
        public override void Initialize()
        {
            RegisterUI();
        }
        
        public override void Inject()
        {
        }
        
        public async UniTask SetVisibleUI(Type showType, bool isVisible)
        {
            if (_uiDictionary.TryGetValue(showType, out var controllerList) == false)
            {
                Log.EF(LogCategory.UI, "Cannot find presenter from uiDic {0}", showType.Name);
                return;
            }
            
            if (controllerList.Count == 0)
            {
                Log.EF(LogCategory.UI, "No Controller in list {0}", showType.Name);
                return;
            }

            var controller = GetValidController(controllerList);

            if (controller == null)
            {
                Log.EF(LogCategory.UI, "No Valid Controller {0}", showType.Name);
                return;
            }
            
            await controller.SetVisible(isVisible);
        }

        private IController GetValidController(IEnumerable<IController> ctrlList)
        {
            return ctrlList.FirstOrDefault();
        }
        
        public T GetController<T>() where T : class, IController
        {
            if (TryGetController<T>(out var controller) == false)
            {
                Log.EF(LogCategory.UI, "Cannot get controller from dic, {0}", typeof(T).Name);
                return null;
            }

            return controller;
        }
        
        public bool TryGetController<T>(out T controller) where T : class, IController
        {
            controller = default;
            
            if(_uiDictionary.TryGetValue(typeof(T), out var ctrlList) == false)
            {
                return false;
            }

            controller = GetValidController(ctrlList) as T;
            return true;
        }
        
        public void RegisterUI(Type key, IController value)
        {
            if (_uiDictionary.TryGetValue(key, out var ctrlList) == false)
            {
                ctrlList = new List<IController>();
                _uiDictionary.Add(key, ctrlList);
            }

            ctrlList.Add(value);
        }
        
        // ** 새로운 UI 추가 시 이곳에 Register해야 함
        private void RegisterUI()
        {
            _uiDictionary = new Dictionary<Type, List<IController>>();
            
            RegisterUI(typeof(TestController), new TestController());
            RegisterUI(typeof(TestControllerPopup), new TestControllerPopup());
            RegisterUI(typeof(TestControllerPopup2), new TestControllerPopup2());
            RegisterUI(typeof(TestControllerPopup3), new TestControllerPopup3());
            RegisterUI(typeof(LoadingUIController), new LoadingUIController());
            RegisterUI(typeof(TestHeavyController), new TestHeavyController());
            RegisterUI(typeof(InGameUIController), new InGameUIController());
        }
    }
}