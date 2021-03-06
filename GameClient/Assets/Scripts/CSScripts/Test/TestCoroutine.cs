using DotEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAsync;
using System.Threading.Tasks;
using DotEngine.AAS;
using System.IO;

public class TestCoroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //IEnumerator ie = Print();
        //CoroutineRunner.Start("Test", ie);

        ToGetBundle();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void ToGetBundle()
    {
        var bundle = await LoadBundle();
        if(bundle!=null)
        {
            string[] names = bundle.GetAllAssetNames();
            Debug.Log(string.Join(",", names));
        }
    }

    async Task<AssetBundle> LoadBundle()
    {
        string path = BundleConst.BundlePath + "/StandaloneWindows/9ca54edbdd133ee78b9af1db4c31c55e";
        if(File.Exists(path))
        {
            Debug.Log("SSSSSSSSSSSS");
        }
        var wbl = new WaitBundleLoad(path);
        await wbl;

        return wbl.GetBundle();
    }

    public struct WaitBundleLoad : IAwaitInstruction
    {
        private AssetBundleCreateRequest createRequest;
        public bool IsCompleted()
        {
            return createRequest.isDone;
        }

        public AssetBundle GetBundle()
        {
            return createRequest.assetBundle;
        }

        public WaitBundleLoad(string path)
        {
            createRequest = AssetBundle.LoadFromFileAsync(path);
        }
    }

    //private IEnumerator Print()
    //{
    //    Debug.Log("DDDDDDDDDDDDDD1" + ",Frame = " + Time.frameCount);
    //    yield return null;

    //    Debug.Log("SDDDDDDDDDDDDD2" + ",Frame = " + Time.frameCount);
    //    yield return new WaitForSeconds(0.2f);

    //    Debug.Log("SDDDDDDDDDDDDD3" + ",Frame = " + Time.frameCount);
    //}
}
