package com.kingsoft.tc.uplugin.util;

import android.content.Context;
import android.graphics.Point;
import android.os.Build;
import android.util.DisplayMetrics;
import android.view.Display;
import android.view.WindowManager;

public class ScreenInfoUtil {
    private static final String WIDTH_PIXEL_KEY = "widthPixels";
    private static final String HEIGHT_PIXEL_KEY = "heightPixels";
    private static final String DENSITY_KEY = "density";
    private static final String DENSITY_DEFAULT_KEY = "densityDefault";
    private static final String DENSITY_DPI_KEY = "densityDpi";
    private static final String SCALED_DENSITY_KEY = "scaledDensity";
    private static final String XDPI_KEY = "xdpi";
    private static final String YDPI_KEY = "ydpi";
    private static final String SIZE_KEY = "size";

    public static String getScreenInfo(Context mContext) {
        int widthPixels;
        int heightPixels;

        WindowManager w = (WindowManager) mContext.getSystemService(Context.WINDOW_SERVICE);

        Display d = w.getDefaultDisplay();
        DisplayMetrics metrics = new DisplayMetrics();
        d.getMetrics(metrics);
        // since SDK_INT = 1;
        widthPixels = metrics.widthPixels;
        heightPixels = metrics.heightPixels;
        // includes window decorations (statusbar bar/menu bar)
        if (Build.VERSION.SDK_INT >= 14 && Build.VERSION.SDK_INT < 17) {
            try {
                widthPixels = (Integer) Display.class.getMethod("getRawWidth").invoke(d);
                heightPixels = (Integer) Display.class.getMethod("getRawHeight").invoke(d);
            } catch (Exception ignored) {
                ignored.printStackTrace();
            }
        }
        // includes window decorations (statusbar bar/menu bar)
        if (Build.VERSION.SDK_INT >= 17) {
            try {
                Point realSize = new Point();
                Display.class.getMethod("getRealSize", Point.class).invoke(d, realSize);
                widthPixels = realSize.x;
                heightPixels = realSize.y;
            } catch (Exception ignored) {
                ignored.printStackTrace();
            }
        }

        return String.format("%s:%s\n%s:%s\n%s:%s\n%s:%s\n%s:%s\n%s:%s\n%s:%s\n%s:%s\n%s:%s\n",
                WIDTH_PIXEL_KEY, widthPixels,
                HEIGHT_PIXEL_KEY, heightPixels,
                DENSITY_KEY, metrics.density,
                DENSITY_DEFAULT_KEY, metrics.DENSITY_DEFAULT,
                DENSITY_DPI_KEY, metrics.densityDpi,
                SCALED_DENSITY_KEY, metrics.scaledDensity,
                XDPI_KEY, metrics.xdpi,
                YDPI_KEY, metrics.ydpi,
                SIZE_KEY, Math.sqrt(Math.pow(widthPixels, 2) + Math.pow(heightPixels, 2)) / metrics.densityDpi);
    }
}
