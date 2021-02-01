package com.kingsoft.tc.uplugin;

import com.kingsoft.tc.uplugin.util.BatteryInfoUtil;
import com.kingsoft.tc.uplugin.util.BuildInfoUtil;
import com.kingsoft.tc.uplugin.util.CPUInfoUtil;
import com.kingsoft.tc.uplugin.util.MemoryInfoUtil;

public class PlatformPlugin {
    public static final String LOG_TAG = "PlatformPlugin";
    public static final int PLUGIN_VERSION = 1;

    public static float getBatteryTemperature() {
        return BatteryInfoUtil.getTemperature();
    }

    public static float getBatteryRate() {
        return BatteryInfoUtil.getRate();
    }

    public static int getBatteryChangingStatus() {
        return BatteryInfoUtil.getChangingStatus();
    }

    public static String getBatteryInfo() {
        return BatteryInfoUtil.getBatteryInfo();
    }

    public static String getBuildInfo() {
        return BuildInfoUtil.getBuildInfo();
    }

    public static long getMemoryPss() {
        return MemoryInfoUtil.getPSS();
    }

    public static String getMemoryInfo() {
        return MemoryInfoUtil.getMemoryInfo();
    }

    public static float getCPUUsageRate() {
        return CPUInfoUtil.getUsageRate();
    }

    public static long[] getCPUCoreFrequence() {
        return CPUInfoUtil.getEveryCoreFrequence();
    }

    public static String getCPUInfo() {
        return CPUInfoUtil.getCPUInfo();
    }
}
