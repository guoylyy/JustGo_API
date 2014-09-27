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
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class SettingPage : PhoneApplicationPage
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string postString = string.Format("token={0}", Global.Token);
            //string url = ServerApi.Logout;

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
            //App.RootFrame.UriMapper = null;
            //NavigationService.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.Relative));
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

                    Debug.WriteLine(response);
                    JObject jObject = JObject.Parse(response);
                    if ((string)jObject["result"] == "success")
                    {
                        StaticMethods.DeleteToken();
                        //Dispatcher.BeginInvoke(
                        //    () => NavigationService.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.Relative)));

                    }

                    sr.Close();
                }
            }
            catch (WebException we)
            {
                Debug.WriteLine(we.Message);
            }
        }

        private void NotificationsGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/NotificationSettingPage.xaml", UriKind.Relative));
        }
    }
}