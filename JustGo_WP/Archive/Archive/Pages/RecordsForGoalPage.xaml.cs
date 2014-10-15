using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class GoalsPopularPage : PhoneApplicationPage
    {
        private bool _isUserHeaderTap = false;

        public GoalsPopularPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.RecordsForGoalViewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var b = e.NavigationMode == NavigationMode.New;

            if (await ViewModelLocator.RecordsForGoalViewModel.LoadRecords(b))
            {
                ProgressGrid.Visibility = Visibility.Collapsed;
                ContentPanel.Opacity = 100;
                
                if (RecordList.ItemsSource.Count > 0)
                {
                    Rectangle.Visibility = Visibility.Visible;
                    if (e.NavigationMode != NavigationMode.Back)
                    {
                        RecordList.ScrollTo(ViewModelLocator.RecordsForGoalViewModel.Records[0]);    
                    }
                    
                }
            }
            else
            {
                ProgressGrid.Visibility = Visibility.Collapsed;
                ContentPanel.Opacity = 100;
                Rectangle.Visibility = Visibility.Collapsed;
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoalNameTextBlock.Text = Global.SelectedGoalJoin.GoalName.ToUpper();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CommentsPage.xaml", UriKind.Relative));
        }

        private void AwesomeUsers_OnTap(object sender, GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Pages/AwesomePage.xaml", UriKind.Relative));
        }

        private void RecordList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (UserRecord)RecordList.SelectedItem;
            Global.SelectedUserRecord = item;
            if (_isUserHeaderTap)
            {
                Global.SelectedUser = item.User;
                _isUserHeaderTap = false;
            }
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

        private void UserHeaderGrid_OnTap(object sender, GestureEventArgs e)
        {
            _isUserHeaderTap = true;
            NavigationService.Navigate(new Uri("/Pages/UserProfilePage.xaml", UriKind.Relative));
        }

        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            if (await ViewModelLocator.RecordsForGoalViewModel.LoadRecords(true))
            {
                Rectangle.Visibility = Visibility.Visible;
                if (RecordList.ItemsSource.Count > 0)
                    RecordList.ScrollTo(RecordList.ItemsSource[0]);    
            }
            else
            {
                Rectangle.Visibility = Visibility.Collapsed;
            }
            
        }
    }
}