using System;

namespace Script.UI.Presenters
{
    public class TestController : Controller
    {
        public TestController(Model model) : base("UITest", model)
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