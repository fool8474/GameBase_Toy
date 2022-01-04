using Script.Manager;

namespace Script.UI.Models
{
    public class TestModelPopup2 : Model
    {
        public override void Initialize()
        {
            UIData = new UIData(UIType.POPUP, "UITestPopup2");
        }
    }
}