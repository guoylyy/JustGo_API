using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Archive.Datas;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Color = System.Windows.Media.Color;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class UserProfilePage : PhoneApplicationPage
    {
        private User _user;
        private ApplicationBar _followApplicationBar;
        private ApplicationBar _unFollowApplicationBar;

        public UserProfilePage()
        {
            InitializeComponent();
            InitAppbar();
            _user = new User
            {
                UserId = Global.SelectedUser.UserId,
                UserName = Global.SelectedUser.UserName
            };

            DataContext = _user;
        }

        private void InitAppbar()
        {
            _followApplicationBar = new ApplicationBar
            {
                BackgroundColor = (Color)Application.Current.Resources["AppbarBackgroundColor"],
                ForegroundColor = (Color)Application.Current.Resources["AppbarForegroundColor"],
                Opacity = 0.99
            };
            var followButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("Assets/AppBar/favorite.png", UriKind.Relative),
                Text = "Follow"
            };
            followButton.Click += async (sender, e) =>
            {
                if (await ServerApi.PostFollowAsync(Global.LoginUser.Token, _user.UserId) == "success")
                {
                    MessageBox.Show("Follow success");
                    ApplicationBar = _unFollowApplicationBar;
                    Global.LoginUser.FollowingCount++;
                    StaticMethods.WriteUser(Global.LoginUser);
                }

            };
            _followApplicationBar.Buttons.Add(followButton);


            _unFollowApplicationBar = new ApplicationBar
            {
                BackgroundColor = (Color)Application.Current.Resources["AppbarBackgroundColor"],
                ForegroundColor = (Color)Application.Current.Resources["AppbarForegroundColor"],
                Opacity = 0.99
            };
            var unFollowButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/like.png", UriKind.Relative),
                Text = "Unfollow"
            };
            unFollowButton.Click += async (sender1, e1) =>
            {
                if (await ServerApi.PostUnFollowAsync(Global.LoginUser.Token, _user.UserId) == "success")
                {
                    MessageBox.Show("Unfollow success");
                    ApplicationBar = _followApplicationBar;
                    Global.LoginUser.FollowingCount--;
                    StaticMethods.WriteUser(Global.LoginUser);
                }
            };
            _unFollowApplicationBar.Buttons.Add(unFollowButton);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await LoadUser();
            //_user.NotifyAllPropertyChanged();
            ChangeAppbar();
        }

        private void ChangeAppbar()
        {
            ApplicationBar = _user.CanFollow ? _followApplicationBar : _unFollowApplicationBar;
        }

        private async Task LoadUser()
        {
            await ServerApi.GetOtherUserAsync(Global.LoginUser.Token, _user);
        }

        private async void EncourageButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = await ServerApi.PostEncourgeAsync(Global.LoginUser.Token, _user.UserId);
            if (string.IsNullOrEmpty(result))
            {
                MessageBox.Show("You have encourged him/her today");
            }
            else
            {
                MessageBox.Show("Encourged success");
            }
            Debug.WriteLine("Encourage result:{0}", result);
        }

        private void FightingCenterGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FightingCenterPage.xaml?type=other", UriKind.Relative));
        }

        private void FollowersGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/OtherFollowPage.xaml?type=follower", UriKind.Relative));
        }

        private void FollowingsGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/OtherFollowPage.xaml?type=following", UriKind.Relative));
        }
    }
}