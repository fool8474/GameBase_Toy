using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Michsky.UI.ModernUIPack;
using Script.Inject;
using Script.Manager;
using Script.UI.Component;
using Script.UI.Controllers;
using Script.UI.Models;
using Script.UI.ScrollItems;
using UniRx;
using UnityEngine;

namespace Script.UI.Views
{
    public class TestView : View<TestModel>
    {
        [SerializeField] private ScrollItemController _itemController;
        [SerializeField] private AnButton _firstPosBtn;
        [SerializeField] private AnButton _lastPosBtn;
        [SerializeField] private ContextMenuContent _contextMenuContent;
        [SerializeField] private AnButton _showPopupBtn;

        private PopupMgr _popupMgr;

        public override void Initialize()
        {
            base.Initialize();
            BindEvent();
            
            _itemController.Init();
        }

        public override void Inject(Model model)
        {
            base.Inject(model);
            _popupMgr = Injector.GetInstance<PopupMgr>();
        }

        private void BindEvent()
        {
            _firstPosBtn.OnClickAsObservable()
                .Subscribe(_ => SetScrollPos(0))
                .AddTo(_disposable);

            _lastPosBtn.OnClickAsObservable()
                .Subscribe(_ => SetScrollPos(_itemController.GetScrollItemList().Count - 1))
                .AddTo(_disposable);

            _showPopupBtn.OnClickAsObservable()
                .Subscribe(_ => ShowPopup())
                .AddTo(_disposable);

            _model.ScrollDataProperty
                .Subscribe(UpdateTestScrollItemData)
                .AddTo(_disposable);
        }

        private void ShowPopup()
        {
            _popupMgr.ShowPopup<TestControllerPopup3>();
        }
        
        private void SetScrollPos(int value)
        {
            _itemController.SetScrollIdx(value);
        }
        
        private void UpdateTestScrollItemData(List<TestScrollData> data)
        {
            _itemController.Build<TestScrollData, TestScrollItem>(data);
        }

        protected override void FinalizeHide()
        {
            base.FinalizeHide();
            _itemController.Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            _itemController.Dispose();
        }
    }
}