using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archive.Datas;
using Archive.Pages;

namespace Archive
{
    public static class Global
    {
        /// <summary>
        /// 用户在已加入目标列表选择查看的目标
        /// </summary>
        public static GoalJoin SelectedGoal { get; set; }

        /// <summary>
        /// 登陆的用户
        /// </summary>
        public static User LoginUser { get; set; }


        public static string AddGoalType { get; set; }
        public static GoalJoin AddGoal { get; set; }
        public static string Token { get; set; }
    }
}
