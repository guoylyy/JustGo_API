using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class CommentsPage : PhoneApplicationPage
    {
        public CommentsPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.CommentsViewModel;
        }

        private void CommentsButton_OnClick(object sender, EventArgs e)
        {
            if (StaticMethods.IsUserLogin())
            {
                NavigationService.Navigate(new Uri("/Pages/CommentAddPage.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
            }
            
        }

        private async void AwesomeButton_OnClick(object sender, EventArgs e)
        {
            if (StaticMethods.IsUserLogin())
            {
                var result = await ViewModelLocator.CommentsViewModel.Awesome();
                if (string.IsNullOrEmpty(result))
                {
                    MessageBox.Show("You Can't awsome a record twice");
                }
                else
                {
                    MessageBox.Show("Awsome success");
                    ViewModelLocator.CommentsViewModel.AwesomeUsers.Add(Global.LoginUser);
                }
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
            }
        }

        private void CommentsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            UserNameTextBlock.Text = Global.SelectedUserRecord.User.UserName;
            AuthorImage.Source = new BitmapImage(new Uri(Global.SelectedUserRecord.User.ImageSource));
        }
    }
}