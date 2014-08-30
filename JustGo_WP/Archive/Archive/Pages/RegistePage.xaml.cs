using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Security;
using System.Text;
using System.Text.RegularExpressions;
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
        private bool _isPasswordOK;
        private Regex _emailRegex = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        private bool _isEmailOK;
        private ApplicationBarIconButton _registerButton;

        public RegistePage()
        {
            InitializeComponent();
            Loaded += RegistePage_Loaded;
        }

        void RegistePage_Loaded(object sender, RoutedEventArgs e)
        {
            _registerButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
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
                    if ((string)jObject["code"] == ServerApi.CorrectCode)
                    {
                        //保存Token并写入文件
                        JObject resultJson = (JObject)jObject["result"];
                        Global.Token = (string)resultJson["token"];
                        StaticMethods.WriteTokenAsync(Global.Token);
                        //导航到主界面
                        Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)));
                    }
                    else if ((string)jObject["code"] == ServerApi.EmailExistCode)
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            MessageBox.Show("邮箱已存在");
                            EmailBox.Focus();
                        });
                        //MessageBox.Show("邮箱已存在");
                    }
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

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Count() < 6)
            {
                ShowText.Text = "密码长度至少6位";
                _isPasswordOK = false;
            }
            else
            {
                _isPasswordOK = true;
            }
            CheckRegister();
        }

        private void EmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Match emailMatch = _emailRegex.Match(EmailBox.Text);
            if (emailMatch.Success)
            {
                _isEmailOK = true;
            }
            else
            {
                ShowText.Text = "请输入正确的邮箱";
                _isEmailOK = false;
            }
            CheckRegister();
        }

        private void CheckRegister()
        {
            if (_isEmailOK && _isPasswordOK)
            {
                
                _registerButton.IsEnabled = true;
            }
            else
            {
                _registerButton.IsEnabled = false;
            }
        }
    }
}