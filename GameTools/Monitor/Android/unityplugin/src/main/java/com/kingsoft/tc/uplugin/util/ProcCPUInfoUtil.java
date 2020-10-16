package com.kingsoft.tc.uplugin.util;

import android.util.Log;

import com.kingsoft.tc.uplugin.PlatformPlugin;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;

public class ProcCPUInfoUtil {
    private static final String PROC_CPU_INFO_FILE_PATH = "/proc/cpuinfo";
    private static final String SCALING_CUR_FREQ_FILE_PATH="/sys/devices/system/cpu/cpu%s/cpufreq/scaling_cur_freq";

    public static String getProcCPUInfo() {
        StringBuilder stringBuilder = new StringBuilder();
        try {
            String line;
            BufferedReader br = new BufferedReader(new FileReader(PROC_CPU_INFO_FILE_PATH));
            while ((line = br.readLine()) != null) {
                stringBuilder.append(line + "\n");
            }
            br.close();
        } catch (IOException e) {
            Log.e(PlatformPlugin.LOG_TAG, "Read File Error,filePath = " + PROC_CPU_INFO_FILE_PATH, e);
        }
        return stringBuilder.toString();
    }

    public static int getCoreCount() {
        try {
            String line;
            int count = 0;
            BufferedReader br = new BufferedReader(new FileReader(PROC_CPU_INFO_FILE_PATH));
            while ((line = br.readLine()) != null) {
                if (line.contains("processor")) {
                    count++;
                }
            }
            br.close();

            return count;
        } catch (IOException e) {
            Log.e(PlatformPlugin.LOG_TAG, "Read File Error,filePath = " + PROC_CPU_INFO_FILE_PATH, e);
        }
        return 1;
    }

    public static float getAverageFrequence()
    {
        int coreCount = getCoreCount();
        int freqSum = 0;
        for(int i =0;i<coreCount;++i)
        {
            freqSum+=GetCurrentFrequence(i);
        }
        return freqSum/coreCount;
    }

    public static String getCoreFrequence()
    {
        int coreCount = getCoreCount();
        StringBuilder infoBuilder = new StringBuilder();
        for(int i =0;i<coreCount;++i)
        {
            infoBuilder.append(GetCurrentFrequence(i)+" ");
        }
        return infoBuilder.toString();
    }

    private static int GetCurrentFrequence(int coreIndex)
    {
        String filePath = String.format(SCALING_CUR_FREQ_FILE_PATH,coreIndex);
        try {
            BufferedReader br = new BufferedReader(new FileReader(filePath));
            String line = br.readLine();
            if(line!=null)
            {
                line = line.trim();
                return Integer.parseInt(line);
            }
            br.close();
        } catch (IOException e) {
            Log.e(PlatformPlugin.LOG_TAG, "Read File Error,filePath = " + filePath, e);
        }
        return 0;
    }
}
