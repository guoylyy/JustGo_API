using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archive.Datas;

namespace Archive.ViewModel
{
    public class NotificationViewModel
    {
        public ObservableCollection<Notification> Notifications { get; set; }

        public NotificationViewModel()
        {
            Notifications = new ObservableCollection<Notification>();
            //LoadData();
        }

        public async void LoadData()
        {
            Notifications.Clear();
            //Notifications.Add(new Notification
            //{
            //    User=new User
            //    {
            //        ImageSource = "http://image.tjcsdc.com/user-header/4/0/4.150x163.jpe?_ts=20140919080439000000",
            //        UserName = "Sofea"
            //    },
            //    Content = "dafdirtjiaof",
            //    NotificationTime = DateTime.Now
            //});
            await ServerApi.GetNotificationAsync(Global.LoginUser.Token, Notifications);
        }
    }
}
