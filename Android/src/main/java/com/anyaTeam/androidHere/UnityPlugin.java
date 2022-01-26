package com.anyaTeam.androidHere;

import android.app.Activity;
import android.os.Bundle;
import android.os.Process;
import android.util.Log;
import android.widget.Toast;

import androidx.annotation.Nullable;

import com.android.billingclient.api.BillingClientStateListener;
import com.android.billingclient.api.Purchase;
import com.unity3d.player.IUnityPlayerLifecycleEvents;
import com.unity3d.player.UnityPlayer;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;

public class UnityPlugin extends Activity implements IUnityPlayerLifecycleEvents
{
    private static UnityPlugin _instance;
    private static Activity _context;

    private BillingClientStateListener _billBillingClientStateListener;
    private boolean _isBillingAvailable;
    private BillingImplement _billingImplement;

    public static UnityPlugin Instance()
    {
        if(_instance == null)
        {
            Init();
        }

        return _instance;
    }

    public static void Init()
    {
        _instance = new UnityPlugin();
        _context = UnityPlayer.currentActivity;
    }

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        Log.println(Log.DEBUG, "Android", "OnCreate Called");
        super.onCreate(savedInstanceState);
    }

    private void InitPurchaseSystem()
    {
        IPurchaseListener purchaseListener = new IPurchaseListener() {
            @Override
            public void OnPurchaseSuccess(Purchase purchase) {
                // SendUnity
            }

            @Override
            public void OnPurchaseFailed(int response) {
                // SendUnity
            }

            @Override
            public void OnPurchasePending() {
                // SendUnity
            }

            @Override
            public void OnPurchaseListError() {
                // SendUnity
            }

            @Override
            public void OnRestoreSuccess(Purchase purchase) {
                // SendUnity
            }

            @Override
            public void OnRestoreFailed(int response) {
                // SendUnity
            }

            @Override
            public void OnRestoreListError() {
                // SendUnity
            }

            @Override
            public void OnConsumePurchase(String purchaseToken) {
                // SendUnity
            }

            @Override
            public void OnConsumeFailed(int response) {
                // SendUnity
            }

            @Override
            public void OnPurchaseNoSkus() {
                // SendUnity
            }

            @Override
            public void OnReceiveUnConsumeList(String receiptList) {
                // SendUnity
            }
        };

        IBillingClientListener stateListener = new IBillingClientListener() {
            @Override
            public void OnConnected(List<SkuDetailWrapper> list) {
                _isBillingAvailable = true;

                JSONArray skusArray = new JSONArray();

                for(int i=0; i<list.size(); i++)
                {
                    skusArray.put(list.get(i).toString());
                }

                JSONObject inventory = new JSONObject();
                try
                {
                    inventory.put("inventory", skusArray);
                    UnitySendMessage("OnQueryInventory", inventory.toString());
                }
                catch (JSONException e)
                {
                    OnDisConnected();
                }
            }

            @Override
            public void OnDisConnected() {
                _isBillingAvailable = false;
                UnitySendMessage("OnQueryInventory", "disconnected");
            }
        };

        Log.println(Log.DEBUG, "IAP", "billing Client Init " + this.toString() + " is appContext");
        _billingImplement = new BillingImplement(this, stateListener, purchaseListener);
    }

    public void ShowToast(String text)
    {
        _context.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(_context, text, Toast.LENGTH_LONG).show();
                UnitySendMessage("ShowDebug", text);
            }
        });
    }

    public void ConnectBillingClient(String [] skusArray)
    {
        List<String> skus = new ArrayList<>();
        for(int i=0, len = skusArray.length; i<len; i++)
        {
            skus.add(skusArray[i]);
        }

        Log.println(Log.DEBUG, "IAP", "ConnectBillingClient Called");

        Log.d("IAP", "Connect Client Called");

        for(int i=0; i<skus.size(); i++)
        {
            Log.println(Log.DEBUG, "IAP", "Skus idx " + i + " / id : " + skus.get(i));
        }

        _billingImplement.ConnectClient(skus);
    }

    public void UnitySendMessage(String methodName, String param)
    {
        UnityPlayer.UnitySendMessage("AndroidPlugin", methodName, param);
    }

    @Override
    public void onUnityPlayerUnloaded() {
        moveTaskToBack(true);
    }

    @Override
    public void onUnityPlayerQuitted() {
        Process.killProcess(Process.myPid());
    }
}
