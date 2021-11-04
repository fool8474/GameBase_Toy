using Script.Inject;
using Script.Manager.Util;
using UnityEditor;
using UnityEngine;

namespace Script.Manager
{
    public class ScriptMgr : IInjectedClass, IInitialize
    {
        public virtual void Inject()
        {
        }

        public virtual void Initialize()
        {
        }
    }
}