using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Script.Inject;
using Script.Manager.CSV;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script.Manager
{
    public class SkusInfo
    {
        public string ProductId { get; private set; }
        public int DefId { get; private set; }
        public string PriceCurrencyCode { get; private set; }
        public string Price { get; private set; }
        public string FormattedPrice { get; private set; }

        public SkusInfo(string productId, int defId)
        {
            ProductId = productId;
            DefId = defId;
        }

        public void AddInfo(string priceCurrencyCode, string price, string formattedPrice)
        {
            Price = price;
            FormattedPrice = formattedPrice;
            PriceCurrencyCode = priceCurrencyCode;
        }
    }

    public static class InAppParameter
    {
        public const string SKU_INVENTORY_FAIL = "disconnected";
        public const string SKU_INVENTORY = "inventory";
        public const string SKU_PRODUCT_ID = "pid";
        public const string SKU_PRICE = "price";
        public const string SKU_FORMATTED_PRICE = "formattedPrice";
        public const string SKU_CURRENCY_CODE = "currencyCode";
    }

    public class InAppMgrAndroid : ScriptMgr
    {
        private Dictionary<string, SkusInfo> _skusInfoDic;
        private UnityPlugins _unityPlugins;
        public override void Initialize()
        {
            _skusInfoDic = new Dictionary<string, SkusInfo>();
            GetSkusList();
        }

        public override void Inject()
        {
            _unityPlugins = Injector.GetInstance<UnityPlugins>();
        }

        private void GetSkusList()
        {
            if(TableMgr.TryGetDefDic<DefPurchase>(out var defDic) == false)
            {
                Log.EF(LogCategory.IAP, "Cannot load purchaseDef from tableMgr");
                return;
            }

            Log.DF(LogCategory.IAP, "IAP Purchase count : {0}", defDic.Count);
            foreach(var defData in defDic.Values)
            {
                if(!(defData is DefPurchase defPurchase))
                {
                    continue;
                }

                _skusInfoDic.Add(defPurchase.InAppId, new SkusInfo(defPurchase.InAppId, defPurchase.Id));
            }

            _unityPlugins.AndroidInAppExtension.GetSkusList(_skusInfoDic.Keys.ToArray());
        }

        public void OnQueryInventory(string skusJson)
        {
            var transResult = TransQueryInventoryResult(skusJson);

            foreach (var eachResult in transResult)
            {
                if (_skusInfoDic.TryGetValue(eachResult.Key, out var targetSkus))
                {
                    targetSkus.AddInfo(eachResult.Value.currency, eachResult.Value.price, eachResult.Value.formattedString);
                }
            }
            
            PrintSkusList();
        }

        public void PrintSkusList()
        {
            foreach(var sku in _skusInfoDic.Values)
            {
                Log.DF(LogCategory.IAP, "Get Sku Id {0}, Price {1}, DefId {2}, FormattedPrice With CurrencyCode {3}",
                    sku.ProductId, sku.Price, sku.DefId, sku.PriceCurrencyCode + " " + sku.FormattedPrice);
            }
        }

        private Dictionary<string, (string currency, string price, string formattedString)> TransQueryInventoryResult(string skuDetailsListJson)
        {
            var result = new Dictionary<string, (string currency, string price, string formattedPrice)>();

            if (skuDetailsListJson == InAppParameter.SKU_INVENTORY_FAIL)
            {
                Log.WF(LogCategory.IAP, "Cannot load SKUS from market");
                return result;
            }

            var inventory = JToken.Parse(skuDetailsListJson);
            var inventoryValue = inventory[InAppParameter.SKU_INVENTORY] as JArray;
                
            if (inventoryValue == null)
            {
                return result;
            }

            for (int i = 0; i < inventoryValue.Count; i++)
            {
                var skuDetailWrapper = inventoryValue[i];
                var cleanyDetail = JObject.Parse(skuDetailWrapper.ToString());
                var productId = (string)cleanyDetail[InAppParameter.SKU_PRODUCT_ID];
                var price = (string)cleanyDetail[InAppParameter.SKU_PRICE];
                var formattedPrice = (string)cleanyDetail[InAppParameter.SKU_FORMATTED_PRICE];
                var currency = (string)cleanyDetail[InAppParameter.SKU_CURRENCY_CODE];
                result.Add(productId, (currency, price, formattedPrice));
            }

            return result;
        }
    }
}
