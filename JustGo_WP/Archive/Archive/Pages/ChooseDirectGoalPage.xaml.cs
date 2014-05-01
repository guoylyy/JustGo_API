using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class ChooseDirectGoalPage : PhoneApplicationPage
    {
        //private string _goalName;
        //private string _participants;
        public ObservableCollection<SimpleGoal> SimpleGoals;

        public ChooseDirectGoalPage()
        {
            InitializeComponent();
            SimpleGoals = new ObservableCollection<SimpleGoal>();
            DirectGoalListBox.ItemsSource = SimpleGoals;

            LoadData();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            TypeName.Text = Global.AddGoalType;
        }

        private void LoadData()
        {
            SimpleGoals.Add(new SimpleGoal { GoalName = "Drink more water", Participants = string.Format("{0} participants", 12345) });
            SimpleGoals.Add(new SimpleGoal { GoalName = "Eat a vegan diet", Participants = string.Format("{0} participants", 5432) });
            SimpleGoals.Add(new SimpleGoal { GoalName = "Eat fruit", Participants = string.Format("{0} participants", 145) });
            SimpleGoals.Add(new SimpleGoal { GoalName = "Floss", Participants = string.Format("{0} participants", 5643) });
        }

        private void DirectGoalListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var simpleGoal = DirectGoalListBox.SelectedItem as SimpleGoal;
            if (simpleGoal != null)
            {
                Global.AddGoalName = simpleGoal.GoalName;
                Global.GoalParticipantString = simpleGoal.Participants;
                NavigationService.Navigate(new Uri("/Pages/GoalDetailPage.xaml", UriKind.Relative));
            }
        }


        public class SimpleGoal : INotifyPropertyChanged
        {
            private string _goalName;
            public string GoalName
            {
                get { return _goalName; }

                set
                {
                    if (value != _goalName)
                    {
                        _goalName = value;
                        NotifyPropertyChanged("GoalName");
                    }
                }
            }

            private string _participants;
            public string Participants
            {
                get { return _participants; }

                set
                {
                    if (value != _participants)
                    {
                        _participants = value;
                        NotifyPropertyChanged("Participants");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void NotifyPropertyChanged(String propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (null != handler)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}