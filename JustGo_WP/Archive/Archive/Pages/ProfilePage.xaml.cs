using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            DataContext = Global.LoginUser;
        }

        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            var button = (ApplicationBarIconButton) sender;
            button.IsEnabled = false;

            Global.LoginUser.UserName = NameTextBox.Text;
            Global.LoginUser.Description = DescriptionTextBox.Text;
           
            await ServerApi.PostUserProfileAsync(Global.LoginUser.Token, NameTextBox.Text, DescriptionTextBox.Text);
            StaticMethods.WriteUser(Global.LoginUser);

            if (MessageBox.Show("Changes have saved!") == MessageBoxResult.OK)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text.Length == 0)
            {
                var appbarButton = (ApplicationBarIconButton) ApplicationBar.Buttons[0];
                appbarButton.IsEnabled = false;
            }
        }

        private void DescriptionTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DescriptionTextBox.Text.Length == 0)
            {
                var appbarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
                appbarButton.IsEnabled = false;
            }
        }
    }
}