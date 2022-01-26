using System.Collections.Generic;
using Script.Event;
using Script.Manager;
using Script.UI.ScrollItems;

namespace Script.UI.Models
{
    public class TestModel : Model
    {
        public IEventProperty<List<TestScrollData>> ScrollDataProperty => _scrollDataProperty;
        private readonly EventProperty<List<TestScrollData>> _scrollDataProperty = new EventProperty<List<TestScrollData>>();

        public override void Initialize()
        {
            UIData = new UIData(UIType.MAIN, "UITest");
        }

        public override void Dispose()
        {
            base.Dispose();
            _scrollDataProperty.Dispose();
        }
    }
}
