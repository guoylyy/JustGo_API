using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class FightingCenterPage : PhoneApplicationPage
    {
        private bool _isOtherUser;

        public FightingCenterPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.FightingCenterViewModel;
            Loaded += FightingCenterPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string type;

            if (e.NavigationMode == NavigationMode.New 
                && NavigationContext.QueryString.TryGetValue("type", out type))
            {
                _isOtherUser = true;
            }
        }

        private void FightingCenterPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isOtherUser)
            {
                ViewModelLocator.FightingCenterViewModel.LoadOtherData();
                FightingImage.Source = new BitmapImage(new Uri(Global.SelectedUser.ImageSource));
            }
            else
            {
                ViewModelLocator.FightingCenterViewModel.LoadData();
                FightingImage.Source = new BitmapImage(new Uri(Global.LoginUser.ImageSource));
            }
            
        }

        private void MoreComments_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CommentsPage.xaml", UriKind.Relative));
        }

        private void FightingList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.SelectedUserRecord = FightingList.SelectedItem as UserRecord;
        }
    }
}