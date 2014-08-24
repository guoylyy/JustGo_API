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
    public class FollowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Person> _followPersons;
        public ObservableCollection<Person> FollowPersons
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

        public ObservableCollection<AlphaKeyGroup<Person>> GroupedFollow
        {
            get
            {
                return new ObservableCollection<AlphaKeyGroup<Person>>
                    (AlphaKeyGroup<Person>.CreateGroups(
                    FollowPersons,
                    (Person s) => { return s.Name; },
                    true));

            }
        }

        public FollowViewModel()
        {
            FollowPersons = new ObservableCollection<Person>();
            LoadData();
        }

        private void LoadData()
        {
            string[] CensusFemaleNames = { "嫣", "丹", "萌", "梦", "冉", "芬", "芳", "美", "妹", "洁" };
            string[] CensusMaleNames = { "伟", "壮", "刚", "强", "猛", "壕", "昊", "熊", "三", "斯", "严", "睿" };
            string[] CensusFamilyNames = { "陈", "赵", "王", "李", "钱", "孙", "苏", "常", "杨", "潘", "郭" };
            Random rnd = new Random(42);


            for (int i = 0; i < 50; i++)
            {
                var familyName = CensusFamilyNames[rnd.Next(CensusFamilyNames.Length - 1)];
                var name = rnd.Next(2) == 1
                    ? CensusFemaleNames[rnd.Next(CensusFemaleNames.Length - 1)]
                    : CensusMaleNames[rnd.Next(CensusMaleNames.Length - 1)];
                string fullName = familyName + name;

                FollowPersons.Add(new Person(fullName, "/Assets/MainPage/DefaulHead.jpg"));
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
