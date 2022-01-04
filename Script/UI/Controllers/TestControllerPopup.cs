using System.Collections.Generic;
using Script.UI.Models;

namespace Script.UI.Controllers
{
    public class TestControllerPopupInitData : UIInitData
    {
        public readonly List<string> InitList;

        public TestControllerPopupInitData(List<string> initList)
        {
            InitList = initList;
        }
    }
    
    public class TestControllerPopup : Controller<TestModelPopup>
    {
        private TestControllerPopup3 _testControllerPopup3;
        
        public TestControllerPopup() : base("UITestPopup", new TestModelPopup())
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
            SetChild();
        }

        private void SetChild()
        {
            if (_uiMgr.TryGetController(out _testControllerPopup3) == false)
            {
                _testControllerPopup3 = new TestControllerPopup3();
            }
            AddChildUI(typeof(TestControllerPopup3), _testControllerPopup3);
        }

        public override void InitializeWithData()
        {
            base.InitializeWithData();

            if (_initData is TestControllerPopupInitData data)
            {
                _model.InitList.Value = data.InitList;
            }
        }
    }
}