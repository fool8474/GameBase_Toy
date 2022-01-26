using UnityEngine;

namespace Script.Manager
{
    public interface IAndroidInAppExtension
    {
        public void InitPurchaseSystem();
        public void Purchase();
        public void GetSkusList(string[] skus);
    }

    public class PluginInAppExtensionBase : IAndroidInAppExtension
    {
        public void InitPurchaseSystem() { }
        public void Purchase() { }
        public void GetSkusList(string[] skus) { }
    }

    public class AndroidInAppExtension : IAndroidInAppExtension
    {
        private AndroidJavaObject _javaObject;

        public AndroidInAppExtension(AndroidJavaObject javaObject)
        {
            _javaObject = javaObject;
        }

        public void InitPurchaseSystem()
        {
            //_javaObject.Call("InitPurchaseSystem");
        }

        public void Purchase()
        {
            _javaObject.Call("Purchase");
        }

        public void GetSkusList(string[] skus)
        {
            //_javaObject.Call("ConnectBillingClient", skus);
        }
    }
}