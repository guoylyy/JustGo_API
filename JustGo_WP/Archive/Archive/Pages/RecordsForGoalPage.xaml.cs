using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Archive.Datas;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class GoalsPopularPage : PhoneApplicationPage
    {
        public GoalsPopularPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            HeadGrid.DataContext = Global.SelectedGoal;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CommentsPage.xaml", UriKind.Relative));
        }

        private void RecordList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = RecordList.SelectedItem as UserRecord;
        }


    }
}