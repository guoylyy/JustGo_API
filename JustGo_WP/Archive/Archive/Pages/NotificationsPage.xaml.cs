using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class NotificationsPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton _refreshBarButton;
        private bool _NeedNotification;
        public NotificationsPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.NotificationViewModel;
            Loaded += NotificationsPage_Loaded;
            _NeedNotification = StaticMethods.ReadNotificationSetting();
        }

        private void InitAppBar()
        {
            ApplicationBar = new ApplicationBar
            {
                BackgroundColor = (Color) Application.Current.Resources["AppbarBackgroundColor"],
                ForegroundColor = (Color) Application.Current.Resources["AppbarForegroundColor"],
                Opacity = 0.99
            };

            _refreshBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh2.png", UriKind.Relative));
            _refreshBarButton.Text = "refresh";
            _refreshBarButton.Click += RefreshBarIconButton_OnClick;

            ApplicationBar.Buttons.Add(_refreshBarButton);
        }

        private async void NotificationsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_NeedNotification)
            {
                NotificationList.Visibility = Visibility.Collapsed;
                NotificationOffTextBlock.Visibility = Visibility.Visible;
                ProgressGrid.Visibility = Visibility.Collapsed;
                return;
            }

            await ViewModelLocator.NotificationViewModel.LoadData();
            InitAppBar();
            ProgressGrid.Visibility = Visibility.Collapsed;
        }

        private async void RefreshBarIconButton_OnClick(object sender, EventArgs e)
        {
            ApplicationBar.IsVisible = false;
            ProgressGrid.Visibility = Visibility.Visible;
            await ViewModelLocator.NotificationViewModel.LoadData();
            ApplicationBar.IsVisible = true;
            ProgressGrid.Visibility = Visibility.Collapsed;
        }
    }
}