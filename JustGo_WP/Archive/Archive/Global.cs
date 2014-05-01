using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archive.Datas;

namespace Archive
{
    public static class Global
    {
        public static Goal SelectedGoal { get; set; }
        public static User LoginUser { get; set; }
        public static string AddGoalType { get; set; }
        public static string AddGoalName { get; set; }
        public static string GoalParticipantString { get; set; }
        public static string Token { get; set; }
    }
}
