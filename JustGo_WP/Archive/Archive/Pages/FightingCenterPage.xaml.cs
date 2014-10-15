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
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class FightingCenterPage : PhoneApplicationPage
    {
        private bool _isOtherUser;
        private bool _needLoadData;

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
            _needLoadData = e.NavigationMode == NavigationMode.New;
        }

        private async void FightingCenterPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isOtherUser)
            {
                await ViewModelLocator.FightingCenterViewModel.LoadOtherData(_needLoadData);
                FightingImage.Source = new BitmapImage(new Uri(Global.SelectedUser.ImageSource));
            }
            else
            {
                await ViewModelLocator.FightingCenterViewModel.LoadData(_needLoadData);
                FightingImage.Source = new BitmapImage(new Uri(Global.LoginUser.ImageSource));
            }

            ProgressGrid.Visibility = Visibility.Collapsed;
            ContentPanel.Visibility = Visibility.Visible;
        }

        private void MoreComments_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CommentsPage.xaml", UriKind.Relative));
        }

        private void FightingList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.SelectedUserRecord = FightingList.SelectedItem as UserRecord;
        }

        private void CommentsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedItems != null && listBox.SelectedItems.Count != 0)
            {
                var comment = (Comment)listBox.SelectedItems[0];
                Global.SelectedUser = comment.User;
            }
        }

        private void CommentGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserProfilePage.xaml", UriKind.Relative));
        }
    }
}