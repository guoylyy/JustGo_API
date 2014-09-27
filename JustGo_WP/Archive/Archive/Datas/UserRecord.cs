using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Datas
{
    public class UserRecord : INotifyPropertyChanged
    {
        private User _user;
        public User User
        {
            get { return _user; }

            set
            {
                if (value != _user)
                {
                    _user = value;
                    NotifyPropertyChanged("User");
                }
            }
        }

        private string _recordContent;
        public string RecordContent
        {
            get { return _recordContent; }

            set
            {
                if (value != _recordContent)
                {
                    _recordContent = value;
                    NotifyPropertyChanged("RecordContent");
                }
            }
        }

        private string _viewAllRecordsString;
        public string ViewAllRecordsString
        {
            get { return _viewAllRecordsString; }
            set
            {
                if (value != _viewAllRecordsString)
                {
                    _viewAllRecordsString = value;
                    NotifyPropertyChanged("ViewAllRecordsString");
                }
            }
        }

        private DateTime _recordTime;
        public DateTime RecordTime
        {
            get { return _recordTime; }

            set
            {
                if (value != _recordTime)
                {
                    _recordTime = value;
                    NotifyPropertyChanged("RecordTime");
                }
            }
        }

        private int _allAwesomeCount;
        public int AllAwesomeCount
        {
            get { return _allAwesomeCount; }
            set
            {
                if (value != _allAwesomeCount)
                {
                    _allAwesomeCount = value;
                    NotifyPropertyChanged("AllAwesomeCount");
                }
            }
        }

        private ObservableCollection<User> _awesomeUsers;
        public ObservableCollection<User> AwesomeUsers
        {
            get { return _awesomeUsers; }

            set
            {
                if (value != _awesomeUsers)
                {
                    _awesomeUsers = value;
                    NotifyPropertyChanged("AwesomeUsers");
                }
            }
        }

        private int _allCommentsCount;
        public int AllCommentsCount
        {
            get { return _allCommentsCount; }
            set
            {
                if (value != _allCommentsCount)
                {
                    _allCommentsCount = value;
                    NotifyPropertyChanged("AllCommentsCount");
                }
            }
        }

        private ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
        {
            get { return _comments; }

            set
            {
                if (value != _comments)
                {
                    _comments = value;
                    NotifyPropertyChanged("Comments");
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

        public string GoalId { get; set; }
        public string GoalRecordId { get; set; }

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
