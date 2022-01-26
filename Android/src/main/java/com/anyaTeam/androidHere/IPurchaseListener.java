package com.anyaTeam.androidHere;

import com.android.billingclient.api.Purchase;

public interface IPurchaseListener {
    void OnPurchaseSuccess(Purchase purchase);
    void OnPurchaseFailed(int response);
    void OnPurchasePending();
    void OnPurchaseListError();

    void OnRestoreSuccess(Purchase purchase);
    void OnRestoreFailed(int response);
    void OnRestoreListError();

    void OnConsumePurchase(String purchaseToken);

    void OnConsumeFailed(int response);

    void OnPurchaseNoSkus();
    void OnReceiveUnConsumeList(String receiptList);
}
