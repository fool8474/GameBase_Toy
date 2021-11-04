using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Script.Manager
{
    public class PrefabMgr : MonoMgr
    {
        public const string RESOURCE_PATH = "Prefabs/";
        
        private T GetPrefabFromPath<T>(string path) where T : Object
        {
            var go = (GameObject)Resources.Load(RESOURCE_PATH + path);

            if (go.TryGetComponent<T>(out var returnVal) == false)
            {
                Debug.LogErrorFormat("No Component in {0}", go.name);
            }

            return returnVal;
        }

        public T InstantiateObjPath<T>(string path, Transform parent = null) where T : Object
        {
            return Instantiate(GetPrefabFromPath<T>(path), parent);
        }

        private async UniTask<T> GetPrefabById<T>(string id) where T : Object
        {
            return await Addressables.LoadAssetAsync<T>(id);
        }

        public async UniTask<T> InstantiateObjId<T>(string id, Transform parent = null) where T : Object
        {
            var go = await Addressables.InstantiateAsync(id, parent);
            
            if (go.TryGetComponent<T>(out var returnVal) == false)
            {
                Debug.LogErrorFormat("No Component in {0}", go.name);
            }
            
            return returnVal;
        }
        
        public override void Inject()
        {
        }
    }

    public static class PrefabPath
    {
        public const string UI_PATH = "UI/";

        public const string MAIN_CANVAS_NAME = "MainCanvas";
    }
}