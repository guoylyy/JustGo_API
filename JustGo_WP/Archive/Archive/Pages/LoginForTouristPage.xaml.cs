using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class LoginForTouristPage : PhoneApplicationPage
    {
        public LoginForTouristPage()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var facebookAgent = new FacebookAgent();
            if (await facebookAgent.AuthorizeAsync())
            {
                var resultStr = await facebookAgent.GetUserInfoAsync();
                var strs = resultStr.Split(',');
                var imageUrl = strs[0];
                var userName = strs[1];
                Global.LoginUser.FacebookId = strs[2];

                var token = await ServerApi.LoginAsync(userName, "Just do it!", Global.LoginUser.FacebookId, imageUrl);
                Debug.WriteLine("登陆后的token:{0}", token);
                Global.LoginUser.Token = token;

                await ServerApi.GetUserProfileAsync(Global.LoginUser);
                //Global.LoginUser = user;
                StaticMethods.WriteUser(Global.LoginUser);
            }
        }
    }
}