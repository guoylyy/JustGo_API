using System.Windows;
using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class ChooseGoalTypePage : PhoneApplicationPage
    {
        public ObservableCollection<string> GoalTypes { get; set; } 

        public ChooseGoalTypePage()
        {
            InitializeComponent();
            GoalTypes = new ObservableCollection<string>();
            //GoalTypeListBox.DataContext = this;
            GoalTypeListBox.ItemsSource = GoalTypes;

            LoadGoalType();
        }

        private async void LoadGoalType()
        {
            if (!await ServerApi.GetCategoriesAsync(GoalTypes))
            {
                StaticMethods.ShowRequestFailedToast();
            }
        }

        private void GoalTypeListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.AddGoalType = GoalTypeListBox.SelectedItem as string;
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/ChooseDirectGoalPage.xaml", UriKind.Relative));
        }
    }
}