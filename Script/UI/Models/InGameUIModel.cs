using Script.Manager;
using Script.Util;

namespace Script.UI.Models
{
    public class InGameUIModel : Model
    {
        public override void Initialize()
        {
            UIData = new UIData(UIType.MAIN, AddressableID.INGAME_UI);
        }
    }
}