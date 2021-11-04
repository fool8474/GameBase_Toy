using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Manager
{
    public enum PoolSize
    {
        ONLY_ONE = 1,
        VERY_LOW = 3,
        LOW = 6,
        MEDIUM = 10,
        HIGH = 30,
        VERY_HIGH = 50,
        CUSTOM = -1,
    }
    
    public class ObjPoolMgr : MonoMgr
    {
        private Dictionary<GameObject, List<GameObject>> _objPoolDic;
        
        public override void Inject()
        {
            
        }

        private void Start()
        {
            _objPoolDic = new Dictionary<GameObject, List<GameObject>>();
        }

        public void InitPool(GameObject obj, PoolSize sizeType, int count = 0)
        {
            if (obj == null)
            {
                Debug.LogErrorFormat("GameObject is Null");
                return;
            }

            if (_objPoolDic.ContainsKey(obj))
            {
                Debug.LogErrorFormat("Pool already have gameObj {0}", obj.name);
                return;
            }
            
            var objs = new List<GameObject>();
            AddPool(obj, objs, sizeType == PoolSize.CUSTOM ? count : (int)sizeType);
            _objPoolDic.Add(obj, objs);
        }
        
        private List<GameObject> AddPool(GameObject target, ICollection<GameObject> targetList, int count)
        {
            var newObjs = new List<GameObject>();
            for (var i = 0; i < count; i++)
            {
                var instantiated = Instantiate(target);
                newObjs.Add(instantiated);
                targetList.Add(instantiated);
            }

            return newObjs;
        }

        public void Return(GameObject target)
        {
            if (_objPoolDic.TryGetValue(target, out var list) == false)
            {
                var newList = new List<GameObject>();
                AddPool(target, newList, (int)PoolSize.ONLY_ONE);
                _objPoolDic.Add(target, newList);
            }

            else
            {
                list.Add(target);
            }
        }

        #region PopPool
        public bool TryPopPool(GameObject target, out GameObject rtnTarget)
        {
            if (_objPoolDic.TryGetValue(target, out var list) == false)
            {
                InitPool(target, PoolSize.ONLY_ONE);
                list = _objPoolDic[target];
            }
            
            if (list.Count > 0)
            {
                rtnTarget = list.FirstOrDefault();
                list.Remove(rtnTarget);

                return true;
            }
            
            rtnTarget = AddPool(target, list, 1)[0];
            return true;
        }

        public bool TryPopPool<T>(GameObject target, out T rtnTarget)
        {
            if (TryPopPool(target, out var goTarget) == false)
            {
                Debug.LogErrorFormat("Cannot get Target, target name is {0}", target.name);
                rtnTarget = default;
                return false;
            }

            if (goTarget.TryGetComponent<T>(out rtnTarget))
            {
                return true;
            }
                
            Debug.LogErrorFormat("Target didn't have component name {0}, target name is {1}", typeof(T).ToString(), target.name);
            return false;
        }

        #endregion
    }
}