using System.ComponentModel;
using System.Windows.Media;
using Archive.Datas;
using Archive.ViewModel;
using Microsoft.Phone.Controls;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Archive.Pages
{
    public partial class AchievementsPage : PhoneApplicationPage
    {
        private int _goalCount;

        public AchievementsPage()
        {
            InitializeComponent();
            InitAppbar();
        }

        private void InitAppbar()
        {
            var appBar = new ApplicationBar
            {
                BackgroundColor = (Color) Application.Current.Resources["AppbarBackgroundColor"],
                ForegroundColor = (Color) Application.Current.Resources["AppbarForegroundColor"],
                Opacity = 0.99
            };

            var shareButton = new ApplicationBarIconButton
            {
                IconUri = new Uri("/Assets/AppBar/share.png", UriKind.Relative),
                Text = "share",
            };
            shareButton.Click += shareButton_Click;

            appBar.Buttons.Add(shareButton);

            ApplicationBar = appBar;
            ApplicationBar.IsVisible = false;
        }

        private void shareButton_Click(object sender, EventArgs e)
        {
            if (MessageRadioButton.IsChecked.HasValue && MessageRadioButton.IsChecked.Value)
            {
                var smsComposeTask = new SmsComposeTask
                {
                    Body = string.Format("I have accomplished {0} times of my daily goals. Come and insist on your daily goals with the people all over the world!", _goalCount)
                           + Environment.NewLine
                           + "http://www.windowsphone.com/s?appid=46f060c1-9afc-4082-9a0c-d7f41dabf68c"
                };

                smsComposeTask.Show();
            }
            else
            {
                var emailComposeTask = new EmailComposeTask
                {
                    Subject = "Share my achievement",
                    Body = string.Format("I have accomplished {0} times of my daily goals. Come and insist on your daily goals with the people all over the world!", _goalCount)
                           + Environment.NewLine
                           + "http://www.windowsphone.com/s?appid=46f060c1-9afc-4082-9a0c-d7f41dabf68c"
                };

                emailComposeTask.Show();
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (ShareGrid.Visibility == Visibility.Visible)
            {
                e.Cancel = true;
                ShareGrid.Visibility = Visibility.Collapsed;
                ApplicationBar.IsVisible = false;
            }
        }

        private void AchievementsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            DayofMonthTextBlock.Text = DateTime.Now.Day + "th ";

            _goalCount = ViewModelLocator.GoalViewModel.MyGoals.Sum(goalJoin => goalJoin.PassedDays);
            GoalInsistTextBlock.Text = _goalCount.ToString(CultureInfo.InvariantCulture);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_goalCount > 0)
            {
                ShareGrid.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = true;
            }
            else
            {
                MessageBox.Show("You need to accomplish at least one daily goal.");
            }
        }

        private void MessageRadioButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(MessageRadioButton.IsChecked.HasValue && MessageRadioButton.IsChecked.Value) return;

            MessageRadioButton.IsChecked = true;
            EmailRadioButton.IsChecked = false;
        }

        private void EmailRadioButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (EmailRadioButton.IsChecked.HasValue && EmailRadioButton.IsChecked.Value) return;

            MessageRadioButton.IsChecked = false;
            EmailRadioButton.IsChecked = true;
        }

        private void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageRadioButton.IsChecked.HasValue && MessageRadioButton.IsChecked.Value)
            {
                var smsComposeTask = new SmsComposeTask
                {
                    Body = string.Format("I have accomplished {0} times of my daily goals. Come and insist on your daily goals with the people all over the world!", _goalCount)
                           + Environment.NewLine
                           + "http://www.windowsphone.com/s?appid=46f060c1-9afc-4082-9a0c-d7f41dabf68c"
                };

                smsComposeTask.Show();
            }
            else
            {
                var emailComposeTask = new EmailComposeTask
                {
                    Subject = "Share my achievement",
                    Body = string.Format("I have accomplished {0} times of my daily goals. Come and insist on your daily goals with the people all over the world!", _goalCount)
                           + Environment.NewLine
                           + "http://www.windowsphone.com/s?appid=46f060c1-9afc-4082-9a0c-d7f41dabf68c"
                };

                emailComposeTask.Show();
            }
            
        }
    }
}