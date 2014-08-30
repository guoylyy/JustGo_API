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
    public partial class FollowPage : PhoneApplicationPage
    {
        public FollowPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string type;
            if (NavigationContext.QueryString.TryGetValue("type", out type))
            {
                switch (type)
                {
                    case "follower":
                        TopicTextBlock.Text = "50 FOLLOWER";
                        break;
                    case "following":
                        TopicTextBlock.Text = "50 FOLLOWING";
                        break;
                }
            }
        }
    }
}