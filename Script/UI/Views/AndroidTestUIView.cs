using Script.Inject;
using Script.Manager;
using Script.UI.Component;
using Script.UI.Models;
using Script.UI.ScrollItems;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Script.UI.Views
{
    public class AndroidTestUIView : View<AndroidTestUIModel>
    {
        [SerializeField] private AnButton _btnCallText1;
        [SerializeField] private AnButton _btnCallText2;
        [SerializeField] private ScrollItemController _purchaseController;

        private UnityPlugins _unityPlugins;

        public override void Initialize()
        {
            base.Initialize();

            _btnCallText1.OnClickAsObservable()
                .Subscribe(_ => ShowAndroidToast("Hello World"))
                .AddTo(_disposable);

            _btnCallText2.OnClickAsObservable()
                .Subscribe(_ => ShowAndroidToast("Android Test"))
                .AddTo(_disposable);

            _model.PurchaseDatas
                .Subscribe(UpdatePurchaseScrollItemData)
                .AddTo(_disposable);

            _purchaseController.Init();
        }

        public override void Inject(Model model)
        {
            base.Inject(model);

            _unityPlugins = Injector.GetInstance<UnityPlugins>();
        }

        private void ShowAndroidToast(string text)
        {
            _unityPlugins.AndroidToastExtension.ShowToast(text);
        }
        
        private void UpdatePurchaseScrollItemData(List<PurchaseScrollData> data)
        {
            _purchaseController.Build<PurchaseScrollData, PurchaseScrollItem>(data);
        }
    }
}