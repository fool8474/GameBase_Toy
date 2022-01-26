using Script.UI.FixedUISetters;
using Script.Event;
using Script.Manager;
using Script.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.UI.Models
{
    public class LobbyNavigationBarModel : Model
    {
        public IEventCommand<LobbyNavigationBarUISetter> SetUISetter => _setUISetter;
        private EventCommand<LobbyNavigationBarUISetter> _setUISetter = new EventCommand<LobbyNavigationBarUISetter>();

        public IEventCommand<UIContentType> MoveEvent => _moveEvent;
        private EventCommand<UIContentType> _moveEvent = new EventCommand<UIContentType>();

        public override void Initialize()
        {
            UIData = new UIData(UIType.FIXED, AddressableID.LOBBY_NAVIGATION_BAR_UI);
        }
        public override void Dispose()
        {
            base.Dispose();

            _setUISetter.Dispose();
            _moveEvent.Dispose();
        }
    }
}