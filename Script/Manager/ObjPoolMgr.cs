using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Inject;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using UnityEngine;

namespace Script.Manager
{
    public class ObjPoolMgr : MonoMgr
    {
        // (Object Get) GetObject - GetGameObject -  CreateNewObject - RegisterNewPool
        // (Object Init) InitObject - RegisterNewPool - CreateNewObject
        
        private Dictionary<string, Queue<GameObject>> _objectPool;
        private ResourceMgr _prefabMgr;
        
        public override void Initialize()
        {
            _objectPool = new Dictionary<string, Queue<GameObject>>();
        }

        public override void Inject()
        {
            _prefabMgr = Injector.GetInstance<ResourceMgr>();
        }

        public void Clear()
        {
            foreach (var currKey in _objectPool.Keys)
            {
                Clear(currKey);
            }
        }

        private void Clear(string id)
        {
            if (_objectPool.TryGetValue(id, out var objectList) == false)
            {
                return;
            }

            foreach (var currGo in objectList)
            {
                Destroy(currGo);
            }
            
            objectList.Clear();
            _objectPool.Remove(id);
        }

        // 오브젝트의 생성을 하거나 최초 등록을 하기위해 사용
        public async UniTask InitObject(string id, int cnt = 1, Transform parent = null)
        {
            RegisterNewPool(id);
            
            // Register시에는 오브젝트를 생성하지 않으므로 따로 생성해주도록 한다.
            for (var i = 0; i < cnt; i++)
            {
                var loaded = await CreateNewObject(id, parent: parent);
                _objectPool[id].Enqueue(loaded);
            }
        }

        public bool HaveKey(string id)
        {
            return _objectPool.ContainsKey(id);
        }

        public bool HaveObject(string id)
        {
            if(HaveKey(id) == false)
            {
                return false;
            }

            return _objectPool[id].Count != 0;
        }

        private async UniTask<GameObject> GetGameObject(string id, Transform parent = null)
        {
            GameObject rtnObj;

            // ObjPool에 등록되었을 경우
            if (_objectPool.TryGetValue(id, out var objQueue))
            {
                if (objQueue.Count == 0)
                {
                    // Queue가 부족할 경우 새롭게 생성
                    await CreateNewObject(id, parent);
                    rtnObj = objQueue.Dequeue();
                }

                else
                {
                    rtnObj = objQueue.Dequeue();
                }
            }
            
            // 등록되지 않았을 경우
            else
            {
                await CreateNewObject(id, parent);
                rtnObj = _objectPool[id].Dequeue();
            }
            
            if (parent != null)
            {
                rtnObj.transform.SetParent(parent);
            }

            return rtnObj;
        }
        
        public async UniTask<GameObject> GetObject(string id, Action createAction = null, Transform parent = null)
        {
            // GameObject는 GetComponent가 안되기 때문에 오버로딩함
            var rtnObj = await GetGameObject(id, parent);
        
            AfterGetObject(rtnObj, createAction);
            return rtnObj;
        }
        
        public async UniTask<T> GetObject<T>(string id, Action createAction = null, Transform parent = null)
        {
            var rtnObj = await GetGameObject(id, parent);
            
            if (rtnObj.TryGetComponent<T>(out var rtnValue) == false)
            {
                Log.EF(LogCategory.OBJECT_POOL, "Cannot load component from obj {0}", rtnObj.name);
                ReturnObject(id, rtnObj);
                return default;
            }
            
            AfterGetObject(rtnObj, createAction);
            return rtnValue;
        }

        // Object를 얻은 뒤 발생
        private void AfterGetObject(GameObject obj, Action createAction = null)
        {
            obj.SetActive(true);
            createAction?.Invoke();
        }
        
        private async UniTask<GameObject> CreateNewObject(string id, Transform parent = null)
        {
            // Resources - Path / Addressable - Id
            var obj = ResourceMgr.IsValidResourcesPath(id) ? 
                _prefabMgr.InstantiateObjPath(id, parent) : 
                await _prefabMgr.InstantiateObjId(id, parent);

            RegisterNewPool(id, obj);
            return obj;
        }

        private GameObject CreateNewObject(GameObject copyTarget, Transform parent = null)
        {
            var newGo = Instantiate(copyTarget, parent);
            newGo.name = copyTarget.name;
            RegisterNewPool(copyTarget.name, newGo);
            return newGo;
        }

        private Queue<GameObject> RegisterNewPool(string id)
        {
            if (HaveKey(id))
            {
                return _objectPool[id];
            }
            
            var newObjectQueue = new Queue<GameObject>();
            _objectPool.Add(id, newObjectQueue);
            return newObjectQueue;
        }
        
        private void RegisterNewPool(string id, GameObject obj)
        {
            var newPool = RegisterNewPool(id);
            newPool.Enqueue(obj);
        }
        
        public void ReturnObject(string id, GameObject returnObj, Action returnAction = null, Transform parent = null)
        {
            if (_objectPool.TryGetValue(id, out var objectList))
            {
                objectList.Enqueue(returnObj);
            }
            else
            {
                RegisterNewPool(id, returnObj);
            }

            returnAction?.Invoke();

            if (returnObj == null)
            {
                return;
            }
            
            returnObj.transform.SetParent(parent == null ? transform : parent);
            returnObj.SetActive(false);
        }
    }
}