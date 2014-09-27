using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Datas
{
    public class GoalTrack : SyncDataBase, INotifyPropertyChanged
    {
        public string GoalTrackId { get; set; }

        public string GoalJoinId { get; set; }

        private DateTime _trackTime;
        public DateTime TrackTime
        {
            get { return _trackTime; }
            set
            {
                if (value != _trackTime)
                {
                    _trackTime = value;
                    NotifyPropertyChanged("TrackTime");
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
