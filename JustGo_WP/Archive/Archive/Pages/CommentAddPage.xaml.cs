using System;
using System.Collections.Generic;
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
                ViewModelLocator.CommentsViewModel.Comments.Insert(0, new Comment
                {
                    User = Global.LoginUser,
                    CommentContent = CommentTextBox.Text
                });
                MessageBox.Show("Comment successed");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Comment failed");
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