using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Archive.Datas;
using Archive.ViewModel;
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class CommentsPage : PhoneApplicationPage
    {
        private CommentsViewModel _commentsViewModel;
        //private bool _hasLoadData = false;

        public CommentsPage()
        {
            InitializeComponent();
            //DataContext = ViewModelLocator.CommentsViewModel;
            _commentsViewModel = new CommentsViewModel();
            DataContext = _commentsViewModel;
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
                    if (StaticMethods.IsNetworkEnable())
                    {
                        StaticMethods.ShowToast("You can't awsome a record twice");
                    }
                    else
                    {
                        StaticMethods.ShowRequestFailedToast();
                    }
                }
                else
                {
                    _commentsViewModel.AwesomeUsers.Add(Global.LoginUser);
                    Global.SelectedUserRecord.AwesomeUsers.Add(Global.LoginUser);
                }
            }
            else
            {
                NavigationService.Navigate(new Uri("/Pages/LoginForTouristPage.xaml", UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (Global.AddingComment != null)
                {
                    var comment = new Comment
                    {
                        User = Global.AddingComment.User,
                        CommentContent = Global.AddingComment.CommentContent
                    };
                    _commentsViewModel.Comments.Insert(0, comment);
                    Global.AddingComment = null;
                }
            }
        }

        private async void CommentsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
                UserNameTextBlock.Text = Global.SelectedUserRecord.User.UserName;
                AuthorImage.Source = new BitmapImage(new Uri(Global.SelectedUserRecord.User.ImageSource));
                await _commentsViewModel.LoadData();
                //_hasLoadData = true;

                ProgressGrid.Visibility = Visibility.Collapsed;
                ContentPanelGrid.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = true;
        }

        private void CommentsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            if (listBox.SelectedItems != null && listBox.SelectedItems.Count != 0)
            {
                var comment = (Comment)listBox.SelectedItems[0];
                Global.SelectedUser = comment.User;
            }
        }

        private void CommentGrid_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserProfilePage.xaml", UriKind.Relative));
        }
    }
}