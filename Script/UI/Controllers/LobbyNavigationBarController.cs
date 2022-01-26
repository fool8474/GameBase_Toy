using Script.UI.FixedUISetters;
using Cysharp.Threading.Tasks;
using Script.Event;
using Script.Inject;
using Script.Manager;
using Script.UI.Models;
using Script.Util;
using UniRx;

namespace Script.UI.Controllers
{

    public class LobbyNavigationBarController : Controller<LobbyNavigationBarModel>, IFixedUI
    {
        public LobbyNavigationBarController() : base(AddressableID.LOBBY_NAVIGATION_BAR_UI, new LobbyNavigationBarModel()){}

        private UITransitionMgr _uITransitionMgr;

        protected override void Inject()
        {
            base.Inject();
            _uITransitionMgr = Injector.GetInstance<UITransitionMgr>();
        }

        public override void Initialize()
        {
            base.Initialize();

            _model.MoveEvent
                .Subscribe(MoveTo)
                .AddTo(_disposable);
        }
        public void ShowUIWithSetter(FixedUISetter fixedUISetter)
        {
            if(fixedUISetter is LobbyNavigationBarUISetter uiSetter)
            {
                _model.SetUISetter.Execute(uiSetter);
            }
        }

        private void MoveTo(UIContentType moveType)
        {
            _uITransitionMgr.MoveEvent.Execute(moveType);
        }
    }
}
