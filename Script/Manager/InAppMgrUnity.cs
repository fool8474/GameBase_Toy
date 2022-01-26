using Script.Inject;
using Script.Manager.CSV;
using Script.Manager.ManagerType;
using Script.Manager.Util.Log;
using Script.Table;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Script.Manager
{
    public class InAppMgrUnity : MonoMgr, IStoreListener
    {
        private IStoreController _storeController;          
        private IExtensionProvider _storeExtensionProvider;

        private bool _isFinishedBuyProduct = true;
        private DefReward _targetReward = default;
        private RewardMgr _rewardMgr;

        public override void Initialize()
        {
            if (_storeController == null)
            {
                InitializePurchasing();
            }
        }

        public override void Inject()
        {
            _rewardMgr = Injector.GetInstance<RewardMgr>();
        }

        public void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            if(TableMgr.TryGetDefDic<DefPurchase>(out var products) == false)
            {
                return;
            }

            foreach(var val in products.Values)
            {
                if (val is DefPurchase product)
                {
                    builder.AddProduct(product.InAppId, product.InAppType);
                }
            }
            
            UnityPurchasing.Initialize(this, builder);
        }

        private bool IsInitialized()
        {
            return _storeController != null && _storeExtensionProvider != null;
        }

        public void Purchase(DefPurchase purchaseDef)
        {
            if(_isFinishedBuyProduct == false)
            {
                return;
            }

            _targetReward = purchaseDef.RewardData;
            BuyProductID(purchaseDef.InAppId);
        }
        
        private void BuyProductID(string productId)
        {
            if (IsInitialized() == false)
            {
                Log.EF(LogCategory.IAP, "BuyProductID FAIL. Not initialized.");
                return;
            }

            var product = _storeController.products.WithID(productId);

            if (product == null || product.availableToPurchase == false)
            {
                Log.EF(LogCategory.IAP, "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                return;
            }

            _isFinishedBuyProduct = false;
            _storeController.InitiatePurchase(product);
        }

        public void RestorePurchases()
        {
            if (!IsInitialized())
            {
                Log.EF(LogCategory.IAP, "RestorePurchases FAIL. Not initialized.");
                return;
            }

            // TODO : Restore
            Log.EF(LogCategory.IAP, "RestorePurchases FAIL. Not supported now.");
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _storeExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Log.EF(LogCategory.IAP, "OnInitializeFailed InitializationFailureReason:" + error);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Log.DF(LogCategory.IAP, "Complete Purchase {0}", args.purchasedProduct.definition.id);
            _isFinishedBuyProduct = true;

            _rewardMgr.GetReward(_targetReward);
            _targetReward = default;
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Log.EF(LogCategory.IAP, "OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason);
            _isFinishedBuyProduct = true;
        }
    }
}