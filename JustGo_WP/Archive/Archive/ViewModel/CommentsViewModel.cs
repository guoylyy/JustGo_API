using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public string UserName { get; set; }

        public CommentsViewModel()
        {
            Topic = Global.SelectedUserRecord.RecordContent;
            Time = Global.SelectedUserRecord.RecordTime;
            UserName = Global.SelectedUserRecord.User.UserName;

            AwesomeUsers = new ObservableCollection<User>();
            Comments = new ObservableCollection<Comment>();

            LoadAwesomes();
            LoadComments();
        }

        public async Task<string> Awesome()
        {
            return await ServerApi.PostRecordAwesomeAsync(Global.SelectedUserRecord.GoalRecordId,
                Global.LoginUser.Token);
        }

        private async void LoadComments()
        {
            await ServerApi.GetRecordCommentsAsync(Comments, Global.SelectedUserRecord.GoalRecordId);
        }

        private async void LoadAwesomes()
        {
            await ServerApi.GetRecordAwesomeAsync(AwesomeUsers, Global.SelectedUserRecord.GoalRecordId);
        }
    }
}
