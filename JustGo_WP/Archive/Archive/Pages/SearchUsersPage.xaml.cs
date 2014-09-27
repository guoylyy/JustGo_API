using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class SearchUsersPage : PhoneApplicationPage
    {
        public SearchUsersPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.SearchUsersViewModel;
        }

        private void SearchUsersPage_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModelLocator.SearchUsersViewModel.LoadData(SearchTextBox.Text);
            }
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserProfilePage.xaml", UriKind.Relative));
        }

        private void LongListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.SelectedUser = UserList.SelectedItem as User;
        }
    }
}