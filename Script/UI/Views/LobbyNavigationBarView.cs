using Script.UI.FixedUISetters;
using Script.Event;
using Script.Manager;
using Script.UI;
using Script.UI.Component;
using Script.UI.Models;
using UniRx;
using UnityEngine;

namespace Assets.Script.UI.Views
{
    public class LobbyNavigationBarView : View<LobbyNavigationBarModel>
    {
        [SerializeField] private AnButton _btnMainTest;
        [SerializeField] private AnButton _btnTest2;
        [SerializeField] private AnButton _btnTest3;
        [SerializeField] private AnButton _btnGame;
        [SerializeField] private AnButton _btnShop;

        public override void Initialize()
        {
            base.Initialize();

            _model.SetUISetter
                .Subscribe(SetUISetter)
                .AddTo(_disposable);

            _btnMainTest.OnClickAsObservable()
                .Subscribe(_ => OnClickButton(UIContentType.TEST1))
                .AddTo(_disposable);

            _btnTest2.OnClickAsObservable()
                .Subscribe(_ => OnClickButton(UIContentType.TEST2))
                .AddTo(_disposable);

            _btnTest3.OnClickAsObservable()
                .Subscribe(_ => OnClickButton(UIContentType.TEST4))
                .AddTo(_disposable);

            _btnGame.OnClickAsObservable()
                .Subscribe(_ => OnClickButton(UIContentType.MAIN_GAME))
                .AddTo(_disposable);

            _btnShop.OnClickAsObservable()
                .Subscribe(_ => OnClickButton(UIContentType.ANDROID))
                .AddTo(_disposable);
        }

        private void OnClickButton(UIContentType type)
        {
             _model.MoveEvent.Execute(type); 
        }

        private void SetUISetter(LobbyNavigationBarUISetter uiSetter)
        {
            SetButtonActive(_btnShop, uiSetter.ButtonShop);
            SetButtonActive(_btnMainTest, uiSetter.ButtonUI1);
            SetButtonActive(_btnTest2, uiSetter.ButtonUI2);
            SetButtonActive(_btnTest3, uiSetter.ButtonUI3);
            SetButtonActive(_btnGame, uiSetter.ButtonGame);
        }

        private void SetButtonActive(AnButton button, bool isActive)
        {
            button.gameObject.SetActive(isActive);
        }
    }
}
