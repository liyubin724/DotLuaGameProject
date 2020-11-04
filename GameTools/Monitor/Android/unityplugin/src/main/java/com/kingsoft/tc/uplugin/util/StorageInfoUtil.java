package com.kingsoft.tc.uplugin.util;

import android.os.Environment;
import android.os.StatFs;

import java.io.File;

public class StorageInfoUtil {
    private static final String SDCARD_ENABLE_KEY = "sdcardEnable";
    private static final String TOTAL_EXTERNAL_SIZE_KEY = "totalExternalSize";
    private static final String AVAILABLE_EXTERNAL_SIZE_KEY = "availableExternalSize";
    private static final String TOTAL_INTERNAL_SIZE_KEY = "totalInternalSize";
    private static final String AVAILABLE_INTERNAL_SIZE_KEY = "availableInternalSize";

    //判断外部存储SD是否可用 (存在且具有读写权限)
    public static boolean isSDCardEnable() {
        return Environment.getExternalStorageState().equals(
                Environment.MEDIA_MOUNTED);
    }

    public static long getExternalAvailableSize() {
        if (!isSDCardEnable()) {
            return 0;
        }
        File externalFile = Environment.getExternalStorageDirectory();
        StatFs stat = new StatFs(externalFile.getPath());
        long blockSize = stat.getBlockSizeLong();
        long availableBlocks = stat.getAvailableBlocksLong();

        return availableBlocks * blockSize;
    }

    public static long getExternalTotalSize() {
        if (!isSDCardEnable()) {
            return 0;
        }
        File externalFile = Environment.getExternalStorageDirectory();
        StatFs stat = new StatFs(externalFile.getPath());
        long blockSize = stat.getBlockSizeLong();
        long totalBlocks = stat.getBlockCountLong();

        return totalBlocks * blockSize;
    }

    public static String getExternalStorageInfo() {
        boolean isSDCardEnable = isSDCardEnable();
        long blockSize = 0, totalBlocks = 0, availableBlocks = 0;
        if (isSDCardEnable) {
            File externalFile = Environment.getExternalStorageDirectory();
            StatFs stat = new StatFs(externalFile.getPath());
            blockSize = stat.getBlockSizeLong();
            availableBlocks = stat.getAvailableBlocksLong();
            totalBlocks = stat.getBlockCountLong();
        }

        return String.format("%s:%s\n%s:%s\n%s:%s\n",
                SDCARD_ENABLE_KEY, isSDCardEnable,
                AVAILABLE_EXTERNAL_SIZE_KEY, availableBlocks * blockSize,
                TOTAL_EXTERNAL_SIZE_KEY, totalBlocks * blockSize);
    }

    public static long getInternalAvailableSize()
    {
        File path = Environment.getDataDirectory();
        StatFs stat = new StatFs(path.getPath());
        long blockSize = stat.getBlockSizeLong();
        long availableBlocks = stat.getAvailableBlocksLong();
        return blockSize*availableBlocks;
    }

    public  static long getInternalTotalSize()
    {
        File path = Environment.getDataDirectory();
        StatFs stat = new StatFs(path.getPath());
        long blockSize = stat.getBlockSizeLong();
        long totalBlocks = stat.getBlockCountLong();
        return totalBlocks * blockSize;
    }

    // 内部存储空间大小，以字节为单位
    public static String getInternalStorageInfo() {
        File path = Environment.getDataDirectory();
        StatFs stat = new StatFs(path.getPath());
        long blockSize = stat.getBlockSizeLong();
        long totalBlocks = stat.getBlockCountLong();
        long availableBlocks = stat.getAvailableBlocksLong();

        return String.format("%s:%s\n%s:%s\n",
                AVAILABLE_INTERNAL_SIZE_KEY, availableBlocks * blockSize,
                TOTAL_INTERNAL_SIZE_KEY, totalBlocks * blockSize);
    }

    public static String getStorageInfo() {
        StringBuilder infoBuilder = new StringBuilder();
        infoBuilder.append(getExternalStorageInfo());
        infoBuilder.append(getInternalStorageInfo());
        return infoBuilder.toString();
    }
}
