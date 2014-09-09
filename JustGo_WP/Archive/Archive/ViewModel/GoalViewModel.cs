using System;
using System.Collections.ObjectModel;
using Archive.DataBase;
using Archive.Datas;
using GalaSoft.MvvmLight;

namespace Archive.ViewModel
{
    public class GoalViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ObservableCollection<GoalJoin> MyGoals { get; set; }

        public GoalViewModel()
        {
            MyGoals = new ObservableCollection<GoalJoin>();

            CsvUtil.ReadGoalJoin(MyGoals);
            foreach (var goalJoin in MyGoals)
            {
                goalJoin.GoalTracks = new ObservableCollection<GoalTrack>();
                CsvUtil.ReadGoalTrack(goalJoin.GoalTracks,goalJoin.GoalId);
                goalJoin.IsFinishedToday = goalJoin.GoalTracks.Count != 0
                                           && goalJoin.GoalTracks[0].TrackTime.Date == DateTime.Now.Date;
                goalJoin.PassedDays = goalJoin.GoalTracks.Count;
            }

            //MyGoals.Add(new GoalJoin { GoalName = "Run", IsFinishedToday = false, PassedDays = 5, TimeSpan = 20 });
            //MyGoals.Add(new GoalJoin { GoalName = "Learn English", IsFinishedToday = false, PassedDays = 10, TimeSpan = 15 });
            //MyGoals.Add(new GoalJoin { GoalName = "Drink Water", IsFinishedToday = true, PassedDays = 12, TimeSpan = 30 });
            //MyGoals.Add(new GoalJoin { GoalName = "Fight", IsFinishedToday = true, PassedDays = 3, TimeSpan = 20 });
            //MyGoals.Add(new GoalJoin { GoalName = "Fight", IsFinishedToday = true, PassedDays = 4, TimeSpan = 20 });
            //MyGoals.Add(new GoalJoin { GoalName = "Fight", IsFinishedToday = true, PassedDays = 1, TimeSpan = 20 });
            //MyGoals.Add(new GoalJoin { GoalName = "Fight", IsFinishedToday = true, PassedDays = 8, TimeSpan = 20 });
            //MyGoals.Add(new GoalJoin { GoalName = "Fight", IsFinishedToday = true, PassedDays = 5, TimeSpan = 20 });
        }

        //public void LoadData()
        //{

        //}
    }
}