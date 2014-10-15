using System.Diagnostics;
using System.Windows;
using Archive.Datas;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Archive.ViewModel
{
    public class CommentsViewModel : INotifyPropertyChanged
    {
        public bool IsLoaded { get; private set; }
        private DateTime _refreshTime;

        public ObservableCollection<User> AwesomeUsers { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        private string _topic;
        public string Topic
        {
            get { return _topic; }
            set
            {
                if (value != _topic)
                {
                    _topic = value;
                    NotifyPropertyChanged("Topic");
                }
            }
        }

        private DateTime _time;
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                if (value != _time)
                {
                    _time = value;
                    NotifyPropertyChanged("Time");
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    NotifyPropertyChanged("UserName");
                }
            }
        }

        private string _goalImage;
        public string GoalImage
        {
            get
            {
                return _goalImage;
            }
            set
            {
                if (value != _goalImage)
                {
                    _goalImage = value;
                    NotifyPropertyChanged("GoalImage");
                }
            }
        }

        public CommentsViewModel()
        {
            AwesomeUsers = new ObservableCollection<User>();
            Comments = new ObservableCollection<Comment>();
        }

        public async Task<string> Awesome()
        {
            return await ServerApi.PostRecordAwesomeAsync(Global.SelectedUserRecord.GoalRecordId,
                Global.LoginUser.Token);
        }

        public async Task LoadData()
        {
            if (!IsLoaded || DateTime.Now - _refreshTime > TimeSpan.FromMinutes(1))
            {
                AwesomeUsers.Clear();
                Comments.Clear();

                Topic = Global.SelectedUserRecord.RecordContent;
                Time = Global.SelectedUserRecord.RecordTime;
                UserName = Global.SelectedUserRecord.User.UserName;

                GoalImage = await ServerApi.GetGoalImageAsync(Global.SelectedUserRecord.GoalId);
                //GoalImage = StaticMethods.ReadGoalImage(Global.SelectedUserRecord.GoalId);

                if (await LoadAllAsync())
                {
                    _refreshTime = DateTime.Now;
                    IsLoaded = true;
                }

            }
            
        }

        private async Task<bool> LoadAllAsync()
        {
            if (!await ServerApi.GetRecordAwesomeAsync(AwesomeUsers, Global.SelectedUserRecord.GoalRecordId))
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
                return false;
            }
            if (!await ServerApi.GetRecordCommentsAsync(Comments, Global.SelectedUserRecord.GoalRecordId))
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
                return false;
            }
            Debug.WriteLine("(评论的viewmodel)评论的条目数:{0}",Comments.Count);
            return true;
        }

        //private async Task LoadComments()
        //{
        //    if (!await ServerApi.GetRecordCommentsAsync(Comments, Global.SelectedUserRecord.GoalRecordId))
        //    {
        //        Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
        //    }
        //}

        //private async Task LoadAwesomes()
        //{
        //    if (!await ServerApi.GetRecordAwesomeAsync(AwesomeUsers, Global.SelectedUserRecord.GoalRecordId))
        //    {
        //        Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
