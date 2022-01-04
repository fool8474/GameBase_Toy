using Script.Manager;
using Script.Util;

namespace Script.UI.Models
{
    public class TestHeavyModel : Model
    {
        public override void Initialize()
        {
            UIData = new UIData(UIType.MAIN, AddressableID.HEAVY_TEST_UI);
        }
    }
}
