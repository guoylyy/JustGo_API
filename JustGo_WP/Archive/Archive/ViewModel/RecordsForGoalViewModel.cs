using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class RecordsForGoalViewModel
    {
        public ObservableCollection<UserRecord> Records { get; set; }

        public RecordsForGoalViewModel()
        {
            Records = new ObservableCollection<UserRecord>();

            LoadRecords();
        }

        private async void LoadRecords()
        {
            await ServerApi.GetAllRecordsForGoalAsync(Records, Global.SelectedGoalJoin.GoalId);

        }
    }
}
