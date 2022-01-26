using Cysharp.Threading.Tasks;
using Script.UI.Controllers;

namespace Script.UI.FixedUISetters
{
    public class LobbyNavigationBarUISetter : FixedUISetter
    {
        public bool ButtonShop = true;
        public bool ButtonUI1 = true;
        public bool ButtonUI2 = true;
        public bool ButtonUI3 = true;
        public bool ButtonGame = true;

        protected override void SetTargetUI()
        {
            _targetUI = _uiMgr.GetController<LobbyNavigationBarController>();
        }

        public override async UniTask ShowUIWithInitData(bool isVisible, bool isImmediate)
        {
            await _targetUI.SetVisible(isVisible, isImmediate);
            _targetUI.ShowUIVisibleAnimation(isVisible).Forget();

            if (_targetUI is LobbyNavigationBarController targetUI)
                targetUI.ShowUIWithSetter(this);
        }
    }
}
