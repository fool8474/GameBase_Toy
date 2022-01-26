using Script.Manager.CSV;
using Script.Manager.Player;
using Script.Manager.Util.Log;
using Script.Util;
using System.Collections.Generic;

namespace Script.Table
{
    public class RewardMoney
    {
        public MoneyType Type { get; }
        public float Value { get; }
        public RewardMoney(MoneyType type, float value)
        {
            Type = type;
            Value = value;
        }
    }

    public class DefRewardMoney : DefBase
    {
        public List<RewardMoney> RewardDatas;

        private string _moneyTypeList;
        private string _countList;
        public DefRewardMoney(int id, string moneyTypeList, string countList) : base(id)
        {
            _moneyTypeList = moneyTypeList;
            _countList = countList;
        }

        public override void Build()
        {
            var typeList = StringUtil.SplitToList<MoneyType>(new char[] { '/' }, _moneyTypeList);
            var valueList = StringUtil.SplitToList<float>(new char[] { '/' }, _countList);

            if (typeList.Count != valueList.Count)
            {
                Log.DF(LogCategory.TABLE, "DefRewardMoney Type & Value not equal Count, Id {0}", Id);
            }

            RewardDatas = new List<RewardMoney>();
            for (var i = 0; i < typeList.Count; i++)
            {
                RewardDatas.Add(new RewardMoney(typeList[i], valueList[i]));
            }
        }
    }

    public class TblRewardMoney : TblBase
    {
        public string MoneyTypeList { get; set; }
        public string CountList { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defData = new DefRewardMoney(Id, MoneyTypeList, CountList);
            return (Id, defData);
        }
    }
}
