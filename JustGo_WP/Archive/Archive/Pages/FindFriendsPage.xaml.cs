using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class FindFriendsPage : PhoneApplicationPage
    {
        public FindFriendsPage()
        {
            InitializeComponent();
        }

        private void SearchGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SearchUsersPage.xaml", UriKind.Relative));
        }

        private void SendViaMessageGrid_OnTap(object sender, GestureEventArgs e)
        {
            var smsComposeTask = new SmsComposeTask();
            //todo:加入应用链接
            smsComposeTask.Body = "Try this new application. It's great!";

            smsComposeTask.Show();
        }

        private void SendViaMailGrid_OnTap(object sender, GestureEventArgs e)
        {
            var emailTask = new EmailComposeTask();
            emailTask.Subject = "Suggest App";
            //todo:加入应用链接
            emailTask.Body = "I Found a great App, you must try it!";

            emailTask.Show();
        }
    }
}