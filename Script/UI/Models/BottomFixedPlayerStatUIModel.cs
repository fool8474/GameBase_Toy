using Script.Event;
using Script.Manager;
using Script.Manager.Player;
using Script.UI.FixedUISetters;
using Script.Util;

namespace Script.UI.Models
{
    public class BottomFixedPlayerStatUIModel : Model
    {
        public IEventCommand<BottomFixedPlayerStatUISetter> SetUISetter => _setUISetter;
        public IEventCommand<MoneyType> UpdateNewValue => _updateNewValue;
        
        private EventCommand<BottomFixedPlayerStatUISetter> _setUISetter = new EventCommand<BottomFixedPlayerStatUISetter>();
        private EventCommand<MoneyType> _updateNewValue = new EventCommand<MoneyType>();

        public override void Initialize()
        {
            UIData = new UIData(UIType.FIXED, AddressableID.BOTTOM_FIXED_PLAYER_STAT_UI);
        }

        public override void Dispose()
        {
            base.Dispose();

            _setUISetter.Dispose();
            _updateNewValue.Dispose();
        }
    }
}