using Script.Manager.CSV;
using Script.Manager.Player;
using System;

namespace Script.Table
{
    public class DefMoneyType : DefBase
    {
        public readonly MoneyType Type;
        public readonly string SmallIconName;

        public DefMoneyType(int id, string moneyType, string smallIconName) : base(id) 
        {
            Type = (MoneyType)Enum.Parse(typeof(MoneyType),moneyType);
            SmallIconName =  smallIconName;
        }
    }

    public class TblMoneyType : TblBase
    {
        public string MoneyType { get; set; }
        public string SmallIcon { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defMoneyType = new DefMoneyType(Id, MoneyType, SmallIcon);
            return (Id, defMoneyType);
        }
    }
}
