using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class FightingCenterViewModel
    {
        public ObservableCollection<UserRecord> Records { get; set; }

        public FightingCenterViewModel()
        {
            Records = new ObservableCollection<UserRecord>();
        }

        public async void LoadData()
        {
            Records.Clear();
            await ServerApi.GetFightingCenter(Records, Global.LoginUser.Token);
        }

        public async void LoadOtherData()
        {
            Records.Clear();
            await ServerApi.GetOtherFightingCenter(Records, Global.LoginUser.Token, Global.SelectedUser.UserId);
        }
    }
}
