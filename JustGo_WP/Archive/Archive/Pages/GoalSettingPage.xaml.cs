using Archive.DataBase;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace Archive.Pages
{
    public partial class GoalSettingPage : PhoneApplicationPage
    {
        public GoalSettingPage()
        {
            InitializeComponent();
        }

        private async void SettingDoneButton_OnClick(object sender, EventArgs e)
        {
            Global.AddingGoalJoin.NeedReminder = ReminderPicker.SelectedItem.ToString() == "On";
            Global.AddingGoalJoin.ReminderTime = ReminderTimePicker.Value;
            Global.AddingGoalJoin.TimeSpan = int.Parse(InsistPicker.SelectedItem.ToString().Split(' ')[0]);
            Global.AddingGoalJoin.StartDate = DateTime.Now;
            //Global.AddingGoalJoin.EndDate = DateTime.Now + TimeSpan.FromDays(Global.AddingGoalJoin.TimeSpan);

            Global.AddingGoalJoin.Frequency = string.Empty;
            Global.AddingGoalJoin.Frequency += SundayCheckBox.IsChecked.HasValue && SundayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += MondayCheckBox.IsChecked.HasValue && MondayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += TuesdayCheckBox.IsChecked.HasValue && TuesdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += WednesdayCheckBox.IsChecked.HasValue && WednesdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += ThursdayCheckBox.IsChecked.HasValue && ThursdayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += FridayCheckBox.IsChecked.HasValue && FridayCheckBox.IsChecked.Value ? "1;" : "0;";
            Global.AddingGoalJoin.Frequency += SaturdayCheckBox.IsChecked.HasValue && SaturdayCheckBox.IsChecked.Value ? "1" : "0";
            Global.AddingGoalJoin.GoalTracks = new ObservableCollection<GoalTrack>();
            Global.AddingGoalJoin.IsFinishedToday = Global.AddingGoalJoin.IsTodayPass;

            //Global.AddingGoalJoin.PassedDays = Global.AddingGoalJoin.TimeSpan;

            ViewModelLocator.GoalViewModel.MyGoals.Insert(0, Global.AddingGoalJoin);
            CsvUtil.SaveGoalJoin(ViewModelLocator.GoalViewModel.MyGoals);
            StaticMethods.UpdateTile();

            await ServerApi.PostNewJoinAsync(Global.LoginUser.Token, Global.AddingGoalJoin.GoalId);
            if(StaticMethods.IsUserLogin())
                ViewModelLocator.ExploreViewModel.AddParticipants(Global.AddingGoalJoin.GoalId, Global.LoginUser);

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}