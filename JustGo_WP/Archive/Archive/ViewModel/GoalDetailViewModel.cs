using System.Windows.Documents;
using Archive.Datas;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Archive.ViewModel
{
    public class GoalDetailViewModel : INotifyPropertyChanged
    {
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                if (value != _imageUrl)
                {
                    _imageUrl = value;
                    NotifyPropertyChanged("ImageUrl");
                }
            }
        }

        private int _joins;
        public int Joins
        {
            get { return _joins; }
            set
            {
                if (value != _joins)
                {
                    _joins = value;
                    NotifyPropertyChanged("Joins");
                }
            }
        }

        private string _goalName;
        public string GoalName
        {
            get { return _goalName; }
            set
            {
                if (value != _goalName)
                {
                    _goalName = value;
                    NotifyPropertyChanged("GoalName");
                }
            }
        }

        public ObservableCollection<UserRecord> Records { get; set; }

        public ObservableCollection<User> Participants { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public GoalDetailViewModel()
        {
            Records = new ObservableCollection<UserRecord>();
            Participants = new ObservableCollection<User>();
        }

        public async Task<bool> LoadData()
        {
            Participants.Clear();
            Records.Clear();
            await Task.Delay(10);

            if (!await ServerApi.GetGoalDetailAsync(Records, Global.AddingGoalJoin.GoalId))
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
                return false;
            }

            StaticMethods.SaveGoalImage(Global.AddingGoalJoin.GoalId,ImageUrl);
            return true;
        }
    }
}
