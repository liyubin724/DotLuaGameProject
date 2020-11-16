using System.IO;
using UnityEditor;
using UnityEngine;

public class TestScreenCapture
{
    [MenuItem("Test/SSC")]
    public static void SSC()
    {
        GrabScreenSwatch(new Rect(0, 0, Screen.currentResolution.width, Screen.currentResolution.height));
        //GrabScreenSwatch(new Rect(0, 0, Screen.width, Screen.height));

        //var texture = ScreenCapture.CaptureScreenshotAsTexture(2);

        //byte[] bytes = texture.EncodeToPNG();
        //File.WriteAllBytes("D:/c2.png", bytes);
    }

    public static Texture GrabScreenSwatch(Rect rect)
    {
        int width = (int)rect.width;
        int height = (int)rect.height;
        int x = (int)rect.x;
        int y = (int)rect.y;
        Vector2 position = new Vector2(x, y);

        Color[] pixels = UnityEditorInternal.InternalEditorUtility.ReadScreenPixel(position, width, height);

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes("D:/t.png", bytes);

        return texture;
    }
}