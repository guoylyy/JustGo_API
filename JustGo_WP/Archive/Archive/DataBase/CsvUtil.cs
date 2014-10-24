using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archive.Datas;

namespace Archive.DataBase
{ 
    public static class CsvUtil
    {
        private const string UserId = "abc";
        private const string GoalFileName = "Goals.csv";
        private const string GoalTrackFolder = "GoalTrack";

        public static void SaveGoalJoin(ObservableCollection<GoalJoin> datas)
        {
            using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fullPath = Path.Combine(UserId, GoalFileName);

                if (!myStore.DirectoryExists(UserId))
                {
                    myStore.CreateDirectory(UserId);
                }

                using (var stream = myStore.OpenFile(fullPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    string str = string.Empty;
                    foreach (var goal in datas)
                    {
                        str += goal.GoalId + ",";
                        str += goal.GoalName + ",";
                        str += goal.NeedReminder + ",";
                        str += goal.ReminderTime + ",";
                        str += goal.Frequency + ",";
                        str += goal.TimeSpan + ",";
                        str += goal.StartDate + ",";
                        str += goal.EndDate + ",";
                        str += goal.UpDateTime + Environment.NewLine;
                    }
                    var goalsByte = Encoding.UTF8.GetBytes(str);
                    stream.Write(goalsByte, 0, goalsByte.Length);
                }
            }
        }

        public static void ReadGoalJoin(ObservableCollection<GoalJoin> goals)
        {
            using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fullPath = Path.Combine(UserId, GoalFileName);

                if (!myStore.FileExists(fullPath))
                {
                    return;
                }

                using (var stream = myStore.OpenFile(fullPath, FileMode.Open, FileAccess.Read))
                {
                    var reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var words = line.Split(',');
                            var goal = new GoalJoin()
                            {
                                GoalId = words[0],
                                GoalName = words[1],
                                NeedReminder = bool.Parse(words[2]),
                                ReminderTime = Convert.ToDateTime(words[3]),
                                Frequency = words[4],
                                TimeSpan = int.Parse(words[5]),
                                StartDate = Convert.ToDateTime(words[6]),
                                EndDate = Convert.ToDateTime(words[7]),
                                UpDateTime = Convert.ToDateTime(words[8])
                            };

                            goals.Add(goal);
                        }
                    }
                }
            }
        }

        public static void SaveGoalTrack(ObservableCollection<GoalTrack> datas , string goalJoinId)
        {
            using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fullPath = Path.Combine(UserId, GoalTrackFolder, goalJoinId + ".csv");

                if (!myStore.DirectoryExists(UserId))
                {
                    myStore.CreateDirectory(UserId);
                }

                if (!myStore.DirectoryExists(Path.Combine(UserId, GoalTrackFolder)))
                {
                    myStore.CreateDirectory(Path.Combine(UserId, GoalTrackFolder));
                }

                using (var stream = myStore.OpenFile(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    string str = string.Empty;
                    foreach (var track in datas)
                    {
                        str += track.GoalTrackId + ",";
                        str += track.GoalJoinId + ",";
                        str += track.TrackTime + ",";
                        str += track.UpDateTime + Environment.NewLine;
                    }
                    var goalsByte = Encoding.UTF8.GetBytes(str);
                    stream.Write(goalsByte, 0, goalsByte.Length);
                }
            }
        }

        public static void ReadGoalTrack(ObservableCollection<GoalTrack> datas, string goalJoinId)
        {
            using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fullPath = Path.Combine(UserId, GoalTrackFolder, goalJoinId + ".csv");

                if (!myStore.FileExists(fullPath))
                {
                    return;
                }

                using (var stream = myStore.OpenFile(fullPath, FileMode.Open, FileAccess.Read))
                {
                    var reader = new StreamReader(stream);
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var words = line.Split(',');
                            var goal = new GoalTrack()
                            {
                                GoalTrackId = words[0],
                                GoalJoinId = words[1],
                                TrackTime = Convert.ToDateTime(words[2]),
                                UpDateTime = Convert.ToDateTime(words[3])
                            };

                            datas.Add(goal);
                        }
                    }
                }
            }
        }

        public static void DeleteGoalTrack(string goalJoinId)
        {
            using (var myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var fullPath = Path.Combine(UserId, GoalTrackFolder, goalJoinId + ".csv");

                if (!myStore.DirectoryExists(UserId))
                {
                    return;
                }
                if (!myStore.DirectoryExists(Path.Combine(UserId, GoalTrackFolder)))
                {
                    return;
                }

                myStore.DeleteFile(fullPath);
            }
        }
    }
}
