using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;

namespace Archive.Pages
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                while (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
        }
        
        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            //var inputUserName = UserNameBox.Text;
            //var inputPassword = PasswordBlock.Password;
            //var postString = string.Format("username={0}&password={1}", inputUserName, inputPassword);

            //string url = ServerApi.Login;
            //HttpWebRequest request = WebRequest.CreateHttp(new Uri(url));
            //request.Method = "POST";
            //request.ContentType = ServerApi.FormContentType;

            //using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null))
            //{
            //    //将用户名和密码写入post请求中
            //    byte[] postBytes = Encoding.UTF8.GetBytes(postString);
            //    await stream.WriteAsync(postBytes, 0, postBytes.Length);
            //}

            //request.BeginGetResponse(ResphonseCallBack, request);
        }

        private void ResphonseCallBack(IAsyncResult ar)
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

                    //todo:解析json
                    if ((string)jObject["code"] == ServerApi.CorrectCode)
                    {
                        //保存Token并写入文件
                        JObject resultJson = (JObject)jObject["result"];
                        Global.Token = (string)resultJson["token"];
                        StaticMethods.WriteToken(Global.Token);
                        //导航到主界面
                        Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)));
                        //while (NavigationService.CanGoBack)
                        //{
                        //    NavigationService.RemoveBackEntry();
                        //}
                    }
                    else if((string)jObject["code"] == ServerApi.LoginFailCode)
                    {
                        MessageBox.Show("用户名或密码错误");
                    }
                    

                    //Global.Token = response.Substring(11, response.Length - 13);
                    //StaticMethods.WriteToken(Global.Token);
                    //Debug.WriteLine(Global.Token);
                    //ShowText.Text = response;
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

        private void RegisteButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/RegistePage.xaml", UriKind.Relative));
        }
    }
}