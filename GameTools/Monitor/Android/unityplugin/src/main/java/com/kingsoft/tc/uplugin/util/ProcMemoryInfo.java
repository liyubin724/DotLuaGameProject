package com.kingsoft.tc.uplugin.util;

import android.util.Log;

import com.kingsoft.tc.uplugin.PlatformPlugin;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;

public class ProcMemoryInfo {
    private static final String PROC_MEMORY_INFO_FILE_PATH = "/proc/meminfo";
    private static final String PROC_SELF_STATUS_INFO_FILE_PATH = "/proc/self/status";

    public static String getProcMemoryInfo() {
        StringBuilder infoBuilder = new StringBuilder();
        try {
            String line;
            BufferedReader br = new BufferedReader(new FileReader(PROC_MEMORY_INFO_FILE_PATH));
            int flag = 0;
            while ((line = br.readLine()) != null && flag < 4) {
                line = line.trim();
                if (line.length() > 0) {
                    if (line.startsWith("MemAvailable")
                            || line.startsWith("SwapFree")
                            || line.startsWith("MemFree")
                            || line.startsWith("Cached")) {
                        infoBuilder.append(line);
                        flag++;
                    }
                }
            }
            br.close();
        } catch (IOException e) {
            Log.e(PlatformPlugin.LOG_TAG, "Read File Error,filePath = " + PROC_MEMORY_INFO_FILE_PATH, e);
        }
        return infoBuilder.toString();
    }

    public static String getProcSelfStatusMemoryInfo() {
        StringBuilder infoBuilder = new StringBuilder();
        try {
            String line;
            BufferedReader br = new BufferedReader(new FileReader(PROC_SELF_STATUS_INFO_FILE_PATH));
            int flag = 0;
            while ((line = br.readLine()) != null && flag < 4) {
                line = line.trim();
                if (line.length() > 0) {
                    if (line.startsWith("VmPeak")
                            || line.startsWith("VmSize")
                            || line.startsWith("VmHWM")
                            || line.startsWith("VmRSS")) {
                        infoBuilder.append(line);
                        flag++;
                    }
                }
            }
            br.close();
        } catch (IOException e) {
            Log.e(PlatformPlugin.LOG_TAG, "Read File Error,filePath = " + PROC_SELF_STATUS_INFO_FILE_PATH, e);
        }
        return infoBuilder.toString();
    }
}
