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
                UserName = "金将军",
                ImageSource = new BitmapImage(new Uri(@"/Assets/MainPage/DefaulHead.jpg", UriKind.Relative))
            };

            var record1 = new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = "今天又发射一个导弹，米国都吓cry了",
                RecordTime = DateTime.Now,
                AllCommentsCount = 23,
                AwesomeUsers = new ObservableCollection<User>()
                {
                    //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
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
                },
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
                }
            };
            record1.ViewAllRecordsString = string.Format("view all comments({0})", record1.AllCommentsCount);

            var record2 = new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = "韩国船沉啦，果然冒牌货要招天谴啊",
                RecordTime = DateTime.Now,
                AllCommentsCount = 31,
                AwesomeUsers = new ObservableCollection<User>()
                {
                    //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
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
                },
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
                        CommentContent = "点赞！"
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
                        CommentContent = "翻得漂亮！"
                    }
                }
            };
            record2.ViewAllRecordsString = string.Format("view all comments({0})", record2.AllCommentsCount);

            var record3 = new UserRecord
            {
                User = Global.LoginUser,
                RecordContent = "韩国船沉啦，果然冒牌货要招天谴啊",
                RecordTime = DateTime.Now,
                AllCommentsCount = 31,
                AwesomeUsers = new ObservableCollection<User>()
                {
                    //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
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
                },
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
                        CommentContent = "点赞！"
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
                        CommentContent = "翻得漂亮！"
                    }
                }
            };
            record3.ViewAllRecordsString = string.Format("view all comments({0})", record3.AllCommentsCount);

            Records.Add(record1);
            //Records.Add(record2);
            //Records.Add(record3);
        }
    }
}
