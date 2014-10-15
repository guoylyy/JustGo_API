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
                if (!StaticMethods.IsUserLogin())
                {
                    NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
                    return;
                }
                if (await ServerApi.PostFollowAsync(Global.LoginUser.Token, _user.UserId) == "success")
                {
                    StaticMethods.ShowToast("Follow success");
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
                    StaticMethods.ShowToast("Unfollow success");
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
            if (await LoadUser())
            {
                ChangeAppbar();
            }
            else
            {
                StaticMethods.ShowRequestFailedToast();
                EncourageButton.IsEnabled = false;
                FollowersGrid.IsHitTestVisible = false;
                FollowingsGrid.IsHitTestVisible = false;
                FightingCenterGrid.IsHitTestVisible = false;
            }
            //_user.NotifyAllPropertyChanged();
            
        }

        private void ChangeAppbar()
        {
            ApplicationBar = _user.CanFollow ? _followApplicationBar : _unFollowApplicationBar;
        }

        private async Task<bool> LoadUser()
        {
            return await ServerApi.GetOtherUserAsync(_user);
        }

        private async void EncourageButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!StaticMethods.IsUserLogin())
            {
                NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
                return;
            }

            var result = await ServerApi.PostEncourgeAsync(Global.LoginUser.Token, _user.UserId);
            if (string.IsNullOrEmpty(result))
            {
                StaticMethods.ShowToast("You have encourged him/her today");
            }
            else
            {
                StaticMethods.ShowToast("Encourged success");
            }
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

        private void AchievementsGrid_OnTap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("Coming soon.");
        }
    }
}