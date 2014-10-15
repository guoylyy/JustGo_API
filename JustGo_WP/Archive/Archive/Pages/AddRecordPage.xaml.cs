using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class AddRecordPage : PhoneApplicationPage
    {
        public AddRecordPage()
        {
            InitializeComponent();
            Loaded += AddRecordPage_Loaded;
        }

        private void AddRecordPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoalNameTextBlock.Text = Global.SelectedGoalJoin.GoalName.ToUpper();
        }

        private void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            var record = new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = TextBox.Text,
                RecordTime = DateTime.Now,
                GoalId = Global.SelectedGoalJoin.GoalId,
                AwesomeUsers = new ObservableCollection<User>(),
                Comments = new ObservableCollection<Comment>()
            };
            ViewModelLocator.MyRecordsViewModel.AddRecord(record);
            NavigationService.GoBack();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var doneButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            doneButton.IsEnabled = TextBox.Text.Length != 0;
        }
    }
}