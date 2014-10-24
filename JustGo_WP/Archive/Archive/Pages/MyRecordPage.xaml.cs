using Archive.DataBase;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class MyRecordPage : PhoneApplicationPage
    {
        private ApplicationBar _myRecordApplicationBar;
        private ApplicationBar _deleteApplicationBar;
        private ApplicationBarIconButton _refreshBarButton;
        private bool _isNewInstance;

        public MyRecordPage()
        {
            InitializeComponent();
            TopGrid.DataContext = Global.SelectedGoalJoin;
            InitAppbar();
            RecordsPivotItem.DataContext = ViewModelLocator.MyRecordsViewModel;
            Loaded += GoalDetailPage_Loaded;
            LayoutUpdated += GoalDetailPage_LayoutUpdated;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _isNewInstance = e.NavigationMode == NavigationMode.New;
        }

        private void InitAppbar()
        {
            _myRecordApplicationBar = new ApplicationBar
            {
                BackgroundColor = (Color) Application.Current.Resources["AppbarBackgroundColor"],
                ForegroundColor = (Color) Application.Current.Resources["AppbarForegroundColor"],
                Opacity = 0.99
            };

            var addBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/add.png", UriKind.Relative));
            addBarButton.Text = "add";
            addBarButton.Click += AddBarButton_Click;

            _refreshBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh2.png", UriKind.Relative));
            _refreshBarButton.Text = "refresh";
            _refreshBarButton.Click += RefreshBarButton_Click;

            _myRecordApplicationBar.Buttons.Add(addBarButton);
            _myRecordApplicationBar.Buttons.Add(_refreshBarButton);


            _deleteApplicationBar = new ApplicationBar
            {
                BackgroundColor = (Color)Application.Current.Resources["AppbarBackgroundColor"],
                ForegroundColor = (Color)Application.Current.Resources["AppbarForegroundColor"],
                Opacity = 0.99
            };

            var deleteBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/minus.png",UriKind.Relative));
            deleteBarButton.Text = "remove";
            deleteBarButton.Click += OnDeleteBarButton_Click;

            _deleteApplicationBar.Buttons.Add(deleteBarButton);
        }

        private void OnDeleteBarButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to remove this goal?", "Waring", MessageBoxButton.OKCancel)
                == MessageBoxResult.OK)
            {
                ViewModelLocator.GoalViewModel.RemoveGoalJoin(Global.SelectedGoalJoin);
                NavigationService.GoBack();
            }
        }

        private void AddBarButton_Click(object sender, EventArgs e)
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

        private async void RefreshBarButton_Click(object sender, EventArgs e)
        {
            if (StaticMethods.IsUserLogin())
            {
                _refreshBarButton.IsEnabled = false;
                await ViewModelLocator.MyRecordsViewModel.LoadRecord(true);
                _refreshBarButton.IsEnabled = true;
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
            }
            
        }

        private void GoalDetailPage_LayoutUpdated(object sender, EventArgs e)
        {
            DoneImage.Margin = new Thickness(0, 10, (LayoutRoot.ActualWidth - TestBlock.ActualWidth) / 2 - 35, 0);
        }

        private async void GoalDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (StaticMethods.IsUserLogin())
            {
                ViewModelLocator.MyRecordsViewModel.LoadRecord(_isNewInstance);
                RecordUserImage.Source = new BitmapImage(new Uri(Global.LoginUser.ImageSourceMedium));
            }
            else
            {
                RecordUserImage.Source = new BitmapImage(new Uri("/Assets/DefaultHeader.jpg", UriKind.Relative));
            }

            var count = await ViewModelLocator.MyRecordsViewModel.LoadRecordsCountAsync();
            BottomGrid.Visibility = Visibility.Visible;
            RecordsCountTextBlock.Text = count.ToString(CultureInfo.InvariantCulture);

            //RecordLongListSelector.UpdateLayout();
        }

        private void DoneGrid_OnTap(object sender, GestureEventArgs e)
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
            StaticMethods.UpdateTile();

            if (Global.SelectedGoalJoin.IsDone) Global.SelectedGoalJoin.EndDate = DateTime.Now;
        }

        private void BottomGrid_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/RecordsForGoalPage.xaml",UriKind.Relative));
        }

        private void MoreComments_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CommentsPage.xaml", UriKind.Relative));
        }

        private void Pivot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBar = e.AddedItems[0].Equals(RecordsPivotItem) ? _myRecordApplicationBar : _deleteApplicationBar;
        }

        private void RecordLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.SelectedUserRecord = RecordLongListSelector.SelectedItem as UserRecord;
        }

        private void CommentGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserProfilePage.xaml", UriKind.Relative));
        }

        private void CommentsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox) sender;
            if (listBox.SelectedItems != null && listBox.SelectedItems.Count != 0)
            {
                var comment = (Comment)listBox.SelectedItems[0];
                Global.SelectedUser = comment.User;
            }
        }
    }
}