using Script.Event;
using Script.Manager;
using Script.UI.ScrollItems;
using Script.Util;
using System.Collections.Generic;

namespace Script.UI.Models
{
    public class AndroidTestUIModel : Model
    {
        public IEventProperty<List<PurchaseScrollData>> PurchaseDatas => _purchaseDatas;
        private readonly EventProperty<List<PurchaseScrollData>> _purchaseDatas = new EventProperty<List<PurchaseScrollData>>();

        public override void Initialize()
        {
            UIData = new UIData(UIType.MAIN,  AddressableID.ANDROID_TEST_UI);
        }

        public override void Dispose()
        {
            base.Dispose();
            _purchaseDatas.Dispose();
        }
    }
}