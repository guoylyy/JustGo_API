using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Archive.Datas
{
    public class User : SyncDataBase, INotifyPropertyChanged
    {
        public string UserId { get; set; }

        private string _userName;
        public string UserName
        {
            get { return _userName; }

            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    NotifyPropertyChanged("UserName");
                }
            }
        }

        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (value != _imageSource)
                {
                    _imageSource = value;
                    NotifyPropertyChanged("ImageSource");
                }
            }
        }

        private string _imageSourceMedium;
        public string ImageSourceMedium
        {
            get { return _imageSourceMedium; }
            set
            {
                if (value != _imageSourceMedium)
                {
                    _imageSourceMedium = value;
                    NotifyPropertyChanged("ImageSourceMedium");
                }
            }
        }

        private string _imageSourceSmall;
        public string ImageSourceSmall
        {
            get { return _imageSourceSmall; }
            set
            {
                if (value != _imageSourceSmall)
                {
                    _imageSourceSmall = value;
                    NotifyPropertyChanged("ImageSourceSmall");
                }
            }
        }

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

        private int _goalCount;
        public int GoalCount
        {
            get { return _goalCount; }
            set
            {
                if (value != _goalCount)
                {
                    _goalCount = value;
                    NotifyPropertyChanged("GoalCount");
                }
            }
        }

        private int _followerCount;
        public int FollowerCount
        {
            get { return _followerCount; }
            set
            {
                if (value != _followerCount)
                {
                    _followerCount = value;
                    NotifyPropertyChanged("FollowerCount");
                }
            }
        }

        private int _followingCount;
        public int FollowingCount
        {
            get { return _followingCount; }
            set
            {
                if (value != _followingCount)
                {
                    _followingCount = value;
                    NotifyPropertyChanged("FollowingCount");
                }
            }
        }

        /// <summary>
        /// 访问服务器的token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// facebook的id，作为注册的唯一标识
        /// </summary>
        public string FacebookId { get; set; }

        public bool CanFollow { get; set; }

        public void NotifyAllPropertyChanged()
        {
            NotifyPropertyChanged("UserName");
            NotifyPropertyChanged("ImageSource");
            NotifyPropertyChanged("ImageSourceMedium");
            NotifyPropertyChanged("ImageSourceSmall");
            NotifyPropertyChanged("Description");
            NotifyPropertyChanged("GoalCount");
            NotifyPropertyChanged("FollowerCount");
            NotifyPropertyChanged("FollowingCount");
        }

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
