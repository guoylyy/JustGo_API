using System.Linq;
using System.Windows;
using Archive.Datas;
using System.Collections.ObjectModel;

namespace Archive.ViewModel
{
    public class ExploreViewModel
    {
        public ObservableCollection<Goal> Explores { get; set; }
        public Goal TopExplore { get; set; }

        public bool HasLoadExplore { get; set; }

        public ExploreViewModel()
        {
            Explores = new ObservableCollection<Goal>();
            TopExplore = new Goal();
        }

        public async void LoadData()
        {
            if (!HasLoadExplore)
            {
                if (await ServerApi.GetExploreAsync(Explores, TopExplore))
                {
                    HasLoadExplore = true;
                }
                else
                {
                    Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
                }
            }
        }

        public void AddParticipants(string goalId, User user)
        {
            if (goalId == TopExplore.GoalId)
            {
                TopExplore.Participants++;
                if(!string.IsNullOrEmpty(user.Token))
                    TopExplore.JoinedUsers.Add(user);
            }
            else
            {
                var goal = Explores.FirstOrDefault(g => g.GoalId == goalId);
                if(goal != null)
                    goal.Participants++;
            }
        }
    }
}
