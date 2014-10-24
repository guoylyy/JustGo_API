using System;
using System.Windows;
using Archive.Datas;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Archive.ViewModel
{
    public class MyRecordsViewModel
    {
        public ObservableCollection<UserRecord> Records { get; set; }
        public bool IsLoaded { get; private set; }
        //private DateTime _refreshTime;

        public MyRecordsViewModel()
        {
            Records = new ObservableCollection<UserRecord>();
        }

        public async Task LoadRecord(bool isForce = false)
        {
            //|| DateTime.Now - _refreshTime > TimeSpan.FromMinutes(1)
            if (isForce || !IsLoaded)
            {
                await LoadData();
            }
        }

        private async Task LoadData()
        {
            Records.Clear();
            if (
                await
                    ServerApi.GetGoalJoinRecordAsync(Records, Global.SelectedGoalJoin.GoalId, Global.LoginUser.Token))
            {
                IsLoaded = true;
                //_refreshTime = DateTime.Now;
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
            }
        }

        public async Task<int> LoadRecordsCountAsync()
        {
            return await ServerApi.GetRecordsCountForGoalAsync(Global.SelectedGoalJoin.GoalId);
        }

        public async void AddRecord(UserRecord record)
        {
            var str = await
                ServerApi.PostRecordAsync(record.RecordContent, Global.SelectedGoalJoin.GoalId, Global.LoginUser.Token);
            if (string.IsNullOrEmpty(str))
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
            }
            else
            {
                record.GoalRecordId = str;
                Records.Insert(0, record);
            }
        }
    }
}
