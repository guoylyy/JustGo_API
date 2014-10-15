using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class CommentAddPage : PhoneApplicationPage
    {
        public CommentAddPage()
        {
            InitializeComponent();
        }

        private async void ApplicationBarIconButton_OnClick(object sender, EventArgs e)
        {
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
            var sendButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            sendButton.IsEnabled = CommentTextBox.Text.Length != 0;
        }
    }
}