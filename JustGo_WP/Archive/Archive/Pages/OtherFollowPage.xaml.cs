﻿using System;
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
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Archive.Pages
{
    public partial class OtherFollowPage : PhoneApplicationPage
    {
        private bool _isFans;
        private string _userId;
        private OtherFollowViewModel _viewModel;

        public OtherFollowPage()
        {
            InitializeComponent();

            _userId = Global.SelectedUser.UserId;
            _viewModel = new OtherFollowViewModel();
            DataContext = _viewModel;
            //DataContext = ViewModelLocator.OtherFollowViewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string type;
            if (NavigationContext.QueryString.TryGetValue("type", out type))
            {
                switch (type)
                {
                    case "follower":
                        _isFans = true;
                        break;
                    case "following":
                        _isFans = false;
                        break;
                }
            }
        }

        private async void FollowPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_isFans)
            {
                await _viewModel.LoadFollowers(_userId);
                TopicTextBlock.Text = _viewModel.FollowPersons.Count + " FOLLOWERS";
            }
            else
            {
                await _viewModel.LoadFollowings(_userId);
                TopicTextBlock.Text = _viewModel.FollowPersons.Count + " FOLLOWINGS";
            }
            ProgressGrid.Visibility = Visibility.Collapsed;
        }

        private void PeopleLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.SelectedUser = PeopleLongListSelector.SelectedItem as User;
        }

        private void UIElement_OnTap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/UserProfilePage.xaml", UriKind.Relative));
        }
    }
}