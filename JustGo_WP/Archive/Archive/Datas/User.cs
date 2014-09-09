using System;
using System.Collections.Generic;
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

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
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

        /// <summary>
        /// 访问服务器的token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 访问facebook的token
        /// </summary>
        public string FacebookToken { get; set; }

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
