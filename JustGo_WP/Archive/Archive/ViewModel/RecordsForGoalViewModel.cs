using Archive.Datas;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Archive.ViewModel
{
    public class RecordsForGoalViewModel
    {
        public ObservableCollection<UserRecord> Records { get; set; }
        public bool IsLoaded { get; private set; }

        public RecordsForGoalViewModel()
        {
            Records = new ObservableCollection<UserRecord>();
        }

        public async Task<bool> LoadRecords(bool isForce = false)
        {
            await Task.Delay(100);
            if (isForce || !IsLoaded)
            {
                Records.Clear();
                if (!await ServerApi.GetAllRecordsForGoalAsync(Records, Global.SelectedGoalJoin.GoalId))
                {
                    Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
                    return false;
                }
                IsLoaded = true;
            }
            return true;
        }
    }
}
