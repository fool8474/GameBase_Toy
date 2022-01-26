using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using UnityEngine;

namespace Script.Manager
{
    public class UnityPlugins : ScriptMgr
    {
        public IAndroidToastExtension AndroidToastExtension;
        public IAndroidInAppExtension AndroidInAppExtension;

        public override void Initialize()
        {
#if UNITY_EDITOR
            InitializeUnity();
#elif UNITY_ANDROID
            InitializeAndroid();
#endif
        }

        private void InitializeUnity()
        {
            AndroidToastExtension = new PluginToastExtensionBase();
            AndroidInAppExtension = new PluginInAppExtensionBase();
        }

        private void InitializeAndroid()
        {
            var pluginClass = new AndroidJavaClass("com.anyaTeam.androidHere.UnityPlugin");
            var pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("Instance");

            AndroidToastExtension = new AndroidToastExtension(pluginInstance);
            AndroidInAppExtension = new AndroidInAppExtension(pluginInstance);
            AndroidInAppExtension.InitPurchaseSystem();
        }
    }
}
