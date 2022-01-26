using Script.Inject;
using Script.Manager;
using Script.Manager.CSV;
using Script.Manager.Player;
using Script.Manager.Util.Log;
using Script.Table;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI.ExtraUI
{
    public class PlayerMoneyStatUI : MonoBehaviour
    {
        [SerializeField] private MoneyType _moneyType;
        [SerializeField] private Image _moneyIcon;
        [SerializeField] private TextMeshProUGUI _moneyValue;

        private PlayerMoneyHolder _moneyHolder;

        public void InitData()
        {
            if (TableMgr.TryGetDef<DefMoneyType>((int)_moneyType, out var rtnDef) == false)
            {
                Log.EF(LogCategory.TABLE, "Cannot get moneyType from table {0}", _moneyType);
                return;
            }

            if(!(rtnDef is DefMoneyType defMoneyType))
            {
                return;
            }

            _moneyHolder = Injector.GetInstance<PlayerStatus>().GetHolder<PlayerMoneyHolder>();
            _moneyIcon.sprite = Injector.GetInstance<AtlasMgr>().GetSprite(AtlasType.UI_ICON, defMoneyType.SmallIconName);
            UpdateValue();
        }

        public void UpdateValue()
        {
            _moneyValue.text = _moneyHolder.GetMoney(_moneyType).ToString();
        }
    }
}
