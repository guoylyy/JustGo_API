using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class FightingCenterViewModel
    {
        public ObservableCollection<UserRecord> Records { get; set; }
        public bool IsLoaded { get; private set; }

        public FightingCenterViewModel()
        {
            Records = new ObservableCollection<UserRecord>();
        }

        public async Task LoadData(bool isForce = false)
        {
            if (isForce || !IsLoaded)
            {
                Records.Clear();
                if (!await ServerApi.GetFightingCenter(Records, Global.LoginUser.Token))
                {
                    Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
                }
                else
                {
                    IsLoaded = true;
                }
            }
        }

        public async Task LoadOtherData(bool isForce = false)
        {
            if (isForce || !IsLoaded)
            {
                Records.Clear();
                if (!await ServerApi.GetOtherFightingCenter(Records, Global.LoginUser.Token, Global.SelectedUser.UserId))
                {
                    Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
                }
                else
                {
                    IsLoaded = true;
                }
            }
            
        }
    }
}
