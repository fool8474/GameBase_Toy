using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Inject;
using Script.Manager.Player;
using Script.UI.FixedUISetters;
using Script.UI.Models;
using Script.Util;
using UniRx;

namespace Script.UI.Controllers
{
    public class BottomFixedPlayerStatUIController : Controller<BottomFixedPlayerStatUIModel>
    {
        private PlayerMoneyHolder _moneyHolder;

        public BottomFixedPlayerStatUIController() : base(AddressableID.BOTTOM_FIXED_PLAYER_STAT_UI, new BottomFixedPlayerStatUIModel()) { }

        public override void Initialize()
        {
            base.Initialize();

            _model.UpdateNewValue
                .Subscribe(UpdateNewValueTest)
                .AddTo(_disposable);
        }

        protected override void Inject()
        {
            base.Inject();
            _moneyHolder = Injector.GetInstance<PlayerStatus>().GetHolder<PlayerMoneyHolder>();
        }

        public void ShowUIWithSetter(FixedUISetter fixedUISetter)
        {
            if(fixedUISetter is BottomFixedPlayerStatUISetter uiSetter)
            {
                _model.SetUISetter.Execute(uiSetter);
            }
        }

        private void UpdateNewValueTest(MoneyType type)
        {
            _moneyHolder.AddMoney(type, 100);
        }
    }
}
