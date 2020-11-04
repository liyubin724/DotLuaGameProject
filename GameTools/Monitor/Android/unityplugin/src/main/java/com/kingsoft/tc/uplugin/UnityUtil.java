package com.kingsoft.tc.uplugin;

import android.app.Activity;
import android.util.Log;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Objects;

public class UnityUtil {

    private static Activity unityActivity = null;

    public static Activity getUnityActivity() {
        if (unityActivity == null) {
            try {
                Class<?> classType = Class.forName("com.unity3d.player.UnityPlayer");
                unityActivity = (Activity) classType.getDeclaredField("currentActivity").get(classType);
            } catch (ClassNotFoundException | IllegalAccessException | NoSuchFieldException e) {
                Log.e(PlatformPlugin.LOG_TAG, Objects.requireNonNull(e.getMessage()));
            }
        }
        return unityActivity;
    }

    public static boolean SendMessage(String gameObjectName, String functionName, String arg) {
        try {
            Class<?> classType = Class.forName("com.unity3d.player.UnityPlayer");
            Method method = classType.getMethod("UnitySendMessage", String.class, String.class, String.class);
            method.invoke(classType, gameObjectName, functionName, arg);
            return true;
        } catch (ClassNotFoundException | NoSuchMethodException | IllegalAccessException | InvocationTargetException e) {
            Log.e(PlatformPlugin.LOG_TAG, Objects.requireNonNull(e.getMessage()));
        }
        return false;
    }
}
