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
        public string Topic { get; set; }
        public DateTime Time { get; set; }

        public CommentsViewModel()
        {
            Topic = "今天终于开始执行我的健身计划了，已经向前迈进一大步了！";
            Time = DateTime.Now;

            AwesomeUsers = new ObservableCollection<User>
            {
                new User
                {
                    UserName = "小红",
                    ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head1.png", UriKind.Relative))
                },
                new User
                {
                    UserName = "小明",
                    ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head2.png", UriKind.Relative))
                }
            };


            Comments = new ObservableCollection<Comment>
            {
                new Comment
                {
                    User =
                        new User
                        {
                            UserName = "小红",
                            ImageSource =
                                new BitmapImage(new Uri(@"/Assets/Heads/head3.png", UriKind.Relative))
                        },
                    CommentContent = "太厉害了！下次也带上我吧。"
                },
                new Comment
                {
                    User =
                        new User
                        {
                            UserName = "小明",
                            ImageSource =
                                new BitmapImage(new Uri(@"/Assets/Heads/head4.png", UriKind.Relative))
                        },
                    CommentContent = "算上我一个！"
                }
            };
        }
    }
}
