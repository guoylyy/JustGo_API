using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;

namespace Archive.Pages
{
    public partial class RegistePage : PhoneApplicationPage
    {
        private static ManualResetEvent _postReset = new ManualResetEvent(false);

        public RegistePage()
        {
            InitializeComponent();
        }

        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            //todo:注册
            var inputUserName = UserNameBox.Text;
            var inputPassword = PasswordBox.Password;
            var inputEmail = EmailBox.Text;
            var postString = string.Format("username={0}&password={1}&email={2}", inputUserName, inputPassword,
                inputEmail);

            string url = ServerApi.Register;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(url));
            request.Method = "POST";
            request.ContentType = ServerApi.FormContentType;
            //request.Method = "GET";

            using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream,request.EndGetRequestStream,null))
            {
                //将用户名和密码写入post请求中
                byte[] postBytes = Encoding.UTF8.GetBytes(postString);
                await stream.WriteAsync(postBytes, 0, postBytes.Length);
            }

            request.BeginGetResponse(ResponseCallBack, request);
        }

        private void ResponseCallBack(IAsyncResult ar)
        {
            string response;
            try
            {
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;

                WebResponse webResponse = request.EndGetResponse(ar);
                using (Stream stream = webResponse.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    response = sr.ReadToEnd();

                    Dispatcher.BeginInvoke(() =>
                    {
                        ShowText.Text = response;
                    });

                    //把Token写入本地文档中
                    JObject jObject = JObject.Parse(response);
                    if ((string)jObject["result"] == "success")
                    {
                        //保存Token并写入文件
                        Global.Token = (string)jObject["token"];
                        StaticMethods.WriteTokenAsync(Global.Token);
                        //导航到主界面
                        Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)));
                    }
                    //else if ((string)jObject["result"] == "no user")
                    //{
                    //    MessageBox.Show("用户名不存在");
                    //}
                    //else
                    //{
                    //    MessageBox.Show("密码错误");
                    //}

                    sr.Close();
                }
            }
            catch (WebException we)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ShowText.Text = we.Message;
                });
            }



        }
    }
}