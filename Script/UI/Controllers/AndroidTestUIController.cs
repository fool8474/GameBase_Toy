using Script.Manager.CSV;
using Script.Table;
using Script.UI.Models;
using Script.UI.ScrollItems;
using Script.Util;
using System.Collections.Generic;

namespace Script.UI.Controllers
{
    public class AndroidTestUIController : Controller<AndroidTestUIModel>
    {
        public AndroidTestUIController() : base(AddressableID.ANDROID_TEST_UI, new AndroidTestUIModel()) { }

        public override void InitializeWithVisible()
        {
            base.InitializeWithVisible();
            GetPurchaseData();
        }

        private void GetPurchaseData()
        {
            if (TableMgr.TryGetDefDic<DefPurchase>(out var purchases) == false)
            {
                return;
            }

            var datas = new List<PurchaseScrollData>();
            foreach(var product in purchases.Values)
            {
                if(!(product is DefPurchase productDef))
                {
                    continue;
                }

                datas.Add(new PurchaseScrollData(productDef));
            }

            _model.PurchaseDatas.Value = datas; 
        }
    }
}