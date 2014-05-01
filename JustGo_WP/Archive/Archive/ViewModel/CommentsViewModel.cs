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
    public class CommentsViewModel
    {
        public ObservableCollection<User> AwesomeUsers { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        public CommentsViewModel()
        {
            AwesomeUsers = new ObservableCollection<User>
            {
                new User
                {
                    UserName = "金大叔",
                    ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg", UriKind.Relative))
                },
                new User
                {
                    UserName = "金二叔",
                    ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg", UriKind.Relative))
                }
            };


            Comments = new ObservableCollection<Comment>
            {
                new Comment
                {
                    User =
                        new User
                        {
                            UserName = "李野汉",
                            ImageSource =
                                new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg", UriKind.Relative))
                        },
                    CommentContent = "大金国崛起指日可待！"
                },
                new Comment
                {
                    User =
                        new User
                        {
                            UserName = "永动机",
                            ImageSource =
                                new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg", UriKind.Relative))
                        },
                    CommentContent = "立马就能干翻米国！"
                }
            };
        }
    }
}
