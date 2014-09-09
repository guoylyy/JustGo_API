using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Archive.DataBase;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class GoalSettingPage : PhoneApplicationPage
    {
        public GoalSettingPage()
        {
            InitializeComponent();
        }

        private void SettingDoneButton_OnClick(object sender, EventArgs e)
        {
            Global.AddGoal.NeedReminder = ReminderPicker.SelectedItem.ToString() == "On";
            Global.AddGoal.ReminderTime = ReminderTimePicker.Value;
            Global.AddGoal.TimeSpan = int.Parse(InsistPicker.SelectedItem.ToString().Split(' ')[0]);
            Global.AddGoal.StartDate = DateTime.Now;
            Global.AddGoal.EndDate = DateTime.Now + TimeSpan.FromDays(Global.AddGoal.TimeSpan);

            Global.AddGoal.Frequency = string.Empty;
            Global.AddGoal.Frequency += SundayCheckBox.IsChecked != null && SundayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddGoal.Frequency += MondayCheckBox.IsChecked != null && MondayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddGoal.Frequency += TuesdayCheckBox.IsChecked != null && TuesdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddGoal.Frequency += WednesdayCheckBox.IsChecked != null && WednesdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddGoal.Frequency += ThursdayCheckBox.IsChecked != null && ThursdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddGoal.Frequency += FridayCheckBox.IsChecked != null && FridayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddGoal.Frequency += SaturdayCheckBox.IsChecked != null && SaturdayCheckBox.IsChecked.Value ? "1" : "0";
            Global.AddGoal.GoalTracks = new ObservableCollection<GoalTrack>();

            ViewModelLocator.GoalViewModel.MyGoals.Insert(0, Global.AddGoal);
            CsvUtil.SaveGoalJoin(ViewModelLocator.GoalViewModel.MyGoals);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}