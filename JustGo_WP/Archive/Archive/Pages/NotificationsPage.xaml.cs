using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class NotificationsPage : PhoneApplicationPage
    {
        public NotificationsPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.NotificationViewModel;
            Loaded += NotificationsPage_Loaded;
        }

        private void NotificationsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.NotificationViewModel.LoadData();
        }
    }
}