using Script.Manager.CSV;
using Script.Manager.Util.Log;
using System;
using System.Linq;
using UnityEngine.Purchasing;

namespace Script.Table
{
    public class DefPurchase : DefBase
    {
        public readonly string InAppId;
        public readonly string Image;
        public readonly ProductType InAppType;
        public readonly float Price;
        public DefReward RewardData;

        private readonly int _rewardId;

        public DefPurchase(int id, string inAppId, string image, string inAppType, float price, int rewardId) : base(id)
        {
            InAppId = inAppId;
            Image = image;
            InAppType = (ProductType)Enum.Parse(typeof(ProductType),  inAppType);
            Price = price;

            _rewardId = rewardId;
        }

        public override void Build()
        {
            if(TableMgr.TryGetDef(_rewardId, out RewardData) == false)
            {
                Log.EF(LogCategory.TABLE, "Cannot load reward from purchase {0}", Id);
            }
        }
    }

    public class TblPurchase : TblBase
    {
        public string InAppId { get; set; }
        public string Image { get; set; }
        public string InAppType { get; set; }
        public float Price { get; set; }
        public int RewardId { get; set; }

        public override (int id, DefBase def) Build()
        {
            var defBlockTypeSet = new DefPurchase(Id, InAppId, Image, InAppType, Price, RewardId);
            return (Id, defBlockTypeSet);
        }
    }
}
