package com.anyaTeam.androidHere;

import android.app.Activity;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.android.billingclient.api.BillingClient;
import com.android.billingclient.api.BillingClientStateListener;
import com.android.billingclient.api.BillingFlowParams;
import com.android.billingclient.api.BillingResult;
import com.android.billingclient.api.Purchase;
import com.android.billingclient.api.PurchasesUpdatedListener;
import com.android.billingclient.api.SkuDetails;
import com.android.billingclient.api.SkuDetailsParams;
import com.android.billingclient.api.SkuDetailsResponseListener;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class BillingImplement
{
    private Activity _appContext;
    private BillingClient _billingClient;
    private IBillingClientListener _clientStateListener;
    private IPurchaseListener  _purchaseListener;
    private Map<String, SkuDetails> _skuDetailMap;
    private InAppUpdateType _updateType = InAppUpdateType.PURCHASE;

    private boolean _isAvailable;

    public enum InAppUpdateType
    {
        NONE,
        PURCHASE,
        RESTORE
    }

    public BillingImplement(UnityPlugin appContext, IBillingClientListener stateListener, IPurchaseListener purchaseListener)
    {
        _appContext = appContext;
        _clientStateListener = stateListener;

        _billingClient = BillingClient.newBuilder(_appContext)
                .setListener(new PurchasesUpdatedListener() {
                    @Override
                    public void onPurchasesUpdated(@NonNull BillingResult billingResult, @Nullable List<Purchase> list) {
                        OnPurchaseUpdated(billingResult, list);
                    }
                }).enablePendingPurchases().build();

        _skuDetailMap = new HashMap<>();
    }

    private void OnPurchaseUpdated(@NonNull BillingResult billingResult, @Nullable List<Purchase> list)
    {
        return;
    }

    public void ConnectClient(List<String> skus)
    {
        SkuDetailsParams.Builder skuParams = SkuDetailsParams.newBuilder();
        skuParams.setSkusList(skus).setType(BillingClient.SkuType.INAPP);

        _billingClient.startConnection(new BillingClientStateListener() {
            @Override
            public void onBillingSetupFinished(@NonNull BillingResult billingResult) {
                _billingClient.querySkuDetailsAsync(skuParams.build(), new SkuDetailsResponseListener() {
                    @Override
                    public void onSkuDetailsResponse(@NonNull BillingResult billingResult, @Nullable List<SkuDetails> list) {
                        if(list == null)
                        {
                            _clientStateListener.OnDisConnected();
                            return;
                        }

                        List<SkuDetailWrapper> wrapper = new ArrayList<>();
                        for(int i=0, len = list.size(); i<len; i++)
                        {
                            SkuDetails detail = list.get(i);
                            wrapper.add(new SkuDetailWrapper(detail));
                            _skuDetailMap.put(detail.getSku(), detail);
                        }

                        _isAvailable = true;
                        _clientStateListener.OnConnected(wrapper);
                    }
                });
            }

            @Override
            public void onBillingServiceDisconnected() {
                _clientStateListener.OnDisConnected();
            }
        });
    }

    public void SetUpdateType(InAppUpdateType type)
    {
        _updateType = type;
    }

    public void LaunchPurchase(String sku, String obfuscatedId) {
        _updateType = InAppUpdateType.PURCHASE;
        SkuDetails detail = _skuDetailMap.get(sku);

        if(detail == null || _isAvailable == false)
        {
            _purchaseListener.OnPurchaseNoSkus();
            return;
        }

        BillingFlowParams flowParams = BillingFlowParams.newBuilder()
                .setSkuDetails(detail).setObfuscatedAccountId(obfuscatedId).build();
        _billingClient.launchBillingFlow(_appContext, flowParams);
    }
}

