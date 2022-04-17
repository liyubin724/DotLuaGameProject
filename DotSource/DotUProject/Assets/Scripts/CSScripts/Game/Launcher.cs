using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        Debug.Log("1 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        LoadUIRootByAssetBundle();
        Debug.Log("2 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
    }

    private async void LoadUIRootByAssetBundle()
    {
        Debug.Log("3 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        var assetBundle = await LoadAssetBundle();
        Debug.Log("4 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        var uiRoot = await LoadAssetFromBundle(assetBundle);
        Debug.Log("5 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        UnityEngine.Object.Instantiate<GameObject>(uiRoot);
        Debug.Log("6 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        assetBundle.Unload(false);
    }

    private async Task<AssetBundle> LoadAssetBundle()
    {
        Debug.Log("7 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        var request = AssetBundle.LoadFromFileAsync("D:/output/Assets/Resources/ui_root_prefab"); //.LoadAssetAsync<GameObject>("Assets/Resources/ui_root.prefab");
        Debug.Log("8 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        while (!request.isDone)
        {
            Debug.Log("9 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
            await Task.Yield();
            Debug.Log("10 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        }
        Debug.Log("11 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        return request.assetBundle;
    }

    private async Task<GameObject> LoadAssetFromBundle(AssetBundle assetBundle)
    {
        Debug.Log("12 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        var request = assetBundle.LoadAssetAsync<GameObject>("Assets/Resources/ui_root.prefab");
        Debug.Log("13 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        while (!request.isDone)
        {
            Debug.Log("14 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
            await Task.Yield();
            Debug.Log("15 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        }
        Debug.Log("16 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        return request.asset as GameObject;
    }

    private void LoadUIRootByResource()
    {
        Debug.Log("1 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        InstanceRoot();
        Debug.Log("2 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
    }

    private async void InstanceRoot()
    {
        Debug.Log("3 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        GameObject uiRoot = await LoadAsset();
        Debug.Log("4 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        UnityEngine.Object.Instantiate<GameObject>(uiRoot);
        Debug.Log("5 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
    }

    private async Task<GameObject> LoadAsset()
    {
        Debug.Log("6 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        var request = Resources.LoadAsync<GameObject>("ui_root");
        Debug.Log("7 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        while (!request.isDone)
        {
            Debug.Log("8 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
            await Task.Yield();
            Debug.Log("9 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        }
        Debug.Log("10 Current Thread id = " + Thread.CurrentThread.ManagedThreadId);
        return request.asset as GameObject;
    }
}
