using System;

namespace Script.UI.Presenters
{
    public class TestControllerPopup : Controller
    {
        public TestControllerPopup(Model model) : base("UITestPopup", model)
        {
            
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void InitializeWithVisible()
        {
            base.InitializeWithVisible();
        }
        
        public override void InitializeWithData(UIInitData data)
        {
            base.InitializeWithData(data);
        }
        
        public override void OnQuit()
        {
            base.OnQuit();
        }


    }
}