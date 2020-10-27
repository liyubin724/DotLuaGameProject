using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

namespace KSTCEngine.GPerf.Recorder
{
    public static class HttpClientUtil
    {
        public static async Task<string> PostAsync(string url, byte[] datas, string contentType, int timeOut = 8, Dictionary<string, string> headers = null)
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
                        Debug.Log("HttpClientUtil::" + e.Message);
                        return null;
                    }
                }
            }
        }
    }
}
