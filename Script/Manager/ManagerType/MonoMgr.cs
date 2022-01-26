using Script.Inject;
using UnityEngine;

namespace Script.Manager.ManagerType
{
    // Mgr 클래스 중 MonoBehaviour를 상속하는 Mgr
    public class MonoMgr : MonoBehaviour, IInjectedClass, IInitialize
    {
        public virtual void Inject() {}

        public virtual void Initialize() {}
    }
}