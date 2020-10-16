package com.kingsoft.tc.uplugin;

import android.Manifest;
import android.app.Activity;
import android.content.pm.PackageManager;
import android.os.Build;
import android.util.Log;

import com.kingsoft.tc.uplugin.util.BatteryInfoUtil;
import com.kingsoft.tc.uplugin.util.BuildInfoUtil;
import com.kingsoft.tc.uplugin.util.CPUInfoUtil;
import com.kingsoft.tc.uplugin.util.MemoryInfoUtil;

public class PlatformPlugin {
    public static final String LOG_TAG = "PlatformPlugin";
    public static final int PLUGIN_VERSION = 1;

    public static void initPlugin()
    {
        Activity activity = UnityUtil.getUnityActivity();
        Log.e("FFFF","SDKVERSION:"+Build.VERSION.SDK_INT);
        if (Build.VERSION.SDK_INT >= 23) {
            int REQUEST_CODE_PERMISSION_STORAGE = 100;
            String[] permissions = {
                    Manifest.permission.READ_EXTERNAL_STORAGE,
                    Manifest.permission.WRITE_EXTERNAL_STORAGE,

            };

            for (String str : permissions) {
                if (activity.checkSelfPermission(str) != PackageManager.PERMISSION_GRANTED) {
                    activity.requestPermissions(permissions, REQUEST_CODE_PERMISSION_STORAGE);
                    return;
                }
            }
        }
    }

    public static float getBatteryTemperature()
    {
        return BatteryInfoUtil.getTemperature();
    }

    public  static String getBatteryInfo()
    {
        return BatteryInfoUtil.getBatteryInfo();
    }

    public static String getBuildInfo()
    {
        return BuildInfoUtil.getBuildInfo();
    }

    public static String getMemoryInfo()
    {
        return MemoryInfoUtil.getMemoryInfo();
    }

    public static long getMemoryPss()
    {
        return MemoryInfoUtil.getPSS();
    }

    public static float getCPUUsageRate()
    {
        return CPUInfoUtil.getUsageRate();
    }

    public static String getCPUInfo()
    {
        return CPUInfoUtil.getCPUInfo();
    }
}
