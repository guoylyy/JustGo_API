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
            smsComposeTask.Body = "I'm using Insist to achieve my goals. Come and join me to insist on daily habits. We could look after each other! Get the app here: http://www.windowsphone.com/s?appid=46f060c1-9afc-4082-9a0c-d7f41dabf68c"
                + Environment.NewLine + "Thanks";

            smsComposeTask.Show();
        }

        private void SendViaMailGrid_OnTap(object sender, GestureEventArgs e)
        {
            var emailTask = new EmailComposeTask();
            emailTask.Subject = "Suggest App";
            emailTask.Body =
                "I'm using Insist to achieve my goals. Come and join me to insist on daily habits. We could look after each other! Get the app here: http://www.windowsphone.com/s?appid=46f060c1-9afc-4082-9a0c-d7f41dabf68c";

            emailTask.Show();
        }
    }
}