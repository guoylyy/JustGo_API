using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class FollowViewModel : INotifyPropertyChanged
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

        public ObservableCollection<AlphaKeyGroup<User>> GroupedFollow
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

        public FollowViewModel()
        {
            FollowPersons = new ObservableCollection<User>();
        }

        //public void LoadData()
        //{
        //    string[] CensusFemaleNames = { "嫣", "丹", "萌", "梦", "冉", "芬", "芳", "美", "妹", "洁" };
        //    string[] CensusMaleNames = { "伟", "壮", "刚", "强", "猛", "壕", "昊", "熊", "三", "斯", "严", "睿" };
        //    string[] CensusFamilyNames = { "陈", "赵", "王", "李", "钱", "孙", "苏", "常", "杨", "潘", "郭" };
        //    Random rnd = new Random(42);


        //    for (int i = 0; i < 50; i++)
        //    {
        //        var familyName = CensusFamilyNames[rnd.Next(CensusFamilyNames.Length - 1)];
        //        var name = rnd.Next(2) == 1
        //            ? CensusFemaleNames[rnd.Next(CensusFemaleNames.Length - 1)]
        //            : CensusMaleNames[rnd.Next(CensusMaleNames.Length - 1)];
        //        string fullName = familyName + name;

        //        FollowPersons.Add(new Person(fullName, "http://image.tjcsdc.com/user-header/4/0/4.150x163.jpe?_ts=20140919080439000000"));
        //    }

        //}

        public async Task LoadFollowers()
        {
            FollowPersons.Clear();
            if (await ServerApi.GetUserFollowersAsync(Global.LoginUser.Token, FollowPersons))
            {
                NotifyPropertyChanged("GroupedFollow");
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
            }
            
        }

        public async Task LoadFollowings()
        {
            FollowPersons.Clear();
            if (await ServerApi.GetUserFollowingsAsync(Global.LoginUser.Token, FollowPersons))
            {
                NotifyPropertyChanged("GroupedFollow");
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(StaticMethods.ShowRequestFailedToast);
            }
            
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
