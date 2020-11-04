package com.kingsoft.tc.uplugin.util;

import android.annotation.SuppressLint;
import android.app.ActivityManager;
import android.content.Context;
import android.os.Debug;

import com.kingsoft.tc.uplugin.UnityUtil;

public class MemoryInfoUtil {
    private static final String TOTAL_MEMORY_KEY = "totalMem";
    private static final String AVAILABLE_MEMORY_KEY = "availMem";
    private static final String THRESHOLD_MEMORY_KEY = "threshold";
    private static final String IS_LOW_MEMORY_KEY = "lowMemory";
    private  static  final String PSS_MEMORY_KEY = "PSS";

    private static ActivityManager.MemoryInfo memoryInfo = new ActivityManager.MemoryInfo();

    public static long getAvailableMemory() {
        Context context = UnityUtil.getUnityActivity();
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        am.getMemoryInfo(memoryInfo);

        return memoryInfo.availMem;
    }

    public static long getTotalMemory() {
        Context context = UnityUtil.getUnityActivity();
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        am.getMemoryInfo(memoryInfo);

        return memoryInfo.totalMem;
    }

    public static long getThresholdMemory() {
        Context context = UnityUtil.getUnityActivity();
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        am.getMemoryInfo(memoryInfo);

        return memoryInfo.threshold;
    }

    public static boolean isLowMemory() {
        Context context = UnityUtil.getUnityActivity();
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        am.getMemoryInfo(memoryInfo);

        return memoryInfo.lowMemory;
    }

    public static long getPSS()
    {
        return getPssByDebug();
    }

    @SuppressLint("DefaultLocale")
    public static String getMemoryInfo() {
        Context context = UnityUtil.getUnityActivity();
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        am.getMemoryInfo(memoryInfo);

        return String.format("%s:%d\n%s:%d\n%s:%d\n%s:%b\n%s:%d\n",
                TOTAL_MEMORY_KEY, memoryInfo.totalMem,
                AVAILABLE_MEMORY_KEY, memoryInfo.availMem,
                THRESHOLD_MEMORY_KEY, memoryInfo.threshold,
                IS_LOW_MEMORY_KEY, memoryInfo.lowMemory,
                PSS_MEMORY_KEY,getPSS());
    }

    private static int getPssByDebug()
    {
        Debug.MemoryInfo memoryInfo = new Debug.MemoryInfo();
        Debug.getMemoryInfo(memoryInfo);
        return memoryInfo.getTotalPss();
    }
}
