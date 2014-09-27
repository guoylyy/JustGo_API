using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Archive.DataBase;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class MyRecordPage : PhoneApplicationPage
    {
        private ApplicationBar _myRecordApplicationBar;

        public MyRecordPage()
        {
            InitializeComponent();
            TopGrid.DataContext = Global.SelectedGoalJoin;
            InitAppbar();
            RecordsPivotItem.DataContext = ViewModelLocator.MyRecordsViewModel;
            Loaded += GoalDetailPage_Loaded;
            LayoutUpdated += GoalDetailPage_LayoutUpdated;
        }

        private void InitAppbar()
        {
            _myRecordApplicationBar = new ApplicationBar();
            _myRecordApplicationBar.BackgroundColor = (Color)Application.Current.Resources["AppbarBackgroundColor"];
            _myRecordApplicationBar.ForegroundColor = (Color)Application.Current.Resources["AppbarForegroundColor"];
            _myRecordApplicationBar.Opacity = 0.99;

            var addBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            addBarButton.Text = "add";
            addBarButton.Click += AddBarButton_Click;

            var refreshBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh2.png",UriKind.Relative));
            refreshBarButton.Text = "refresh";
            refreshBarButton.Click += RefreshBarButton_Click;

            _myRecordApplicationBar.Buttons.Add(addBarButton);
            _myRecordApplicationBar.Buttons.Add(refreshBarButton);
        }

        void AddBarButton_Click(object sender, EventArgs e)
        {
            if (StaticMethods.IsUserLogin())
            {
                NavigationService.Navigate(new Uri("/Pages/AddRecordPage.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
            }
            
        }

        void RefreshBarButton_Click(object sender, EventArgs e)
        {
            if (StaticMethods.IsUserLogin())
            {
                ViewModelLocator.MyRecordsViewModel.LoadRecord();
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
            }
            
        }

        void GoalDetailPage_LayoutUpdated(object sender, EventArgs e)
        {
            DoneImage.Margin = new Thickness(0, 10, (LayoutRoot.ActualWidth - TestBlock.ActualWidth) / 2 - 35, 0);
        }

        void GoalDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ViewModelLocator.MyRecordsViewModel.IsLoaded && StaticMethods.IsUserLogin())
            {
                ViewModelLocator.MyRecordsViewModel.LoadRecord();
                RecordUserImage.Source = new BitmapImage(new Uri(Global.LoginUser.ImageSource));
            }
            else
            {
                RecordUserImage.Source = new BitmapImage(new Uri("/Assets/DefaultHeader.jpg",UriKind.Relative));
            }
        }

        private void DoneGrid_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Global.SelectedGoalJoin.IsFinishedToday = true;
            Global.SelectedGoalJoin.PassedDays++;

            var golaTrack = new GoalTrack
            {
                GoalJoinId = Global.SelectedGoalJoin.GoalId,
                GoalTrackId = Guid.NewGuid().ToString(),
                TrackTime = DateTime.Now,
                UpDateTime = DateTime.Now
            };
            Global.SelectedGoalJoin.GoalTracks.Insert(0, golaTrack);
            CsvUtil.SaveGoalTrack(Global.SelectedGoalJoin.GoalTracks,Global.SelectedGoalJoin.GoalTracksId);
        }

        private void BottomGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/RecordsForGoalPage.xaml",UriKind.Relative));
        }

        private void MoreComments_OnClick(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("more comments clicked");
            //Global.SelectedUserRecord = 
            NavigationService.Navigate(new Uri("/Pages/CommentsPage.xaml", UriKind.Relative));
        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBar = e.AddedItems[0].Equals(RecordsPivotItem) ? _myRecordApplicationBar : null;
        }

        private void RecordLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(".......List selection changed");
            Global.SelectedUserRecord = RecordLongListSelector.SelectedItem as UserRecord;
        }
    }
}