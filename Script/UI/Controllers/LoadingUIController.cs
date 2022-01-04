using Script.Manager;
using Script.UI.Models;
using Script.Util;

namespace Script.UI.Controllers
{
    public class LoadingUIController : Controller<LoadingUIModel>
    {
        private UITransitionMgr _uiTransitionMgr;
        
        public LoadingUIController() : base(AddressableID.LOADING_UI, new LoadingUIModel())
        {
        }
    }
}