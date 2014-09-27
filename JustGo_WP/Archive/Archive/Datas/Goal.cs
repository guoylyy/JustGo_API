﻿using System;
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

        private int _participants;
        public int Participants
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

        private string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                if (value != _image)
                {
                    _image = value;
                    NotifyPropertyChanged("Image");
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
