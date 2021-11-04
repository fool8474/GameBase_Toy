using System;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

namespace Script.Inject
{
    // Inject를 관리하는 클래스
    public static class Injector
    {
        private static readonly Dictionary<Type, object> InjectDic = new Dictionary<Type, object>();

        // Mono가 아닌 type을 등록
        public static void RegisterTypeScript<T>(bool ignoreOrigin = false) where T : IInjectedClass, IInitialize, new()
        {
            if (InjectDic.ContainsKey(typeof(T)))
            {
                if (ignoreOrigin == false)
                {
                    return;
                } // 기존의 클래스가 존재
                
                InjectDic.Remove(typeof(T));
            }

            var instance = new T();
            instance.Inject();
            instance.Initialize();
            InjectDic.Add(typeof(T), instance);
        }

        // Mono를 등록 (path로 받음)
        public static void RegisterTypeMono<T>(string path) where T : MonoBehaviour, IInjectedClass
        {
            if (InjectDic.ContainsKey(typeof(T)))
            {
                InjectDic.Remove(typeof(T));
                return;
            }

            if (InjectDic.TryGetValue(typeof(PrefabMgr), out var prefabManager) == false)
            {
                Debug.LogErrorFormat("No PrefabManager to load target {0}", typeof(T));
                return;
            }

            var target = ((PrefabMgr) prefabManager).InstantiateObjPath<T>(path);
            target.Inject();
            InjectDic.Add(typeof(T), target);
        }
       
        // Mono를 등록 (class로 받는 케이스)
        public static void RegisterTypeMono<T>(T target) where T : MonoBehaviour, IInjectedClass
        {
            if (InjectDic.ContainsKey(typeof(T)))
            {
                InjectDic.Remove(typeof(T));
                return;
            }

            target.Inject();
            InjectDic.Add(typeof(T), target);
        }
        
        // 등록한 타입을 사용할 수 있도록 함
        public static T GetInstance<T>()
        {
            return (T)InjectDic[typeof(T)];
        }
    }
}