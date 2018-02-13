using ApiTest.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ApiTest
{
    class Program
    {
        private static string _logUrl = "http://localhost:50321/api/logs";
        static void Main(string[] args)
        {
            try
            {
                //WebLog("测试");
                //GetWebLog(0,1);
                //var remark = GetLongRemark();
                var remark = "备注信息";
                var product = new Product()
                {
                    Name = "名称",
                    Desc = remark,
                };

                remark = JsonConvert.SerializeObject(product);

                WebLog(userCode: "000001", userName: "张三", operation: "普通操作", remark: remark);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        static string GetLongRemark()
        {
            var str = string.Empty;

            for (int i = 0; i < 2100; i++)
            {
                str += "商品描述";
            }

            return str;
        }

        static void WebLog(string userCode= "000001", string userName= "Admin", string loginIp= "127.0.0.1"
            , int type=1,string operation= "普通操作", string remark="")
        {
            var dic = new Dictionary<string, string>();
            dic.Add("UserCode", userCode);
            dic.Add("UserName", userName);
            dic.Add("LoginIP", loginIp);
            dic.Add("Type", type.ToString());
            dic.Add("Operation", operation);
            dic.Add("Remark", remark);
            dic.Add("DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            var queryStr = DicToQueryStr(dic);

            var response = CreatePostHttpResponse(_logUrl, queryStr, Encoding.UTF8);
            if (response != null)
            {
                var stream = response.GetResponseStream(); //获取响应的字符串流
                if (stream != null)
                {
                    var sr = new StreamReader(stream); //创建一个stream读取流  
                    var str = sr.ReadToEnd();
                    Console.WriteLine(str);
                }
            }
        }

        static void WebLog(string msg)
        {
            var response = CreatePostHttpResponse(_logUrl, "Remark="+msg, Encoding.UTF8);
            if (response != null)
            {
                var stream = response.GetResponseStream(); //获取响应的字符串流
                if (stream != null)
                {
                    var sr = new StreamReader(stream); //创建一个stream读取流  
                    var str = sr.ReadToEnd();
                    Console.WriteLine(str);
                }
            }
        }

        static string GetWebLog(int pageIndex = 0, int pageSize = 10)
        {
            var result = HttpGet(_logUrl + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize, Encoding.UTF8);

            Console.WriteLine(result);

            return result;
        }

        private static HttpWebResponse CreatePostHttpResponse(string url, string datas, Encoding charset)
        {
            HttpWebRequest request = null;
            
            request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
            {
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                request.Method = "POST";
                byte[] data = charset.GetBytes(datas);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                return request.GetResponse() as HttpWebResponse;
            }
            else
            {
                return null;
            }
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }

        private static string HttpGet(string url, Encoding encoding = null)
        {
            var wc = new WebClient();
            wc.Encoding = encoding ?? Encoding.UTF8;
            return wc.DownloadString(url);
        }

        private static string DicToQueryStr(Dictionary<string, string> dictionary)
        {
            var list = new List<string>();
            foreach (var dic in dictionary)
            {
                list.Add(dic.Key + "=" + dic.Value);
            }

            return string.Join("&", list);
        }
    }
}
