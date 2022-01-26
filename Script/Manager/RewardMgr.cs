using Script.Inject;
using Script.Manager.ManagerType;
using Script.Manager.Player;
using Script.Table;

namespace Script.Manager
{
    public class RewardMgr : ScriptMgr
    {
        PlayerMoneyHolder _moneyHolder;

        public override void Inject()
        {
            _moneyHolder = Injector.GetInstance<PlayerStatus>().GetHolder<PlayerMoneyHolder>();
        }

        public void GetReward(DefReward defReward)
        {
            if (defReward.MoneyReward != null)
            {
                var rewardList = defReward.MoneyReward.RewardDatas;

                foreach (var reward in rewardList)
                    _moneyHolder.AddMoney(reward.Type, reward.Value);
            }
        }
    }
}
