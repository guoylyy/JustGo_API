using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Datas
{
    public class Goal : INotifyPropertyChanged
    {
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

        private int _totalDays;
        public int TotalDays
        {
            get { return _totalDays; }

            set
            {
                if (value != _totalDays)
                {
                    _totalDays = value;
                    NotifyPropertyChanged("TotalDays");
                }
            }
        }

        private int _passedDays;
        public int PassedDays
        {
            get { return _passedDays; }

            set
            {
                if (value != _passedDays)
                {
                    _passedDays = value;
                    NotifyPropertyChanged("PassedDays");
                    NotifyPropertyChanged("LeftDays");
                }
            }
        }

        private bool _isFinished;
        public bool IsFinished
        {
            get { return _isFinished; }

            set
            {
                if (value != _isFinished)
                {
                    _isFinished = value;
                    NotifyPropertyChanged("IsFinished");
                }
            }
        }

        public int LeftDays
        {
            get
            {
                return _totalDays - _passedDays;
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
