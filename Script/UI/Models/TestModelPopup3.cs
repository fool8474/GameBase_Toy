using Script.Manager;

namespace Script.UI.Models
{
    public class TestModelPopup3 : Model
    {
        public override void Initialize()
        {
            UIData = new UIData(UIType.POPUP, "UITestPopup3");
        }
    }
}