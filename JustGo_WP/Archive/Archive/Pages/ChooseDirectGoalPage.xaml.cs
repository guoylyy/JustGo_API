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

        private void LoadData()
        {
            Goals.Add(new Goal { GoalId = "1", GoalName = "Drink more water", Participants = string.Format("{0} participants", 12345) });
            Goals.Add(new Goal { GoalId = "2", GoalName = "Eat a vegan diet", Participants = string.Format("{0} participants", 5432) });
            Goals.Add(new Goal { GoalId = "3", GoalName = "Eat fruit", Participants = string.Format("{0} participants", 145) });
            Goals.Add(new Goal { GoalId = "4", GoalName = "Floss", Participants = string.Format("{0} participants", 5643) });
        }

        private void DirectGoalListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var simpleGoal = DirectGoalListBox.SelectedItem as Goal;
            if (simpleGoal != null)
            {
                Global.AddGoal = new GoalJoin
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