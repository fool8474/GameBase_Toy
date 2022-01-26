using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Script.Manager;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using UnityEngine;

namespace Script.Inject
{
    // DISystem
    public static class Injector
    {
        private static readonly Dictionary<Type, object> InjectDic = new Dictionary<Type, object>();

        // Inject / Initialize를 하지 않는 클래스 등록
        public static T RegisterType<T>(bool ignoreOrigin) where T : new()
        {
            if (InjectDic.ContainsKey(typeof(T)))
            {
                if (ignoreOrigin == false)
                {
                    return default;
                }
                InjectDic.Remove(typeof(T));
            }

            var instance = new T();
            InjectDic.Add(typeof(T), instance);

            return instance;
        }
        
        // Mono가 아닌 type을 등록
        public static void RegisterTypeScript<T>(bool ignoreOrigin = false) where T : IInjectedClass, IInitialize, new()
        {
            RegisterType<T>(ignoreOrigin);
        }

        // Mono를 등록 (path로 받음)
        public static void RegisterTypeMono<T>(string path) where T : MonoBehaviour, IInjectedClass, IInitialize
        {
            if (InjectDic.ContainsKey(typeof(T)))
            {
                return;
            }

            if (InjectDic.TryGetValue(typeof(ResourceMgr), out var prefabManager) == false)
            {
                Log.EF(LogCategory.INJECT, "No PrefabManager to load target {0}", typeof(T));
                return;
            }

            var target = ((ResourceMgr) prefabManager).InstantiateObjPath<T>(path);
            InjectDic.Add(typeof(T), target);
        }

        // Mono를 등록 (class로 받는 케이스)
        public static void RegisterTypeMono<T>(T target) where T : MonoBehaviour, IInjectedClass, IInitialize
        {
            if (InjectDic.ContainsKey(typeof(T)))
            {
                InjectDic.Remove(typeof(T));
                return;
            }

            InjectDic.Add(typeof(T), target);
        }

        public static void InitializeContainer()
        {
            foreach(var injectedTarget in InjectDic.Values)
            {
                if (injectedTarget is IInjectedClass injectable)
                {
                    injectable.Inject();
                }
                
                if (injectedTarget is IInitialize initializable)
                {
                    initializable.Initialize();
                }
            }
        }
       
        
        // 등록한 타입을 사용할 수 있도록 함
        public static T GetInstance<T>()
        {
            return (T)InjectDic[typeof(T)];
        }
    }
}