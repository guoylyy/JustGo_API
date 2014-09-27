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
    public class SearchUsersViewModel
    {
        public ObservableCollection<User> SearchedUsers { get; set; }

        public SearchUsersViewModel()
        {
            SearchedUsers = new ObservableCollection<User>();
        }

        public async void LoadData(string key)
        {
            await ServerApi.GetSearchUserAsync(Global.LoginUser.Token, key, SearchedUsers);
        }
    }
}
