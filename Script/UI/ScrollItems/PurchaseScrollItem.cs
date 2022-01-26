using Script.Inject;
using Script.Manager;
using Script.Manager.Util.Log;
using Script.Table;
using Script.UI.Component;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.ScrollItems
{
    public class PurchaseScrollData : ScrollData
    {
        public DefPurchase PurchaseData;

        public PurchaseScrollData(DefPurchase data)
        {
            PurchaseData = data;
        }
    }
    
    public class PurchaseScrollItem : ScrollItem
    {
        [SerializeField] private AnButton _btnPurchase;
        [SerializeField] private TextMeshProUGUI _txtName;
        [SerializeField] private Image _image;

        private PurchaseScrollData _purchaseData;
        private InAppMgrUnity _inAppMgr;
        private AtlasMgr _atlasMgr;

        public override void Init(ScrollData data)
        {
            base.Init(data);
            
            _inAppMgr = Injector.GetInstance<InAppMgrUnity>();
            _atlasMgr = Injector.GetInstance<AtlasMgr>();
            
            _btnPurchase
                .OnClickAsObservable()
                .Subscribe(_ => Purchase())
                .AddTo(_disposable);
        }
        
        private void Purchase()
        {
            _inAppMgr.Purchase(_purchaseData.PurchaseData);
        }

        public override void UpdateData(ScrollData data)
        {
            base.UpdateData(data);

            if (!(data is PurchaseScrollData pData))
            {
                Log.EF(LogCategory.UI, "Not TestScrollData, {0}", gameObject.name);
                return;
            }

            _txtName.text = pData.PurchaseData.Id.ToString();
            _purchaseData = pData;
        }
    }
}