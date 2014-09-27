using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Facebook;
using Facebook.Client;
using Microsoft.Phone.Controls;
using Newtonsoft.Json;

namespace Archive
{
    public class FacebookAgent
    {
        #region Fields

        /// <summary>
        ///  程序的AppId
        /// </summary>
        public string AppId { get; private set; }

        // 用户授权后获取到的token
        private FacebookSession _accessToken;
        // 申请的权限
        private const string ExtendedPermissions = "user_about_me,read_stream,publish_stream,user_photos";
        //用于登陆登出的client
        private FacebookSessionClient _facebookSessionClient;

        private const string SettingsKeyForFacebookToken = "WP8_SNS_FacebookToken"; // facebook的存储关键字

        private DateTime _sendTime;

        #endregion

        public FacebookAgent()
        {
            AppId = "344702782359268";
            _facebookSessionClient = new FacebookSessionClient(AppId);
        }

        /// <summary>
        /// 判断当前是否已经登陆facebook
        /// </summary>
        /// <returns>是否已经登陆</returns>
        public bool IsFacebookLogin()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingsKeyForFacebookToken))
            {
                _accessToken =
                    IsolatedStorageSettings.ApplicationSettings[SettingsKeyForFacebookToken] as FacebookSession;
                if (_accessToken != null && !string.IsNullOrEmpty(_accessToken.AccessToken) &&
                    _accessToken.Expires > DateTime.Now.AddSeconds(30))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 申请授权
        /// </summary>
        /// <returns>授权是否成功</returns>
        public async Task<bool> AuthorizeAsync()
        {
            // 首先看记录下来的Token信息是否有效
            if (IsFacebookLogin())
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(AppId)) return false;

            _accessToken = await _facebookSessionClient.LoginAsync(ExtendedPermissions);
            // 将token信息记录到系统的Settings信息中
            if (_accessToken != null)
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(SettingsKeyForFacebookToken))
                {
                    IsolatedStorageSettings.ApplicationSettings[SettingsKeyForFacebookToken] = _accessToken;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add(SettingsKeyForFacebookToken, _accessToken);
                }
                IsolatedStorageSettings.ApplicationSettings.Save();

                return true;
            }
            return false;
        }

        /// <summary>
        /// 登出facebook
        /// </summary>
        public void Logout()
        {
            //本地清除token，并调用登出方法
            _accessToken = null;
            _facebookSessionClient.Logout();
            IsolatedStorageSettings.ApplicationSettings.Remove(SettingsKeyForFacebookToken);

            //在前台添加webBrowser，用于删除服务端的cookie
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var popup = new Popup();
                var webBrowser = new WebBrowser();
                popup.Child = webBrowser;
                popup.IsOpen = true;

                LoadCompletedEventHandler loadCompleted = null;
                loadCompleted = (sender, e) =>
                {
                    if (
                        webBrowser.SaveToString().Contains("logout_form"))
                    {
                        webBrowser.InvokeScript("eval", "document.forms['logout_form'].submit();");
                        webBrowser.Visibility = Visibility.Collapsed;
                        webBrowser.LoadCompleted -= loadCompleted;
                        popup.IsOpen = false;
                        popup = null;
                    }
                };

                webBrowser.LoadCompleted += loadCompleted;
                //跳转到登出所需网址
                webBrowser.Navigate(new Uri("https://www.facebook.com/logout.php"));
            });
        }

        /// <summary>
        /// 分享文字
        /// </summary>
        /// <param name="text">需要分享的内容</param>
        /// <returns>是否分享成功</returns>
        public async Task<bool> ShareTextAsync(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AppId)) throw new ArgumentException("Appid is null");
                if (string.IsNullOrWhiteSpace(_accessToken.AccessToken))
                    throw new ArgumentException("Failed autherized");
                if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException("text");

                var parameters = new Dictionary<string, object>();
                parameters["message"] = text;
                var postTask = new TaskCompletionSource<bool>();

                var fb = new Facebook.FacebookClient(_accessToken.AccessToken);
                fb.PostCompleted += (sender, args) => postTask.SetResult(args.Error == null);

                await fb.PostTaskAsync("me/feed", parameters);
                return await postTask.Task;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 分享图片到Facebook 
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="fileStream">图片流</param>
        /// <returns></returns>
        public async Task<bool> ShareImgAsync(string text, FileStream fileStream)
        {
            try
            {
                //创建FacebookMediaStream
                using (var tFile = new Facebook.FacebookMediaStream
                {
                    ContentType = "image/jpeg",
                    FileName = DateTime.Now.ToString("yyyyMMMMddhhmmss")
                }.SetValue(fileStream))
                {
                    var fb = new Facebook.FacebookClient(_accessToken.AccessToken);
                    var postTask = new TaskCompletionSource<bool>();
                    fb.PostCompleted += (sender, args) => { postTask.SetResult(args.Error == null); };
                    //发布图像至我的照片：/me/photos/
                    //dynamic fbPostTaskResult = await fb.PostTaskAsync("me/photos", new { message = text, tFile });
                    _sendTime = DateTime.Now;
                    await fb.PostTaskAsync("me/photos", new { message = text, tFile });
                    //return fbPostTaskResult.Status == HttpStatusCode.OK;
                    return await postTask.Task;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断当前分享是否成功
        /// </summary>
        /// <returns>是否成功</returns>
        public async Task<bool> IsShareSuccess()
        {
            var fb = new FacebookClient(_accessToken.AccessToken);
            var shareTask = new TaskCompletionSource<bool>();

            fb.GetCompleted += (o, e2) =>
            {
                if (e2.Error != null)
                {
                    Debug.WriteLine(e2.Error.Message);
                    shareTask.SetResult(false);
                    return;
                }
                //读取用户发布的记录
                var result = (IDictionary<string, object>)e2.GetResultData();
                var list = new List<object>(result.Values);
                var valueList = list[0] as List<object>;
                var str = string.Empty;
                //得到扫描全能王的相册
                foreach (var value in valueList)
                {
                    if (value.ToString().Contains("CamScanner"))
                    {
                        str = value.ToString();
                        break;
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    shareTask.SetResult(false);
                    return;
                }

                //获取更新时间
                dynamic d = JsonConvert.DeserializeObject(str);
                string time = d.updated_time;
                var updateTime = DateTime.Parse(time);

                shareTask.SetResult(updateTime - _sendTime < TimeSpan.FromMinutes(2));
            };

            await Task.Delay(1000);
            await fb.GetTaskAsync("me/albums");

            return await shareTask.Task;
        }

        public async Task<string> GetUserInfoAsync()
        {
            var fb = new FacebookClient(_accessToken.AccessToken);
            var userTask = new TaskCompletionSource<string>();

            fb.GetCompleted += async (o, e) =>
            {
                if (e.Error != null)
                {
                    Debug.WriteLine(e.Error.Message);
                    userTask.SetResult(string.Empty);
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();


                var profilePictureUrl = await GetUserPictureAsync();

                //this.MyImage.Source = new BitmapImage(new Uri(profilePictureUrl));
                var userName = String.Format("{0} {1}", result["first_name"], result["last_name"]);
                var id = result["id"];
                var str = profilePictureUrl + "," + userName + "," + id;
                userTask.SetResult(str);
            };

            await fb.GetTaskAsync("me");
            return await userTask.Task;
        }

        private async Task<string> GetUserPictureAsync()
        {
            var fb = new FacebookClient(_accessToken.AccessToken);
            var userTask = new TaskCompletionSource<string>();

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Debug.WriteLine(e.Error.Message);
                    userTask.SetResult(string.Empty);
                    return;
                }

                var result = (IDictionary<string, object>)e.GetResultData();
                var data = (IDictionary<string, object>)result["data"];
                var pictureUrl = data["url"];
                userTask.SetResult(pictureUrl.ToString());
            };
            await fb.GetTaskAsync("me/picture?redirect=false&width=480");

            return await userTask.Task;
        }
    }
}