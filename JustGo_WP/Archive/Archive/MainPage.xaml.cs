using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Archive.Datas;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Archive.Resources;
using Archive.ViewModel;
using Newtonsoft.Json.Linq;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApplicationBar _goalApplicationBar;
        private ApplicationBar _profileApplicationBar;

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
            GoalsPivotItem.DataContext = ViewModelLocator.GoalViewModel;
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
            ApplicationBarIconButton addBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
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

            ApplicationBarIconButton settingButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/feature.settings.png",UriKind.Relative));
            settingButton.Text = "settings";
            settingButton.Click += settingButton_Click;
            _profileApplicationBar.Buttons.Add(settingButton);
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            //ApplicationBar = new ApplicationBar();
            //ApplicationBar.Opacity = 0.99;


            ApplicationBar = _goalApplicationBar;
            //// 使用 AppResources 中的本地化字符串创建新菜单项。
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        void settingButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingPage.xaml", UriKind.Relative));
        }

        private void logoutBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/SettingPage.xaml", UriKind.Relative));
            //string postString = string.Format("token={0}", Global.Token);
            //string url = ServerApi.Logout;

            //HttpWebRequest request = WebRequest.CreateHttp(new Uri(url));
            //request.Method = "POST";
            //request.ContentType = ServerApi.FormContentType;

            //using (var stream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null))
            //{
            //    //将用户名和密码写入post请求中
            //    byte[] postBytes = Encoding.UTF8.GetBytes(postString);
            //    await stream.WriteAsync(postBytes, 0, postBytes.Length);
            //}

            //request.BeginGetResponse(ResphonseCallBack, request);
            //App.RootFrame.UriMapper = null;
            //App.RootFrame.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.Relative));
        }

        private void ResphonseCallBack(IAsyncResult ar)
        {
            string response;
            try
            {
                HttpWebRequest request = (HttpWebRequest)ar.AsyncState;

                WebResponse webResponse = request.EndGetResponse(ar);
                using (Stream stream = webResponse.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    response = sr.ReadToEnd();

                    Debug.WriteLine(response);
                    JObject jObject = JObject.Parse(response);
                    if ((string)jObject["result"] == "success")
                    {
                        StaticMethods.DeleteToken();
                        //Dispatcher.BeginInvoke(
                        //    () => NavigationService.Navigate(new Uri("/Pages/LoginPage.xaml", UriKind.Relative)));

                    }

                    sr.Close();
                }
            }
            catch (WebException we)
            {
                Debug.WriteLine(we.Message);
            }
        }

        //添加新任务按钮点击响应函数
        void appBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ChooseGoalTypePage.xaml", UriKind.Relative));
        }

        //切换pivot页面时的响应函数,用于改变appbar状态
        private void ContentPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ApplicationBar.Mode = !e.AddedItems[0].Equals(GoalsPivotItem) ? ApplicationBarMode.Minimized : ApplicationBarMode.Default;
            if (e.AddedItems[0].Equals(ExplorePivotItem))
            {
                ApplicationBar.IsVisible = false;
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
            }
        }

        //点击任务列表的响应函数,跳转到选中任务的具体状态页面
        private void GoalList_OnTap(object sender, GestureEventArgs e)
        {
            var list = sender as LongListSelector;
            if (list != null) Global.SelectedGoal = list.SelectedItem as GoalJoin;

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
        }

        private void FightingGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FightingCenterPage.xaml", UriKind.Relative));
        }

        private void FriendsGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/FindFriendsPage.xaml", UriKind.Relative));
        }
    }
}