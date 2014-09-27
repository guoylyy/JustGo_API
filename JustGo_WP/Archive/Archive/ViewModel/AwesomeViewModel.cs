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
    public class AwesomeViewModel
    {
        public ObservableCollection<User> AwesomeUsers { get; set; }

        public AwesomeViewModel()
        {
            AwesomeUsers = new ObservableCollection<User>
            {
                new User
                {
                    GoalCount = 2,
                    ImageSource = @"/Assets/Heads/head1.png",
                    UserName = "小红"
                },
                new User
                {
                    GoalCount = 12,
                    ImageSource = @"/Assets/Heads/head2.png",
                    UserName = "小华"
                },
                new User
                {
                    GoalCount = 23,
                    ImageSource = @"/Assets/Heads/head3.png",
                    UserName = "小丽"
                },
                new User
                {
                    GoalCount = 8,
                    ImageSource = @"/Assets/Heads/head4.png",
                    UserName = "小彤"
                }
            };
        }
    }
}
