using Script.UI.Models;
using Script.Util;

namespace Script.UI.Controllers
{
    public class TestHeavyController : Controller<TestHeavyModel>
    {
        public TestHeavyController() : base(AddressableID.HEAVY_TEST_UI, new TestHeavyModel())
        {
            
        }
    }
}