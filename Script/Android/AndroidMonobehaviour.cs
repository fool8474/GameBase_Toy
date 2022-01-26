using Newtonsoft.Json.Linq;
using Script.Inject;
using Script.Manager;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Android
{
    // Android로부터 return받는 대상 Monobehaviour
    public class AndroidMonobehaviour : MonoMgr
    {
        private InAppMgrAndroid _inAppMgrAndroid;
        public override void Inject()
        {
            _inAppMgrAndroid = Injector.GetInstance<InAppMgrAndroid>();
        }

        private void ShowDebug(string text)
        {
            Log.DF(LogCategory.ANDROID, "android called {0}", text);
        }

        private void OnQueryInventory(string skusJson)
        {
            _inAppMgrAndroid.OnQueryInventory(skusJson);
        }
    }
}
