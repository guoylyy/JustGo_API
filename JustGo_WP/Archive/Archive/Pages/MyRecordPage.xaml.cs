using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class MyRecordPage : PhoneApplicationPage
    {
        public MyRecordPage()
        {
            InitializeComponent();
            TopGrid.DataContext = Global.SelectedGoal;
            //RecordsPivotItem.DataContext = ViewModelLocator.MyRecordsViewModel;
            Loaded += GoalDetailPage_Loaded;
            LayoutUpdated += GoalDetailPage_LayoutUpdated;
        }

        void GoalDetailPage_LayoutUpdated(object sender, EventArgs e)
        {
            DoneImage.Margin = new Thickness(0, 10, (LayoutRoot.ActualWidth - TestBlock.ActualWidth) / 2 - 35, 0);
        }

        void GoalDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            //DoneImage.Margin = new Thickness(0, 10, (LayoutRoot.ActualWidth - TestBlock.ActualWidth) / 2 - 35, 0);
        }

        private void DoneGrid_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Global.SelectedGoal.IsFinished = true;
            Global.SelectedGoal.PassedDays++;
        }

        private void BottomGrid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/RecordsForGoalPage.xaml",UriKind.Relative));
        }

        private void MoreComments_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CommentsPage.xaml", UriKind.Relative));
        }
    }
}