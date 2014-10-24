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
                CsvUtil.ReadGoalTrack(goalJoin.GoalTracks,goalJoin.GoalTracksId);
                goalJoin.IsFinishedToday = (goalJoin.GoalTracks.Count != 0
                                           && goalJoin.GoalTracks[0].TrackTime.Date == DateTime.Now.Date)
                                           || goalJoin.IsTodayPass
                                           || goalJoin.IsDone;
                goalJoin.PassedDays = goalJoin.GoalTracks.Count;
            }
        }

        public void RemoveGoalJoin(GoalJoin goalJoin)
        {
            MyGoals.Remove(goalJoin);
            CsvUtil.DeleteGoalTrack(goalJoin.GoalTracksId);
            CsvUtil.SaveGoalJoin(MyGoals);
        }
    }
}