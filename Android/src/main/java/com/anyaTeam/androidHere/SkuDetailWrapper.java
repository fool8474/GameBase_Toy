package com.anyaTeam.androidHere;

import com.android.billingclient.api.SkuDetails;
import com.google.gson.JsonObject;

public class SkuDetailWrapper {
    public String ProductId = "";
    public String Price = "";
    public String FormattedPrice = "";
    public String CurrencyCode = "";

    public SkuDetailWrapper(SkuDetails detail)
    {
        if(detail == null)
        {
            return;
        }

        ProductId = detail.getSku();
        Price = AmountMicrosToString(detail.getPriceAmountMicros());
        FormattedPrice = detail.getPrice();
        CurrencyCode = detail.getPriceCurrencyCode();
    }

    public String AmountMicrosToString(long priceLong)
    {
        float f = ((float) priceLong) / 10000.0f;
        f = Math.round(f) / 100.0f;
        if(f == (long)f)
        {
            return String.format("%d", (long)f);
        }
        else
        {
            return String.format("%s", f);
        }
    }

    @Override
    public String toString() {
        JsonObject skusJson = new JsonObject();
        skusJson.addProperty("productId", ProductId);
        skusJson.addProperty("price", Price);
        skusJson.addProperty("formattedPrice", FormattedPrice);
        skusJson.addProperty("currencyCode", CurrencyCode);

        return skusJson.toString();
    }
}
