using Cysharp.Threading.Tasks;
using Script.Inject;
using Script.Manager;
using UnityEngine;

namespace Script.UI
{
    public class FixedUISetter : MonoBehaviour
    {
        protected IController _targetUI;
        protected UIMgr _uiMgr;

        public void Initialize()
        {
            _uiMgr = Injector.GetInstance<UIMgr>();
            SetTargetUI();
        }

        protected virtual void SetTargetUI() { _targetUI = null; }
        public virtual UniTask ShowUIWithInitData(bool isVisible, bool isImmediate) { return default;  }
    }
}
