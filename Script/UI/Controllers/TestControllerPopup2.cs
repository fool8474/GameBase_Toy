using Script.UI.Models;

namespace Script.UI.Controllers
{
    public class TestControllerPopup2 : Controller<TestModelPopup2>
    {
        public TestControllerPopup2() : base("UITestPopup2", new TestModelPopup2())
        {
            
        }
    }
}