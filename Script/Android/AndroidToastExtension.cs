using Script.Manager.Util.Log;
using UnityEngine;

namespace Script.Manager
{
    public interface IAndroidToastExtension
    {
        void ShowToast(string text);
    }

    public class PluginToastExtensionBase : IAndroidToastExtension
    {
        public void ShowToast(string text) {}
    }

    public class AndroidToastExtension : IAndroidToastExtension
    {
        private AndroidJavaObject _javaObject;

        public AndroidToastExtension(AndroidJavaObject javaObject)
        {
            _javaObject = javaObject;
        }

        public void ShowToast(string text)
        {
            _javaObject.Call("ShowToast", text);
        }
    }
}
