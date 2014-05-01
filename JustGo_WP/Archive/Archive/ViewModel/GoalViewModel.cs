using System.Collections.ObjectModel;
using Archive.Datas;
using GalaSoft.MvvmLight;

namespace Archive.ViewModel
{
    public class GoalViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ObservableCollection<Goal> MyGoals { get; set; }

        public GoalViewModel()
        {
            MyGoals = new ObservableCollection<Goal>();

            MyGoals.Add(new Goal { GoalName = "Run", IsFinished = false, PassedDays = 5, TotalDays = 20});
            MyGoals.Add(new Goal { GoalName = "Learn English", IsFinished = false, PassedDays = 3, TotalDays = 15 });
            MyGoals.Add(new Goal { GoalName = "Drink Water", IsFinished = true, PassedDays = 30, TotalDays = 30 });
            MyGoals.Add(new Goal { GoalName = "Fight", IsFinished = true, PassedDays = 8, TotalDays = 60 });
            MyGoals.Add(new Goal { GoalName = "Fight", IsFinished = true, PassedDays = 8, TotalDays = 60 });
            MyGoals.Add(new Goal { GoalName = "Fight", IsFinished = true, PassedDays = 8, TotalDays = 60 });
            MyGoals.Add(new Goal { GoalName = "Fight", IsFinished = true, PassedDays = 8, TotalDays = 60 });
            MyGoals.Add(new Goal { GoalName = "Fight", IsFinished = true, PassedDays = 8, TotalDays = 60 });
        }

        //public void LoadData()
        //{

        //}
    }
}