using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Datas
{
    public class Comment :INotifyPropertyChanged
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

        private string _commentContent;
        public string CommentContent
        {
            get { return _commentContent; }

            set
            {
                if (value != _commentContent)
                {
                    _commentContent = value;
                    NotifyPropertyChanged("CommentContent");
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
