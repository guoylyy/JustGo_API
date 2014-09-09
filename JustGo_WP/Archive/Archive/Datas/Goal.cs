using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Datas
{
    public class Goal : INotifyPropertyChanged
    {
        public string GoalId { get; set; }
        public string GoalCategory { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime UpDateTime { get; set; }

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

        private string _participants;
        public string Participants
        {
            get { return _participants; }

            set
            {
                if (value != _participants)
                {
                    _participants = value;
                    NotifyPropertyChanged("Participants");
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
