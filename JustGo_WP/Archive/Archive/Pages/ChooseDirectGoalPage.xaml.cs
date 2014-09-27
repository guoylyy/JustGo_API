using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class ChooseDirectGoalPage : PhoneApplicationPage
    {
        //private string _goalName;
        //private string _participants;
        public ObservableCollection<Goal> Goals;

        public ChooseDirectGoalPage()
        {
            InitializeComponent();
            Goals = new ObservableCollection<Goal>();
            DirectGoalListBox.ItemsSource = Goals;

            LoadData();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            TypeName.Text = Global.AddGoalType;
        }

        private async void LoadData()
        {
            await ServerApi.GetCategoryGoalsAsync(Goals, Global.AddGoalType);
        }

        private void DirectGoalListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var simpleGoal = DirectGoalListBox.SelectedItem as Goal;
            if (simpleGoal != null)
            {
                Global.AddingGoalJoin = new GoalJoin
                {
                    GoalId = simpleGoal.GoalId,
                    GoalName = simpleGoal.GoalName,
                    Participants = simpleGoal.Participants
                };
                NavigationService.Navigate(new Uri("/Pages/GoalDetailPage.xaml", UriKind.Relative));
            }
        }
    }
}