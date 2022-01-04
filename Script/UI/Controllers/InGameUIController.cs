using Script.InGame;
using Script.UI.Models;
using Script.Util;

namespace Script.UI.Controllers
{
    public class InGameUIController : Controller<InGameUIModel>
    {
        public InGameUIController() : base(AddressableID.INGAME_UI, new InGameUIModel()){}
    }
}