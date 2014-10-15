using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.Datas
{
    public class GoalJoin : SyncDataBase, INotifyPropertyChanged
    {
        /// <summary>
        /// [DB] 用户加入的目标ID
        /// </summary>
        public string GoalId { get; set; }

        private string _goalName;
        /// <summary>
        /// [DB + Bind] 用户加入的目标名称
        /// </summary>
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

        private int _timeSpan;
        /// <summary>
        /// [DB + Bind] 目标总的持续天数
        /// </summary>
        public int TimeSpan
        {
            get { return _timeSpan; }
            set
            {
                if (value != _timeSpan)
                {
                    _timeSpan = value;
                    NotifyPropertyChanged("TimeSpan");
                }
            }
        }

        private int _passedDays;
        /// <summary>
        /// [Bind] 用户已经完成的天数
        /// </summary>
        public int PassedDays
        {
            //get { return GoalTracks != null? GoalTracks.Count : 0; }

            get { return _passedDays; }
            set
            {
                if (value != _passedDays)
                {
                    _passedDays = value;
                    NotifyPropertyChanged("PassedDays");
                    NotifyPropertyChanged("LeftDays");
                    NotifyPropertyChanged("IsDone");
                }
            }
        }

        private bool _isFinishedToday;
        /// <summary>
        /// 用户当天是否完成任务
        /// </summary>
        public bool IsFinishedToday
        {
            //get { return GoalTracks != null 
            //    && GoalTracks.Count != 0 
            //    && GoalTracks[0].TrackTime.Date == DateTime.Now.Date; }

            get { return _isFinishedToday; }
            set
            {
                if (value != _isFinishedToday)
                {
                    _isFinishedToday = value;
                    NotifyPropertyChanged("IsFinishedToday");
                }
            }
        }

        public int Participants { get; set; }
        //private int _participants;
        //public int Participants
        //{
        //    get { return _participants; }
        //    set
        //    {
        //        if (value != _participants)
        //        {
        //            _participants = value;
        //            NotifyPropertyChanged("Participants");
        //        }
        //    }
        //}

        /// <summary>
        /// [Bind] 用户所加目标剩余的天数
        /// </summary>
        public int LeftDays
        {
            get
            {
                return TimeSpan - PassedDays;
            }
        }

        /// <summary>
        /// [DB] 目标开始的时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// [DB] 目标结束的时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// [DB] 提醒的频率
        /// </summary>
        public string Frequency { get; set; }

        /// <summary>
        /// [DB] 是否开启提醒
        /// </summary>
        public bool NeedReminder { get; set; }

        /// <summary>
        /// [DB] 目标提醒的时间
        /// </summary>
        public DateTime? ReminderTime { get; set; }

        /// <summary>
        /// 每个Goal对应的记录文件名
        /// </summary>
        public string GoalTracksId
        {
            get { return GoalId + "&" + StartDate.ToString("s").Replace(':',' '); }
        }

        /// <summary>
        /// 判断目标是否已经结束
        /// </summary>
        public bool IsDone 
        {
            get { return LeftDays == 0; } 
        }

        public bool IsTodayPass
        {
            get
            {
                bool value = false;
                var frequency = Frequency.Split(';');
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        value = frequency[0] == "0";
                        break;
                    case DayOfWeek.Monday:
                        value = frequency[1] == "0";
                        break;
                    case DayOfWeek.Tuesday:
                        value = frequency[2] == "0";
                        break;
                    case DayOfWeek.Wednesday:
                        value = frequency[3] == "0";
                        break;
                    case DayOfWeek.Thursday:
                        value = frequency[4] == "0";
                        break;
                    case DayOfWeek.Friday:
                        value = frequency[5] == "0";
                        break;
                    case DayOfWeek.Saturday:
                        value = frequency[6] == "0";
                        break;
                }
                return value;
            }
        }

        /// <summary>
        /// 每个Goal对应的记录
        /// </summary>
        public ObservableCollection<GoalTrack> GoalTracks { get; set; }

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
