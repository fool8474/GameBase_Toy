using Script.Manager;

namespace Script.UI
{
    public class UIData
    {
        public readonly UIType UIType;
        public readonly string Id;

        public UIData(UIType uiType, string id)
        {
            UIType = uiType;
            Id = id;
        }
    }
}