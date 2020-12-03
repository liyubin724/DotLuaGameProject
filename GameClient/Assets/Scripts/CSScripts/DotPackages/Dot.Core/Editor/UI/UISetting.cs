using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace DotEditor.UI
{
    public class UISetting
    {
        static public SpriteAtlas atlas
        {
            get { return Get<SpriteAtlas>("TP Atlas", null); }
            set { Set("TP Atlas", value); }
        }

        static public string selectedSprite
        {
            get { return GetString("UI Sprite", null); }
            set { SetString("UI Sprite", value); }
        }

        static public string partialSprite
        {
            get { return GetString("UI Partial", null); }
            set { SetString("UI Partial", value); }
        }

        static public string GetString(string name, string defaultValue) { return EditorPrefs.GetString(name, defaultValue); }
        static public void SetString(string name, string val) { EditorPrefs.SetString(name, val); }

        static public T Get<T>(string name, T defaultValue) where T : Object
        {
            string path = EditorPrefs.GetString(name);
            if (string.IsNullOrEmpty(path)) return null;

            T retVal = LoadAsset<T>(path);

            if (retVal == null)
            {
                int id;
                if (int.TryParse(path, out id))
                    return EditorUtility.InstanceIDToObject(id) as T;
            }
            return retVal;
        }
        static public void Set(string name, Object obj)
        {
            if (obj == null)
            {
                EditorPrefs.DeleteKey(name);
            }
            else
            {
                if (obj != null)
                {
                    string path = AssetDatabase.GetAssetPath(obj);

                    if (!string.IsNullOrEmpty(path))
                    {
                        EditorPrefs.SetString(name, path);
                    }
                    else
                    {
                        EditorPrefs.SetString(name, obj.GetInstanceID().ToString());
                    }
                }
                else EditorPrefs.DeleteKey(name);
            }
        }
        static public T LoadAsset<T>(string path) where T : Object
        {
            Object obj = LoadAsset(path);
            if (obj == null) return null;

            T val = obj as T;
            if (val != null) return val;

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    GameObject go = obj as GameObject;
                    return go.GetComponent(typeof(T)) as T;
                }
            }
            return null;
        }
        static public Object LoadAsset(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return AssetDatabase.LoadMainAssetAtPath(path);
        }
    }
}
