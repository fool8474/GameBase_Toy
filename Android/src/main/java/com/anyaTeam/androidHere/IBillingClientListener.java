package com.anyaTeam.androidHere;

import java.util.List;

public interface IBillingClientListener {
    void OnConnected(List<SkuDetailWrapper> list);
    void OnDisConnected();
}
