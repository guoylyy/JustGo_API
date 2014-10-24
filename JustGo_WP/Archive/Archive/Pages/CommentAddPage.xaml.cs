using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class CommentAddPage : PhoneApplicationPage
    {
        private ApplicationBarIconButton _commentDoneButton;
        public CommentAddPage()
        {
            InitializeComponent();
            InitAppBar();
        }

        private void InitAppBar()
        {
            ApplicationBar = new ApplicationBar
            {
                BackgroundColor = (Color)Application.Current.Resources["AppbarBackgroundColor"],
                ForegroundColor = (Color)Application.Current.Resources["AppbarForegroundColor"],
                Opacity = 0.99
            };
            _commentDoneButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/check.png",UriKind.Relative),
                Text = "send",
                IsEnabled = false
            };
            _commentDoneButton.Click += ApplicationBarIconButton_OnClick;

            ApplicationBar.Buttons.Add(_commentDoneButton);
        }

        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
            _commentDoneButton.IsEnabled = false;

            var str = await ServerApi.PostRecordCommentAsync(Global.SelectedUserRecord.GoalRecordId,
                Global.LoginUser.Token, CommentTextBox.Text);
            if (!string.IsNullOrEmpty(str))
            {
                var comment = new Comment
                {
                    User = Global.LoginUser,
                    CommentContent = CommentTextBox.Text
                };
                Global.AddingComment = comment;

                Global.SelectedUserRecord.Comments.Insert(0, comment);
                if (Global.SelectedUserRecord.Comments.Count > 5)
                {
                    Global.SelectedUserRecord.Comments.RemoveAt(5);
                }

                NavigationService.GoBack();
            }
            else
            {
                StaticMethods.ShowRequestFailedToast();
                Global.AddingComment = null;
                NavigationService.GoBack();
            }
        }

        private void CommentTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _commentDoneButton.IsEnabled = CommentTextBox.Text.Length != 0;
        }
    }
}