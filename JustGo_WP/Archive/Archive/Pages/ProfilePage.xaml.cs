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
            var button = (ApplicationBarIconButton)sender;
            button.IsEnabled = false;

            var result = await ServerApi.PostUserProfileAsync(Global.LoginUser.Token,
                NameTextBox.Text, DescriptionTextBox.Text);
            if (string.IsNullOrEmpty(result))
            {
                StaticMethods.ShowRequestFailedToast();
                button.IsEnabled = true;
            }
            else
            {
                Global.LoginUser.UserName = NameTextBox.Text;
                Global.LoginUser.Description = DescriptionTextBox.Text;

                StaticMethods.WriteUser(Global.LoginUser);
                StaticMethods.ShowToast("Changes have saved!");
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text.Length == 0)
            {
                var appbarButton = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
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