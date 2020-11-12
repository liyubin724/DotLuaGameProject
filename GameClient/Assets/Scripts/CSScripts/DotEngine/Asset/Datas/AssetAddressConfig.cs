using DotEngine.Log;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.Asset.Datas
{
    /// <summary>
    /// 资源地址配置
    /// </summary>
    public class AssetAddressConfig : ScriptableObject,ISerializationCallbackReceiver
    {
        public AssetAddressData[] addressDatas = new AssetAddressData[0];

        private Dictionary<string, AssetAddressData> addressToDataDic = new Dictionary<string, AssetAddressData>();
        private Dictionary<string, AssetAddressData> pathToDataDic = new Dictionary<string, AssetAddressData>();
        private Dictionary<string, List<string>> labelToAddressDic = new Dictionary<string, List<string>>();

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            foreach (var data in addressDatas)
            {
                if (addressToDataDic.ContainsKey(data.assetAddress))
                {
                    LogUtil.Error(typeof(AssetAddressConfig).Name, $"The address is repeated. address = {data.assetAddress}");
                    continue;
                }
                else
                {
                    addressToDataDic.Add(data.assetAddress, data);
                }

                if (pathToDataDic.ContainsKey(data.assetPath))
                {
                    LogUtil.Error(typeof(AssetAddressConfig).Name, $"The path is repeated. path = {data.assetPath}");
                    continue;
                }
                else
                {
                    pathToDataDic.Add(data.assetPath, data);
                }

                if (data.labels != null && data.labels.Length > 0)
                {
                    foreach (var label in data.labels)
                    {
                        if (!labelToAddressDic.TryGetValue(label, out List<string> addressList))
                        {
                            addressList = new List<string>();
                            labelToAddressDic.Add(label, addressList);
                        }

                        addressList.Add(data.assetAddress);
                    }
                }
            }
        }

        /// <summary>
        /// 判断资源是否是场景资源
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckIsSceneByPath(string path)
        {
            if (pathToDataDic.TryGetValue(path, out AssetAddressData data))
            {
                return data.isScene;
            }
            LogUtil.Error(typeof(AssetAddressConfig).Name, $"data is not found.path={path}!");
            return false;
        }

        /// <summary>
        /// 查找资源地址对应的路径
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public string GetPathByAddress(string address)
        {
            if(addressToDataDic.TryGetValue(address,out AssetAddressData data))
            {
                return data.assetPath;
            }

            LogUtil.Error(typeof(AssetAddressConfig).Name, $"Path is not found.address={address}!");
            return null;
        }

        /// <summary>
        /// 查找资源地址对应的路径
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns></returns>
        public string[] GetPathsByAddresses(string[] addresses)
        {
            string[] result = new string[addresses.Length];
            for(int i =0;i<addresses.Length;++i)
            {
                string assetPath = GetPathByAddress(addresses[i]);
                if(string.IsNullOrEmpty(assetPath))
                {
                    return null;
                }

                result[i] = assetPath;
            }
            return result;
        }

        /// <summary>
        /// 查找所有标记为指定标签的资源
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public string[] GetAddressesByLabel(string label)
        {
            if (labelToAddressDic.TryGetValue(label, out List<string> addressList))
            {
                return addressList.ToArray();
            }
            LogUtil.Error(typeof(AssetAddressConfig).Name, $"address is not found.label={label}!");
            return null;
        }

        /// <summary>
        /// 根据资源的路径查找所在的AB
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetBundleByPath(string path)
        {
            if (pathToDataDic.TryGetValue(path,out AssetAddressData data))
            {
                return data.bundlePath;
            }
            LogUtil.Error(typeof(AssetAddressConfig).Name, $"bundle is not found.path={path}!");
            return null;
        }

        public void Reload()
        {
            addressToDataDic.Clear();
            pathToDataDic.Clear();
            labelToAddressDic.Clear();

            OnAfterDeserialize();
        }

        public void Clear()
        {
            addressDatas = new AssetAddressData[0];
            addressToDataDic.Clear();
            pathToDataDic.Clear();
            labelToAddressDic.Clear();
        }

        /// <summary>
        /// 资源地址的详细信息
        /// </summary>
        [Serializable]
        public class AssetAddressData
        {
            /// <summary>
            /// 资源地址
            /// </summary>
            public string assetAddress;
            /// <summary>
            /// 资源路径
            /// </summary>
            public string assetPath;
            /// <summary>
            /// 资源所属AB
            /// </summary>
            public string bundlePath;
            /// <summary>
            /// 资源标签
            /// </summary>
            public string[] labels = new string[0];

            /// <summary>
            /// 是否是场景类资源
            /// </summary>
            public bool isScene = false;
            /// <summary>
            /// 是否需要提前预加载
            /// </summary>
            public bool isPreload = false;
            /// <summary>
            /// 资源一旦加载后是否常驻内存
            /// </summary>
            public bool isNeverDestroy = false;
        }
    }
}
