using System.Linq;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBar _goalApplicationBar;
        private ApplicationBar _profileApplicationBar;
        private FacebookAgent _facebookAgent = FacebookAgent.Current;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
            //Rectangle progressRectangle = new Rectangle
            //{
            //    Width = 15,
            //    Height = 15,
            //    Fill = new SolidColorBrush(new Color {R = 201, G = 201, B = 201})
            //};
            //Rectangle currentRectangle = new Rectangle
            //{
            //    Width = 15,
            //    Height = 15,
            //    Fill = new SolidColorBrush(new Color { R = 13, G = 95, B = 127 })
            //};
            BuildLocalizedApplicationBar();
            if (Global.LoginUser != null) ProfilePivotItem.DataContext = Global.LoginUser;
            GoalsPivotItem.DataContext = ViewModelLocator.GoalViewModel;
            ExplorePivotItem.DataContext = ViewModelLocator.ExploreViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                while (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
            }

        }

        // 用于生成本地化 ApplicationBar 
        private void BuildLocalizedApplicationBar()
        {
            _goalApplicationBar = new ApplicationBar();
            _goalApplicationBar.BackgroundColor = (Color)Application.Current.Resources["AppbarBackgroundColor"];
            _goalApplicationBar.ForegroundColor = (Color)Application.Current.Resources["AppbarForegroundColor"];
            _goalApplicationBar.Opacity = 0.99;
            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            var addBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            addBarButton.Text = "add";
            addBarButton.Click += appBarButton_Click;

            //ApplicationBarIconButton logoutBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/back.png", UriKind.Relative));
            //logoutBarButton.Text = "logout";
            //logoutBarButton.Click += logoutBarButton_Click;

            _goalApplicationBar.Buttons.Add(addBarButton);
            //_goalApplicationBar.Buttons.Add(logoutBarButton);


            _profileApplicationBar = new ApplicationBar();
            _profileApplicationBar.BackgroundColor = (Color)Application.Current.Resources["AppbarBackgroundColor"];
            _profileApplicationBar.ForegroundColor = (Color)Application.Current.Resources["AppbarForegroundColor"];
            _profileApplicationBar.Opacity = 0.99;

            var settingButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png", UriKind.Relative));
            settingButton.Text = "settings";
            settingButton.Click += settingButton_Click;
            _profileApplicationBar.Buttons.Add(settingButton);

            ApplicationBar = _goalApplicationBar;
            //// 使用 AppResources 中的本地化字符串创建新菜单项。
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        void settingButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingPage.xaml", UriKind.Relative));
        }

        //添加新任务按钮点击响应函数
        void appBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ChooseGoalTypePage.xaml", UriKind.Relative));
        }

        //切换pivot页面时的响应函数,用于改变appbar状态
        private async void ContentPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ApplicationBar.Mode = !e.AddedItems[0].Equals(GoalsPivotItem) ? ApplicationBarMode.Minimized : ApplicationBarMode.Default;
            if (e.AddedItems[0].Equals(ExplorePivotItem))
            {
                ApplicationBar.IsVisible = false;

                Task.Run(() => ViewModelLocator.ExploreViewModel.LoadData());
            }
            else if (e.AddedItems[0].Equals(GoalsPivotItem))
            {
                ApplicationBar = _goalApplicationBar;
                ApplicationBar.IsVisible = true;
                ApplicationBar.Mode = ApplicationBarMode.Default;
            }
            else//profile页面
            {
                ApplicationBar = _profileApplicationBar;
                ApplicationBar.IsVisible = true;
                ApplicationBar.Mode = ApplicationBarMode.Minimized;

                if (Global.LoginUser != null && Global.LoginUser.Token != null)
                {
                    if (await ServerApi.GetUserProfileAsync(Global.LoginUser))
                    {
                        StaticMethods.WriteUser(Global.LoginUser);
                    }
                    else
                    {
                        StaticMethods.ShowRequestFailedToast();
                    }
                }
            }
        }

        //点击任务列表的响应函数,跳转到选中任务的具体状态页面
        private void GoalList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.SelectedGoalJoin = GoalList.SelectedItem as GoalJoin;
        }

        private void GoalList_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/MyRecordPage.xaml", UriKind.Relative));
        }

        private void headImage_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ProfilePage.xaml", UriKind.Relative));
        }

        private void NotificationsGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/NotificationsPage.xaml", UriKind.Relative));
        }

        private void FollowerGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FollowPage.xaml?type=follower", UriKind.Relative));
        }

        private void FollowingGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FollowPage.xaml?type=following", UriKind.Relative));
        }

        private void AchievementsGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/AchievementsPage.xaml", UriKind.Relative));
            //MessageBox.Show("Comming soon.");
        }

        private void FightingGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FightingCenterPage.xaml", UriKind.Relative));
        }

        private void FriendsGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FindFriendsPage.xaml", UriKind.Relative));
        }

        //使用facebook登陆
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ProgressGrid.Visibility = Visibility.Visible;
            //await _facebookAgent.AuthorizeAsync()
            if (await _facebookAgent.AuthorizeAsync())
            {
                var resultStr = await _facebookAgent.GetUserInfoAsync();
                var strs = resultStr.Split(',');
                var imageUrl = strs[0];
                var userName = UserNameTextBlock.Text = strs[1];
                Global.LoginUser.FacebookId = strs[2];

                //var imageUrl = "http://tp2.sinaimg.cn/1847191521/180/40039469890/1";
                //var userName = UserNameTextBlock.Text = "Cool Man";
                //Global.LoginUser.FacebookId = "0987654321";

                var token = await ServerApi.LoginAsync(userName, "Just do it!", Global.LoginUser.FacebookId, imageUrl);

                if (string.IsNullOrEmpty(token))
                {
                    StaticMethods.ShowRequestFailedToast();
                    ProgressGrid.Visibility = Visibility.Collapsed;
                    return;
                }

                Global.LoginUser.Token = token;

                await ServerApi.GetUserProfileAsync(Global.LoginUser);
                //Global.LoginUser = user;
                StaticMethods.WriteUser(Global.LoginUser);

                ProfileGrid.Visibility = Visibility.Visible;
                UnLoginGrid.Visibility = Visibility.Collapsed;
                //ProgressGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                StaticMethods.ShowRequestFailedToast();
            }
            ProgressGrid.Visibility = Visibility.Collapsed;
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Global.LoginUser == null || Global.LoginUser.Token == null)
            {
                ProfileGrid.Visibility = Visibility.Collapsed;
                UnLoginGrid.Visibility = Visibility.Visible;
                Global.LoginUser = Global.LoginUser ?? new User();
            }
            else
            {
                ProfileGrid.Visibility = Visibility.Visible;
                UnLoginGrid.Visibility = Visibility.Collapsed;
            }
            ProfilePivotItem.DataContext = Global.LoginUser;

            StaticMethods.UpdateTile();
        }

        private void ExploreGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/GoalDetailPage.xaml", UriKind.Relative));
        }

        private void ExploreList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var simpleGoal = ExploreList.SelectedItem as Goal;
            if (simpleGoal != null)
            {
                Global.AddingGoalJoin = new GoalJoin
                {
                    GoalId = simpleGoal.GoalId,
                    GoalName = simpleGoal.GoalName,
                    Participants = simpleGoal.Participants
                };
            }
        }

        private void TopExploreGrid_OnTap(object sender, GestureEventArgs e)
        {
            Global.AddingGoalJoin = new GoalJoin
            {
                GoalId = ViewModelLocator.ExploreViewModel.TopExplore.GoalId,
                GoalName = ViewModelLocator.ExploreViewModel.TopExplore.GoalName,
                Participants = ViewModelLocator.ExploreViewModel.TopExplore.Participants
            };
            NavigationService.Navigate(new Uri("/Pages/GoalDetailPage.xaml", UriKind.Relative));
        }

        
    }
}