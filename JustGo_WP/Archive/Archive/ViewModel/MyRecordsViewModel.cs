using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class MyRecordsViewModel
    {
        public ObservableCollection<UserRecord> Records { get; set; }

        public MyRecordsViewModel()
        {
            Records = new ObservableCollection<UserRecord>();

            Records.Add(new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = "今天又发射一个导弹，米国都吓cry了",
                RecordTime = DateTime.Now,
                AwesomeUsers = new ObservableCollection<User>()
                {
                    //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
                    new User{UserName = "金大叔",ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg",UriKind.Relative))},
                    new User{UserName = "金二叔",ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg",UriKind.Relative))}
                },
                Comments = new ObservableCollection<Comment>
                {
                    new Comment
                    {
                        User = new User{UserName = "金大伯",ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg",UriKind.Relative))},
                        CommentContent = "大金国崛起指日可待！"
                    },
                    new Comment
                    {
                        User = new User{UserName = "金二伯",ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg",UriKind.Relative))},
                        CommentContent = "立马就能干翻米国！"
                    }
                }
            });
        }
    }
}
