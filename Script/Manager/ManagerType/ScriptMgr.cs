using Script.Inject;

namespace Script.Manager.ManagerType
{
    // Mgr클래스 중 Mono 상속 타입이 아닌 클래스 
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