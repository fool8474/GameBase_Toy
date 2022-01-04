using Script.Manager;
using Script.Util;

namespace Script.UI.Models
{
    public class LoadingUIModel : Model
    {
        public override void Initialize()
        {
            UIData = new UIData(UIType.LOADING, AddressableID.LOADING_UI);
        }
    }
}