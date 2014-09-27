using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class OtherFollowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _followPersons;
        public ObservableCollection<User> FollowPersons
        {
            get
            {
                return _followPersons;
            }
            private set
            {
                _followPersons = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<AlphaKeyGroup<User>> OtherGroupedFollow
        {
            get
            {
                return new ObservableCollection<AlphaKeyGroup<User>>
                    (AlphaKeyGroup<User>.CreateGroups(
                    FollowPersons,
                    (User s) => { return s.UserName; },
                    true));

            }
        }

        public OtherFollowViewModel()
        {
            FollowPersons = new ObservableCollection<User>();
        }

        public async Task LoadFollowers()
        {
            FollowPersons.Clear();
            await ServerApi.GetOtherFollowersAsync(Global.LoginUser.Token, FollowPersons,Global.SelectedUser.UserId);
            NotifyPropertyChanged("OtherGroupedFollow");
        }

        public async Task LoadFollowings()
        {
            FollowPersons.Clear();
            await ServerApi.GetOtherFollowingsAsync(Global.LoginUser.Token, FollowPersons,Global.SelectedUser.UserId);
            NotifyPropertyChanged("OtherGroupedFollow");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
