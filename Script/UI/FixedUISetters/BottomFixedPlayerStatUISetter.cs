using Cysharp.Threading.Tasks;
using Script.UI.Controllers;

namespace Script.UI.FixedUISetters
{
    public class BottomFixedPlayerStatUISetter : FixedUISetter
    {
        public bool ShowCoin = true;
        public bool ShowDiamond = true;

        protected override void SetTargetUI()
        {
            _targetUI = _uiMgr.GetController<BottomFixedPlayerStatUIController>();
        }

        public override async UniTask ShowUIWithInitData(bool isVisible, bool isImmediate)
        {
            await _targetUI.SetVisible(isVisible, isImmediate);
            _targetUI.ShowUIVisibleAnimation(isVisible).Forget();

            if (_targetUI is BottomFixedPlayerStatUIController targetUI)
                targetUI.ShowUIWithSetter(this);
        }
    }
}
