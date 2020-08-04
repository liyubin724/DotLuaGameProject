using DotEngine.UI.DynAtlas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDynamicAtlas : MonoBehaviour
{
    public Texture2D texture1 = null;
    public Texture2D texture2 = null;
    public Texture2D texture3 = null;
    public Texture2D texture4 = null;

    DynamicAtlas dynamicAtlas = null;

    public Texture2D texture;

    public Image image = null;
    void Start()
    {
        dynamicAtlas = new DynamicAtlas(1024, "Icon");
        dynamicAtlas.Write(texture1);
        dynamicAtlas.Write(texture2);
        dynamicAtlas.Write(texture3);
        dynamicAtlas.Write(texture4);

        image.sprite = dynamicAtlas.GetSprite(texture2.name);
        image.SetNativeSize();

        dynamicAtlas.Remove(texture4.name);
        dynamicAtlas.Apply();

        texture = dynamicAtlas.Texture;

        DynamicAtlas.Save(dynamicAtlas, new DynamicAtlas.FileInfo("test", "D:/"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
