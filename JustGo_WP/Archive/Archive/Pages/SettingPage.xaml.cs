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
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class SettingPage : PhoneApplicationPage
    {
        public SettingPage()
        {
            InitializeComponent();
            Loaded += SettingPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ToggleSwitchButton.IsChecked = StaticMethods.ReadNotificationSetting();
        }

        private void SettingPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (StaticMethods.IsUserLogin())
            {
                LogoutButton.Visibility = Visibility.Visible;
            }
        }

        private void NotificationsGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/NotificationSettingPage.xaml", UriKind.Relative));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var fb = FacebookAgent.Current;
            fb.Logout();
            StaticMethods.DeleteUserId();

            Global.LoginUser = null;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void FeedbackGrid_OnTap(object sender, GestureEventArgs e)
        {
            var emailTask = new EmailComposeTask();
            emailTask.Subject = "Insist Feedback";
            emailTask.To = "18801790649@163.com";

            emailTask.Show();
        }

        private void ToggleSwitchButton_OnClick(object sender, RoutedEventArgs e)
        {
            StaticMethods.ChangeNotificationSetting(ToggleSwitchButton.IsChecked.HasValue 
                && ToggleSwitchButton.IsChecked.Value);
        }
    }
}