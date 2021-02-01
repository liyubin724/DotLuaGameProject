package com.kingsoft.tc.uplugin.util;

import android.os.Process;
import android.util.Log;

import com.kingsoft.tc.uplugin.PlatformPlugin;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class ProcCPUStatInfoUtil {
    private static final String PROC_STAT_INFO_FILE_PATH = "/proc/stat";

    private static CPUUsageStat getUsageStat() {
        CPUUsageStat stat = new CPUUsageStat();
        try {
            String line;
            BufferedReader br = new BufferedReader(new FileReader(PROC_STAT_INFO_FILE_PATH));
            while ((line = br.readLine()) != null) {
                String[] contents = line.split("\\s+");

                Log.e("FFFF", line);

                if (contents.length == 11 && contents[0].toLowerCase() == "cpu") {

                    Log.e("FFFF", "SSSSSSSSSSSSSSSSSS");

                    stat.user = Long.parseLong(contents[1]);
                    stat.nice = Long.parseLong(contents[2]);
                    stat.system = Long.parseLong(contents[3]);
                    stat.idle = Long.parseLong(contents[4]);
                    stat.iowait = Long.parseLong(contents[5]);
                    stat.irq = Long.parseLong(contents[6]);
                    stat.softirq = Long.parseLong(contents[7]);
                    stat.stealstolen = Long.parseLong(contents[8]);
                    stat.guest = Long.parseLong(contents[9]);
                    stat.guest_nice = Long.parseLong(contents[10]);
                    break;
                }
            }
            br.close();
        } catch (IOException e) {
            Log.e(PlatformPlugin.LOG_TAG, "Read File Error,filePath = " + PROC_STAT_INFO_FILE_PATH, e);
        }

        return stat;
    }

    public static float getUsageRateByStat() {
        CPUUsageStat state1 = getUsageStat();
        try {
            Thread.sleep(1000);
        } catch (InterruptedException e) {
            //e.printStackTrace();
        }
        CPUUsageStat stat2 = getUsageStat();
        long deltaTotalTime = stat2.getTotalTime() - state1.getTotalTime();
        long deltaNonIdleTime = stat2.getNonIdleTime() - state1.getNonIdleTime();
        if (deltaTotalTime == 0) {
            return 0.0f;
        } else {
            return deltaNonIdleTime / (float) deltaTotalTime;
        }
    }

    private static int getCPUIndex(String line)
    {
        if(line.contains("CPU"))
        {
            String[] titles = line.split("\\s+");
            for(int i =0;i<titles.length;++i)
            {
                if(titles[i].contains("CPU"))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public static float getUsageRateByCmd() {
        java.lang.Process process = null;
        StringBuilder infoBuilder = new StringBuilder();
        try {
            process = Runtime.getRuntime().exec("top -n 1");
            BufferedReader reader = new BufferedReader(new InputStreamReader(process.getInputStream()));
            String line;
            int cpuIndex = -1;
            while ((line = reader.readLine()) != null) {
                line = line.trim();
                if(line.length()==0)
                {
                    continue;
                }
                int tempIndex = getCPUIndex(line);
                if(tempIndex!=-1)
                {
                    cpuIndex = tempIndex;
                    continue;
                }
                if(line.startsWith(String.valueOf(Process.myPid())) && cpuIndex!=-1)
                {
                    String[] contents = line.split("\\s+");
                    if(contents.length<=cpuIndex)
                    {
                        continue;
                    }
                    String cpuContent = contents[cpuIndex];
                    if(cpuContent.endsWith("%"))
                    {
                        cpuContent = cpuContent.substring(0,cpuContent.lastIndexOf("%"));
                    }
                    return Float.parseFloat(cpuContent) / Runtime.getRuntime().availableProcessors()/100;
                }
            }
            reader.close();
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            if (process != null) {
                process.destroy();
            }
        }
        return 0.0f;
    }

    public static class CPUUsageStat {
        public long user;
        public long nice;
        public long system;
        public long idle;
        public long iowait;
        public long irq;
        public long softirq;
        public long stealstolen;
        public long guest;
        public long guest_nice;

        public long getTotalTime() {
            return user + nice + system + idle + iowait + irq + softirq + stealstolen + guest_nice + guest;
        }

        public long getIdleTime() {
            return idle;
        }

        public long getNonIdleTime() {
            return getTotalTime() - getIdleTime();
        }
    }
}