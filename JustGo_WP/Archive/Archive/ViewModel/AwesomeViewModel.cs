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
                    ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head1.png",UriKind.Relative)),
                    UserName = "小红"
                },
                new User
                {
                    GoalCount = 12,
                    ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head2.png",UriKind.Relative)),
                    UserName = "小华"
                },
                new User
                {
                    GoalCount = 23,
                    ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head3.png",UriKind.Relative)),
                    UserName = "小丽"
                },
                new User
                {
                    GoalCount = 8,
                    ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head4.png",UriKind.Relative)),
                    UserName = "小彤"
                }
            };
        }
    }
}
