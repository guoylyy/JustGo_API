using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public bool IsLoaded { get; private set; }

        public MyRecordsViewModel()
        {
            Records = new ObservableCollection<UserRecord>();

            //Records.Add(new UserRecord
            //{
            //    User = Global.LoginUser,
            //    RecordContent = "今天终于开始执行我的健身计划了，已经向前迈进一大步了！",
            //    RecordTime = DateTime.Now,
            //    AwesomeUsers = new ObservableCollection<User>()
            //    {
            //        //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
            //        new User{UserName = "金大叔",ImageSource = @"/Assets/Heads/head1.png"},
            //        new User{UserName = "金二叔",ImageSource = @"/Assets/Heads/head2.png"}
            //    },
            //    Comments = new ObservableCollection<Comment>
            //    {
            //        new Comment
            //        {
            //            User = new User{UserName = "小红",ImageSource = @"/Assets/Heads/head3.png"},
            //            CommentContent = "太厉害了！下次也带上我吧。"
            //        },
            //        new Comment
            //        {
            //            User = new User{UserName = "小明",ImageSource = @"/Assets/Heads/head4.png"},
            //            CommentContent = "算上我一个！"
            //        }
            //    }
            //});

            //Records.Add(new UserRecord
            //{
            //    User = Global.LoginUser,
            //    RecordContent = "药药切克闹！",
            //    RecordTime = DateTime.Now,
            //    AwesomeUsers = new ObservableCollection<User>()
            //    {
            //        //new BitmapImage(new Uri("/Assets/MainPage/DefaulHead",UriKind.Relative))
            //        new User{UserName = "金大叔",ImageSource = @"/Assets/Heads/head1.png"},
            //        new User{UserName = "金二叔",ImageSource = @"/Assets/Heads/head2.png"}
            //    },
            //    Comments = new ObservableCollection<Comment>
            //    {
            //        new Comment
            //        {
            //            User = new User{UserName = "小红",ImageSource = @"/Assets/Heads/head3.png"},
            //            CommentContent = "该吃药了！"
            //        },
            //        new Comment
            //        {
            //            User = new User{UserName = "小明",ImageSource = @"/Assets/Heads/head4.png"},
            //            CommentContent = "萌萌哒！"
            //        }
            //    }
            //});
        }

        public async void LoadRecord()
        {
            //todo; 可改为添加本地没有的record
            Records.Clear();

            await ServerApi.GetGoalJoinRecordAsync(Records, Global.SelectedGoalJoin.GoalId, Global.LoginUser.Token);
            IsLoaded = true;
        }

        public async void AddRecord(UserRecord record)
        {
            Records.Insert(0,record);
            Debug.WriteLine("上传record, token为:{0}",Global.LoginUser.Token);
            await
                ServerApi.PostRecordAsync(record.RecordContent, Global.SelectedGoalJoin.GoalId, Global.LoginUser.Token);
            Debug.WriteLine("上传record成功");
        }
    }
}
