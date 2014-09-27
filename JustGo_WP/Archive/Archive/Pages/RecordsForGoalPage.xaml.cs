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
        public GoalsPopularPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.RecordsForGoalViewModel;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoalNameTextBlock.Text = Global.SelectedGoalJoin.GoalName;
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
            var item = RecordList.SelectedItem as UserRecord;
            Global.SelectedUserRecord = item;
        }
    }
}