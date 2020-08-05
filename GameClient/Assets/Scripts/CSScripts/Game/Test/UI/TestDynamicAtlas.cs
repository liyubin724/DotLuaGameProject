using DotEngine.UI.DynAtlas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TestDynamicAtlas : MonoBehaviour
{
    public Texture2D texture1 = null;
    public Texture2D texture2 = null;
    public Texture2D texture3 = null;
    public Texture2D texture4 = null;

    DynamicAtlasSet dynamicAtlasSet = null;

    public Image image = null;
    void Start()
    {
        dynamicAtlasSet = new DynamicAtlasSet("Icon",2048);

        string dir = @"D:\WorkSpace\DotLuaGameProject\GameClient\Assets\ArtRes\UI\BG\HighQuality\Alpha";
        string[] files = Directory.GetFiles(dir, "*.png", SearchOption.TopDirectoryOnly);
        foreach(var f in files)
        {
            Texture2D t = new Texture2D(1, 1);
            t.LoadImage(File.ReadAllBytes(f), false);
            t.name = Path.GetFileNameWithoutExtension(f);
            Sprite sprite = dynamicAtlasSet.AddTexture(t);
            if(sprite.name == "activity_patern3")
            {
                image.sprite = sprite;
                image.SetNativeSize();
            }

            Destroy(t);
        }

        //GC.Collect();
        //Resources.UnloadUnusedAssets();
        //GC.Collect(0,GCCollectionMode.Forced);
        //Resources.UnloadUnusedAssets();
        //GC.Collect();
        //GC.Collect();
        //dynamicAtlasSet.ClearAtlasSet();

        //dynamicAtlasSet.WriteTo("D:/");
    }

    // Update is called once per frame
    void Update()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Output"))
        {
            dynamicAtlasSet.UnloadUnused();

            dynamicAtlasSet.WriteTo("D:/");
        }
    }
}
