using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

namespace KSTCEngine.GPerf.Recorder
{
    public static class RemoteUtil
    {
        public static async Task<string> PostSessionAsync(string url, byte[] datas, string contentType, int timeOut = 5, Dictionary<string, string> headers = null)
        {
            using(HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = new TimeSpan(0, 0, timeOut);
                if(headers!=null)
                {
                    foreach(var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                using (HttpContent httpContent = new ByteArrayContent(datas))
                {
                    httpContent.Headers.ContentLength = datas.Length;
                    if(!string.IsNullOrEmpty(contentType))
                    {
                        httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }
                    try
                    {
                        HttpResponseMessage responseMessage = await httpClient.PostAsync(url, httpContent);
                        return await responseMessage.Content.ReadAsStringAsync();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("HttpClientUtil::PostAsync->" + e.Message);
                        return null;
                    }
                }
            }
        }

        public static async Task<string> PutLogAsync(string url, byte[] datas, int timeOut = 5, Dictionary<string, string> headers = null)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = new TimeSpan(0, 0, timeOut);
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                using (HttpContent httpContent = new ByteArrayContent(datas))
                {
                    httpContent.Headers.ContentLength = datas.Length;
                    try
                    {
                        HttpResponseMessage responseMessage = await httpClient.PutAsync(url, httpContent);
                        return await responseMessage.Content.ReadAsStringAsync();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("HttpClientUtil::PostAsync->" + e.Message);
                        return null;
                    }
                }
            }
        }

        public static async Task<byte[]> GZipFileAsync(string logPath)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    using (var inputStream = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            GZip.Compress(inputStream, outStream, false);

                            return outStream.ToArray();
                        }
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError("RemoteUtil::GZipFileAsync->" + e.Message);
                    return null;
                }
            });
            return await task;
        }

    }
}
