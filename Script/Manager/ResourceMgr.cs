using System.Text;
using Cysharp.Threading.Tasks;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.Util;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Script.Manager
{
    public class ResourceMgr : MonoMgr
    {
        // Resources / Addressable Load 제공

        #region Addressables
        public async UniTask<T> InstantiateObjId<T>(string id, Transform parent = null) where T : Component
        {
            // T Case
            var go = await Addressables.InstantiateAsync(id, parent);
            go.name = go.name.Replace("(Clone)", "").Trim();
            return CheckAndGetComponent<T>(id, go);
        }

        public async UniTask<GameObject> InstantiateObjId(string id, Transform parent = null)
        {
            // GameObject Case
            var go = await Addressables.InstantiateAsync(id, parent);
            go.name = go.name.Replace("(Clone)", "").Trim();
            return go;
        }

        public async UniTask<T> GetObjById<T>(string id)
        {
            // Not Mono Type
            return await Addressables.LoadAssetAsync<T>(id);
        }

        #endregion

        #region Resources
        public T InstantiateObjPath<T>(string path, Transform parent = null) where T : Component
        {
            // T Case
            var go = Instantiate(GetPrefabFromPath<T>(path), parent);
            go.name = go.name.Replace("(Clone)", "").Trim();
            return go;
        }

        public GameObject InstantiateObjPath(string path, Transform parent = null)
        {
            // GameObject Case
            var go = Instantiate(GetPrefabFromPath(path), parent);
            go.name = go.name.Replace("(Clone)", "").Trim();
            return go;
        }
        
        private T GetPrefabFromPath<T>(string path) where T : Component
        {
            var go = (GameObject)Resources.Load(PrefabPath.PREFAB_PATH + path);
            return CheckAndGetComponent<T>(path, go);
        }

        private GameObject GetPrefabFromPath(string path)
        {
            return (GameObject)Resources.Load(PrefabPath.PREFAB_PATH + path);
        }
        
        #endregion

        private T CheckAndGetComponent<T>(string id, GameObject go) where T : Component
        {
            if (go == null)
            {
                Log.EF(LogCategory.PREFAB, "No Resource in {0}", id);
            }
            
            if (go.TryGetComponent<T>(out var returnVal) == false)
            {
                Log.EF(LogCategory.PREFAB, "No Component in type {0}, {1}", typeof(T).ToString(), go.name);
            }

            return returnVal;
        }

        public static bool IsValidResourcesPath(string path)
        {
            // 유효한 Path인지 확인, Resources로 사용 가능한지 체크
            var buildPath = new StringBuilder(PrefabPath.RESOURCES_PATH);
            buildPath.Append(PrefabPath.PREFAB_PATH);
            buildPath.Append(path);
            
            return System.IO.File.Exists(buildPath.ToString());
        }
    }
}