package com.kingsoft.tc.uplugin.util;

import android.os.Build;

public class BuildInfoUtil {
    private static final String VERSION_KEY = "androidVersion";
    private static final String SDK_KEY = "sdk";
    private static final String MODE_KEY = "model";
    private static final String DEVICE_KEY = "device";
    private static final String BRAND_KEY = "brand";
    private static final String BOARD_KEY = "board";
    private static final String MANUFACTURER_KEY = "manufacturer";
    private static final String PRODUCT_KEY = "product";
    private static final String DISPLAY_KEY = "display";
    private static final String ID_KEY = "id";
    private static final String SERIAL_KEY = "serial";
    private static final String USER_KEY = "user";

    //Android版本
    public static String getAndroidVersion() {
        return Build.VERSION.RELEASE;
    }

    public static int getSDK() {
        return Build.VERSION.SDK_INT;
    }

    //型号
    public static String getMode() {
        return Build.MODEL;
    }

    public static String getDevice() {
        return Build.DEVICE;
    }

    //品牌
    public static String getBrand() {
        return Build.BRAND;
    }

    //主板名
    public static String getBoard() {
        return Build.BOARD;
    }

    //厂商名
    public static String getManufacturer() {
        return Build.MANUFACTURER;
    }

    //产品名
    public static String getProduct() {
        return Build.PRODUCT;
    }

    public static String getDisplay() {
        return Build.DISPLAY;
    }

    public static String getId() {
        return Build.ID;
    }

    public static String getSerial() {
        return Build.SERIAL;
    }

    public static String getUser() {
        return Build.USER;
    }

    public static String getBuildInfo() {
        StringBuilder buffer = new StringBuilder();
        buffer.append(String.format("%s:%s\n", VERSION_KEY, getAndroidVersion()));
        buffer.append(String.format("%s:%s\n", SDK_KEY, getSDK()));
        buffer.append(String.format("%s:%s\n", MODE_KEY, getMode()));
        buffer.append(String.format("%s:%s\n", DEVICE_KEY, getDevice()));
        buffer.append(String.format("%s:%s\n", BRAND_KEY, getBrand()));
        buffer.append(String.format("%s:%s\n", BOARD_KEY, getBoard()));
        buffer.append(String.format("%s:%s\n", MANUFACTURER_KEY, getManufacturer()));
        buffer.append(String.format("%s:%s\n", PRODUCT_KEY, getProduct()));
        buffer.append(String.format("%s:%s\n", DISPLAY_KEY, getDisplay()));
        buffer.append(String.format("%s:%s\n", ID_KEY, getId()));
        buffer.append(String.format("%s:%s\n", SERIAL_KEY, getSerial()));
        buffer.append(String.format("%s:%s\n", USER_KEY, getUser()));
        return buffer.toString();
    }
}
