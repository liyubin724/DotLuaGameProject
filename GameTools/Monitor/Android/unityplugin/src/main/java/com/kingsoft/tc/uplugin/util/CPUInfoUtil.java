package com.kingsoft.tc.uplugin.util;

import android.annotation.SuppressLint;

public class CPUInfoUtil {
    private static final String CORE_COUNT_KEY = "coreCount";
    private static final String AVERAGE_FREQUENCE_KEY = "averageFrequence";
    private static final String USAGE_RATE_KEY = "usageRate";

    public static int getCoreCount()
    {
        return ProcCPUInfoUtil.getCoreCount();
    }

    public static float getAverageFrequence()
    {
        return ProcCPUInfoUtil.getAverageFrequence();
    }

    public static long[] getEveryCoreFrequence()
    {
        return ProcCPUInfoUtil.getEveryCoreFrequence();
    }

    public static float getUsageRate() {
        return ProcCPUStatInfoUtil.getUsageRateByCmd();
    }
    
    @SuppressLint("DefaultLocale")
    public static String getCPUInfo()
    {
        StringBuilder infoBuilder = new StringBuilder();
        infoBuilder.append(String.format("%s:%d\n",CORE_COUNT_KEY,getCoreCount()));
        infoBuilder.append(String.format("%s:%f\n",AVERAGE_FREQUENCE_KEY,getAverageFrequence()));
        infoBuilder.append(String.format("%s:%f\n",USAGE_RATE_KEY,getUsageRate()));
        return infoBuilder.toString();
    }
}
