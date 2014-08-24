using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class CommentsPage : PhoneApplicationPage
    {
        public CommentsPage()
        {
            InitializeComponent();
        }

        private void CommentsButton_OnClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CommentAddPage.xaml", UriKind.Relative));
        }

        private void AwesomeButton_OnClick(object sender, EventArgs e)
        {
            //todo:点赞
        }
    }
}