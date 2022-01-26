using Script.Manager.CSV;

namespace Script.Table
{
    public class DefReward : DefBase
    {
        public DefRewardMoney MoneyReward;

        private int _moneyRewardId;
        public DefReward(int id, int moneyRewardId) : base(id)
        {
            _moneyRewardId = moneyRewardId;
        }

        public override void Build()
        {
            base.Build();

            if (_moneyRewardId != 0)
            {
                TableMgr.TryGetDef(_moneyRewardId, out MoneyReward);
            }
        }
    }

    public class TblReward : TblBase
    {
        public int MoneyRewardId { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defData = new DefReward(Id, MoneyRewardId);
            return (Id, defData);
        }
    }
}
