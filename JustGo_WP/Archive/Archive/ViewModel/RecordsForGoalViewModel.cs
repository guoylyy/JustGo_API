using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class RecordsForGoalViewModel
    {
        public ObservableCollection<UserRecord> Records { get; set; }

        public RecordsForGoalViewModel()
        {
            Records = new ObservableCollection<UserRecord>();

            LoadRecords();
        }

        private void LoadRecords()
        {
            Global.LoginUser = new User
            {
                UserName = "Micky",
                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head3.png", UriKind.Relative))
            };

            var record1 = new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = "今天终于开始执行我的健身计划了，已经向前迈进一大步了！",
                RecordTime = DateTime.Now,
                AllCommentsCount = 23,
                AwesomeUsers = new ObservableCollection<User>()
                {
                    //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
                    new User
                    {
                        UserName = "金大叔",
                        ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head1.png", UriKind.Relative))
                    },
                    new User
                    {
                        UserName = "金二叔",
                        ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head2.png", UriKind.Relative))
                    }
                },
                Comments = new ObservableCollection<Comment>
                {
                    new Comment
                    {
                        User =
                            new User
                            {
                                UserName = "小红",
                                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head3.png", UriKind.Relative))
                            },
                        CommentContent = "太厉害了！下次也带上我吧。"
                    },
                    new Comment
                    {
                        User =
                            new User
                            {
                                UserName = "小明",
                                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head4.png", UriKind.Relative))
                            },
                        CommentContent = "算上我一个！"
                    }
                }
            };
            record1.ViewAllRecordsString = string.Format("view all comments({0})", record1.AllCommentsCount);

            var record2 = new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = "刚完成100俯卧撑，累死了，但感觉很棒",
                RecordTime = DateTime.Now,
                AllCommentsCount = 31,
                AwesomeUsers = new ObservableCollection<User>()
                {
                    //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
                    new User
                    {
                        UserName = "金大叔",
                        ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head5.png", UriKind.Relative))
                    },
                    new User
                    {
                        UserName = "金二叔",
                        ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head6.png", UriKind.Relative))
                    }
                },
                Comments = new ObservableCollection<Comment>
                {
                    new Comment
                    {
                        User =
                            new User
                            {
                                UserName = "小华",
                                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head7.png", UriKind.Relative))
                            },
                        CommentContent = "点赞！"
                    },
                    new Comment
                    {
                        User =
                            new User
                            {
                                UserName = "小刚",
                                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head8.png", UriKind.Relative))
                            },
                        CommentContent = "我也刚完成！"
                    }
                }
            };
            record2.ViewAllRecordsString = string.Format("view all comments({0})", record2.AllCommentsCount);

            var record3 = new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = "刚完成100俯卧撑，累死了，但感觉很棒",
                RecordTime = DateTime.Now,
                AllCommentsCount = 31,
                AwesomeUsers = new ObservableCollection<User>()
                {
                    //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
                    new User
                    {
                        UserName = "金大叔",
                        ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head5.png", UriKind.Relative))
                    },
                    new User
                    {
                        UserName = "金二叔",
                        ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head6.png", UriKind.Relative))
                    }
                },
                Comments = new ObservableCollection<Comment>
                {
                    new Comment
                    {
                        User =
                            new User
                            {
                                UserName = "小华",
                                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head7.png", UriKind.Relative))
                            },
                        CommentContent = "点赞！"
                    },
                    new Comment
                    {
                        User =
                            new User
                            {
                                UserName = "小刚",
                                ImageSource = new BitmapImage(new Uri(@"/Assets/Heads/head8.png", UriKind.Relative))
                            },
                        CommentContent = "我也刚完成！"
                    }
                }
            };
            record3.ViewAllRecordsString = string.Format("view all comments({0})", record3.AllCommentsCount);

            Records.Add(record1);
            Records.Add(record2);
            Records.Add(record3);
        }
    }
}
