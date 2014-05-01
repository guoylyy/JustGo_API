using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Archive.Datas;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Archive.Pages
{
    public partial class GoalDetailPage : PhoneApplicationPage
    {
        private string _description;
        public ObservableCollection<User> Participants { get; set; }

        public GoalDetailPage()
        {
            InitializeComponent();
            Participants = new ObservableCollection<User>();
            ParticipantsImageList.DataContext = this;
            LoadData();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoalNameBlock.Text = Global.AddGoalName;
            ParticipantsBlock.Text = Global.GoalParticipantString;
            DescriptionBlock.Text = _description;
        }

        private void LoadData()
        {
            _description = "This is a 21-day goal to help you build the habit of “Drink more water.”"
                + " Each day is supported by optional discussion prompts.";

            Participants.Add(new User
            {
                ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg", UriKind.Relative))
            });
            Participants.Add(new User
            {
                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head6.png", UriKind.Relative))
            });
            Participants.Add(new User
            {
                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head7.png", UriKind.Relative))
            });
            Participants.Add(new User
            {
                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head8.png", UriKind.Relative))
            });
            Participants.Add(new User
            {
                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head9.png", UriKind.Relative))
            });
        }
    }
}