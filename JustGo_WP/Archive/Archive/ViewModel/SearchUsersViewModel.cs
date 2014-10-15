using Archive.Datas;
using System.Collections.ObjectModel;
using System.Windows;

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
            if (!await ServerApi.GetSearchUserAsync(Global.LoginUser.Token, key, SearchedUsers))
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
            }
        }
    }
}
