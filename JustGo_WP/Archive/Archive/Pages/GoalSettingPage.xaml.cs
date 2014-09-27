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
            Global.AddingGoalJoin.NeedReminder = ReminderPicker.SelectedItem.ToString() == "On";
            Global.AddingGoalJoin.ReminderTime = ReminderTimePicker.Value;
            Global.AddingGoalJoin.TimeSpan = int.Parse(InsistPicker.SelectedItem.ToString().Split(' ')[0]);
            Global.AddingGoalJoin.StartDate = DateTime.Now;
            Global.AddingGoalJoin.EndDate = DateTime.Now + TimeSpan.FromDays(Global.AddingGoalJoin.TimeSpan);

            Global.AddingGoalJoin.Frequency = string.Empty;
            Global.AddingGoalJoin.Frequency += SundayCheckBox.IsChecked != null && SundayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += MondayCheckBox.IsChecked != null && MondayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += TuesdayCheckBox.IsChecked != null && TuesdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += WednesdayCheckBox.IsChecked != null && WednesdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += ThursdayCheckBox.IsChecked != null && ThursdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += FridayCheckBox.IsChecked != null && FridayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += SaturdayCheckBox.IsChecked != null && SaturdayCheckBox.IsChecked.Value ? "1" : "0";
            Global.AddingGoalJoin.GoalTracks = new ObservableCollection<GoalTrack>();

            ViewModelLocator.GoalViewModel.MyGoals.Insert(0, Global.AddingGoalJoin);
            CsvUtil.SaveGoalJoin(ViewModelLocator.GoalViewModel.MyGoals);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}