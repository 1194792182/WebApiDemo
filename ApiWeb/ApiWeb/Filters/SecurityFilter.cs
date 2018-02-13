using System.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace ApiWeb.Filters
{
    public class SecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var currentRequest = HttpContext.Current.Request;
            var httpMethod = currentRequest.HttpMethod;

            switch (httpMethod)
            {
                case "POST":
                    var stream = currentRequest.InputStream;
                    stream.Position = 0;
                    var streamReader = new StreamReader(stream);
                    var data = streamReader.ReadToEnd();
                    var result = new Dictionary<string, object>();
                    var isJson = true;
                    try
                    {
                        result = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                    }
                    catch
                    {
                        isJson = false;
                    }
                    if (!isJson)
                    {
                        try
                        {
                            var dataSplitArr = data.Split('&');
                            foreach (var item in dataSplitArr)
                            {
                                var itemSplitArr = item.Split('=');
                                if (itemSplitArr.Length > 1)
                                {
                                    result.Add(itemSplitArr[0],itemSplitArr[1]);
                                }
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                    var encryptDic = new Dictionary<string, object>();
                    string encryptStr = "";
                    string key = "";

                    CheckTimeStamp(result);

                    foreach (var item in result.OrderBy(q => q.Key))
                    {
                        if (item.Key=="appKey")
                        {
                            try
                            {
                                key = HttpUtility.UrlDecode(item.Value.ToString());
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            encryptDic.Add(item.Key, HttpUtility.UrlDecode(item.Value.ToString()));
                        }
                    }

                    encryptDic.Add("salt", "4454b7e8b23146c6b4f273c579aef2dd");

                    encryptStr = DicOrderByKeyToQueryStr(encryptDic);

                    var comparisonResult = Sha1(encryptStr);

                    if (string.IsNullOrEmpty(key) || key != comparisonResult)
                    {
                        var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            Content = new StringContent("无权限访问"),
                            ReasonPhrase = "无权限"
                        };
                        throw new HttpResponseException(resp);
                    }
                    break;
                case "GET":
                case "DELETE":
                    //var appKey = currentRequest["appKey"];
                    var getMethodResult = new Dictionary<string, object>();
                    var getMethodEncryptDic = new Dictionary<string, object>();
                    string getMethodEncryptStr = "";
                    string getMethodKey = "";
                    var collections = currentRequest.QueryString;
                    try
                    {
                        foreach (var item in collections)
                        {
                            getMethodResult.Add(item.ToString(), collections[item.ToString()]);
                        }
                    }
                    catch
                    {
                        
                    }

                    CheckTimeStamp(getMethodResult);

                    try
                    {
                        foreach (var item in getMethodResult.OrderBy(q => q.Key))
                        {
                            if (item.Key == "appKey")
                            {
                                try
                                {
                                    getMethodKey = item.Value.ToString();
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                getMethodEncryptDic.Add(item.Key, item.Value);
                            }
                        }
                    }
                    catch
                    {
                        
                    }

                    getMethodEncryptDic.Add("salt", "4454b7e8b23146c6b4f273c579aef2dd");

                    getMethodEncryptStr = DicOrderByKeyToQueryStr(getMethodEncryptDic);

                    var getMethodComparisonResult = Sha1(getMethodEncryptStr);

                    if (string.IsNullOrEmpty(getMethodKey) || getMethodKey != getMethodComparisonResult)
                    {
                        var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            Content = new StringContent("无权限访问"),
                            ReasonPhrase = "无权限"
                        };
                        throw new HttpResponseException(resp);
                    }
                    break;
                default:
                    break;
            }

            base.OnActionExecuting(actionContext);
        }

        private string DicOrderByKeyToQueryStr(Dictionary<string, object> dictionary)
        {
            var list = new List<string>();
            foreach (var dic in dictionary.OrderBy(q => q.Key))
            {
                list.Add(dic.Key + "=" + dic.Value);
            }

            return string.Join("&", list);
        }

        private string Sha1(string content)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = Encoding.UTF8.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        private void CheckTimeStamp(Dictionary<string, object> result)
        {
            var timestamp = result.FirstOrDefault(q => q.Key.Equals("timestamp"));
            if (string.IsNullOrEmpty(timestamp.Key))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("缺少timestamp参数"),
                    ReasonPhrase = "缺少timestamp参数"
                };
                throw new HttpResponseException(resp);
            }
            if (timestamp.Key.Equals("timestamp"))
            {
                var val = Convert.ToUInt64(timestamp.Value);
                var oldDt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var newDt = oldDt.AddSeconds(val);
                var timeSpan = DateTime.UtcNow - newDt;
                if (timeSpan.TotalSeconds > 10)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("权限已过期"),
                        ReasonPhrase = "权限已过期"
                    };
                    throw new HttpResponseException(resp);
                }
            }
        }
        
    }
}