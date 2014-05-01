using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Archive.Datas
{
    public class User:INotifyPropertyChanged
    {
        private string _userId;
        public string UserId
        {
            get { return _userId; }

            set
            {
                if (value != _userId)
                {
                    _userId = value;
                    NotifyPropertyChanged("UserId");
                }
            }
        }

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
