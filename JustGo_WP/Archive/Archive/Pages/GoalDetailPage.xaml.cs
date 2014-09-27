using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class GoalDetailPage : PhoneApplicationPage
    {
        //private string _description;

        public GoalDetailPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.GoalDetailViewModel;
            //ParticipantsImageList.DataContext = this;
            //ViewModelLocator.GoalDetailViewModel.LoadData();
           
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.GoalDetailViewModel.GoalName = Global.AddingGoalJoin.GoalName;
            ViewModelLocator.GoalDetailViewModel.Joins = Global.AddingGoalJoin.Participants;
            //DescriptionBlock.Text = _description;
            LoadData();
        }

        private void LoadData()
        {
            ViewModelLocator.GoalDetailViewModel.LoadData();
        }

        private void JoinButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/GoalSettingPage.xaml", UriKind.Relative));
        }
    }
}