using System.Collections.Generic;
using Script.Event;
using Script.Manager;

namespace Script.UI.Models
{
    public class TestModelPopup : Model
    {
        public IEventProperty<List<string>> InitList => _initList;
        private EventProperty<List<string>> _initList = new EventProperty<List<string>>();
        
        public override void Initialize()
        {
            UIData = new UIData(UIType.POPUP, "UITestPopup");
        }

        public override void Dispose()
        {
            _initList.Value.Clear();
            _initList = null;
            
            base.Dispose();
        }
    }
}