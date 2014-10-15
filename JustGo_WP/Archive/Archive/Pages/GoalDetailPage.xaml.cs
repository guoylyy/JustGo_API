using System.Threading.Tasks;
using System.Windows.Controls;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class GoalDetailPage : PhoneApplicationPage
    {
        //private string _description;

        public GoalDetailPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.GoalDetailViewModel;
           
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.GoalDetailViewModel.GoalName = Global.AddingGoalJoin.GoalName;
            ViewModelLocator.GoalDetailViewModel.Joins = Global.AddingGoalJoin.Participants;

            if (ViewModelLocator.GoalViewModel.MyGoals.Any(g => g.GoalId == Global.AddingGoalJoin.GoalId && !g.IsDone))
            {
                JoinButton.Visibility = Visibility.Collapsed;
                JoinedTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                JoinButton.Visibility = Visibility.Visible;
                JoinedTextBlock.Visibility = Visibility.Collapsed;
            }

            LoadData();
        }

        private async void LoadData()
        {
            ProgressGrid.Visibility = Visibility.Visible;
            ContentGrid.Visibility = Visibility.Collapsed;

            //await Task.Delay(20);
            if (await ViewModelLocator.GoalDetailViewModel.LoadData())
            {
                ProgressGrid.Visibility = Visibility.Collapsed;
                ContentGrid.Visibility = Visibility.Visible;    
            }
            else
            {
                ProgressGrid.Visibility = Visibility.Collapsed;
                //ContentGrid.Visibility = Visibility.Visible;
            }
        }

        private void JoinButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/GoalSettingPage.xaml", UriKind.Relative));
        }

        private void UserGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserProfilePage.xaml", UriKind.Relative));
        }

        private void RecordsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var record = (UserRecord) RecordsList.SelectedItem;
            Global.SelectedUser = record.User;
        }
    }
}