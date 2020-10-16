package com.kingsoft.tc.uplugin.util;

import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.BatteryManager;

import com.kingsoft.tc.uplugin.UnityUtil;

public class BatteryInfoUtil {
    private static final String RATE_KEY = "rate";
    private static final String IS_CHARGING_KEY = "isChanging";
    private static final String TEMPERATURE_KEY = "temperature";

    private static boolean isInited = false;

    private static int temperature = 0;
    private static int level = 0;
    private static int scale = 0;
    private static int status = 0;

    private static void initReceiver()
    {
        if(!isInited)
        {
            BatteryBroadcastReceiver receiver = new BatteryBroadcastReceiver();

            Context context = UnityUtil.getUnityActivity().getApplication();
            IntentFilter iFilter = new IntentFilter(Intent.ACTION_BATTERY_CHANGED);
            context.registerReceiver(receiver, iFilter);

            isInited = true;
        }
    }

    public static float getTemperature()
    {
        initReceiver();

        return temperature / 10.0f;
    }

    @SuppressLint("DefaultLocale")
    public static String getBatteryInfo() {
        initReceiver();

        return String.format("%s:%f\n%s:%b\n%s:%f",
                RATE_KEY, (level>0&&scale>0)?level/(float)scale:0.0f,
                IS_CHARGING_KEY, (status== BatteryManager.BATTERY_STATUS_CHARGING || status == BatteryManager.BATTERY_STATUS_FULL),
                TEMPERATURE_KEY, temperature / 10.0f
        );
    }

    static class BatteryBroadcastReceiver extends BroadcastReceiver {
        @Override
        public void onReceive(Context context, Intent intent) {
            String action = intent.getAction();
            if (Intent.ACTION_BATTERY_CHANGED.equals(action)) {
                level = intent.getIntExtra(BatteryManager.EXTRA_LEVEL, 0);    ///电池剩余电量
                scale = intent.getIntExtra(BatteryManager.EXTRA_SCALE, 0);  ///获取电池满电量数值
                status = intent.getIntExtra(BatteryManager.EXTRA_STATUS, BatteryManager.BATTERY_STATUS_UNKNOWN); ///获取电池状态
                temperature = intent.getIntExtra(BatteryManager.EXTRA_TEMPERATURE, 0);  ///获取电池温度
            }
        }
    };
}
