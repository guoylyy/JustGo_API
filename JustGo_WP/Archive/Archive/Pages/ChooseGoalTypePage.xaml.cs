using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class ChooseGoalTypePage : PhoneApplicationPage
    {
        public ObservableCollection<string> GoalTypes { get; set; } 
        public ChooseGoalTypePage()
        {
            InitializeComponent();
            GoalTypes = new ObservableCollection<string>();
            GoalTypeListBox.DataContext = this;

            LoadGoalType();
        }

        private void LoadGoalType()
        {
            GoalTypes.Add("Popular");
            GoalTypes.Add("Health diet");
            GoalTypes.Add("Train plans");
            GoalTypes.Add("Habits");
            GoalTypes.Add("Learning");
        }

        private void GoalTypeListBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Global.AddGoalType = GoalTypeListBox.SelectedItem as string;
            NavigationService.Navigate(new Uri("/Pages/ChooseDirectGoalPage.xaml", UriKind.Relative));
        }
    }
}