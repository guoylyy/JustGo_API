using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
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
    }
}