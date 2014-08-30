using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;

namespace Archive
{
    public class ServerApi
    {
        #region URL变量
        private const string SerVerUrl = "http://115.28.4.78:8888";
        public const string Register = SerVerUrl + "/user/register";
        public const string Login = SerVerUrl + "/user/login";
        public const string Logout = SerVerUrl + "/user/logout";
        public const string DataPull = SerVerUrl + "/user/data_pull";
        public const string FormContentType = "application/x-www-form-urlencoded";

        #endregion


        #region 请求返回码

        public const string CorrectCode = "200";
        public const string BadRequestCode = "400";
        public const string WrongAuthoriedCode = "401";
        public const string ForbiddenCode = "403";
        public const string NotFoundCode = "404";
        public const string PrametersMissingCode = "414";
        public const string InternalErrorCode = "500";

        public const string LoginFailCode = "001";

        public const string EmailExistCode = "011";
        public const string PassWordSimpleCode = "012";

        #endregion

        //private static ManualResetEvent _waitSignal = new ManualResetEvent(false);
        //public static string JsonResponse;
        ///// <summary>
        ///// 用于用户登录的方法，需传入用户名和密码
        ///// </summary>
        ///// <param name="userName">用户名</param>
        ///// <param name="password">密码</param>
        ///// <returns>服务器返回的JSON字符串，需进行JSON解析</returns>
        //public static async Task LoginAsync(string userName, string password)
        //{
        //    var postString = string.Format("username={0}&password={1}", userName, password);

        //    string url = Login;
        //    HttpWebRequest request = WebRequest.CreateHttp(new Uri(url));
        //    request.Method = "POST";
        //    request.ContentType = FormContentType;

        //    try
        //    {
        //        using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null))
        //        {
        //            //将用户名和密码写入post请求中
        //            byte[] nameBytes = Encoding.UTF8.GetBytes(postString);
        //            await stream.WriteAsync(nameBytes, 0, nameBytes.Length);
        //        }

        //        request.BeginGetResponse(ResphonseCallBack, request);
        //        //_waitSignal.WaitOne();
        //        //WebResponse webResponse =
        //        //    await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request);
        //        //using (Stream stream = webResponse.GetResponseStream())
        //        //{
        //        //    StreamReader sr = new StreamReader(stream);
        //        //    var response = sr.ReadToEnd();
        //        //    sr.Close();

        //        //    return response;
        //        //}
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //    }

        // }

        //private static void ResphonseCallBack(IAsyncResult ar)
        //{
        //    HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
        //    WebResponse webResponse = request.EndGetResponse(ar);
        //    using (Stream stream = webResponse.GetResponseStream())
        //    {
        //        StreamReader sr = new StreamReader(stream);
        //        JsonResponse = sr.ReadToEnd();
        //        //_waitSignal.Set();
        //    }
        //}
    }
}
