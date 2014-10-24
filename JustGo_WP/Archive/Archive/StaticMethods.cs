using System;
using System.Linq;
using Archive.Datas;
using Archive.ViewModel;
using Coding4Fun.Toolkit.Controls;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;

namespace Archive
{
    public static class StaticMethods
    {
        private const string LoginUser = "LoginUser";
        private static string _loginUserId;
        private const string Notification = "Notification";

        public static void WriteUser(User user)
        {
            _loginUserId = user.FacebookId;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(_loginUserId))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(_loginUserId, user);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[_loginUserId] = user;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();

            WriteUserId();
        }

        public static User ReadUser()
        {
            if (ReadUserId())
            {
                User loginUser;
                if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(_loginUserId, out loginUser))
                {
                    return loginUser;
                }
            }

            return null;
        }

        public static bool IsUserLogin()
        {
            return Global.LoginUser != null && Global.LoginUser.Token != null;
        }

        private static void WriteUserId()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(LoginUser))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(LoginUser, _loginUserId);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[LoginUser] = _loginUserId;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private static bool ReadUserId()
        {
            return IsolatedStorageSettings.ApplicationSettings.TryGetValue(LoginUser, out _loginUserId);
        }

        public static void DeleteUserId()
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(LoginUser);
            IsolatedStorageSettings.ApplicationSettings.Save();
        }


        public static void SaveGoalImage(string goalId, string imageUrl)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(goalId))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(goalId, imageUrl);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[goalId] = imageUrl;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static string ReadGoalImage(string goalId)
        {
            string goalUrl;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(goalId, out goalUrl))
            {
                return goalUrl;
            }
            return string.Empty;
        }

        public static bool IsNetworkEnable()
        {
            return DeviceNetworkInformation.IsWiFiEnabled
                   || DeviceNetworkInformation.IsCellularDataEnabled
                   || DeviceNetworkInformation.IsNetworkAvailable;
        }

        public static void ShowRequestFailedToast()
        {
            var toast = new ToastPrompt
            {
                Message = "Network request failed",
                MillisecondsUntilHidden = 1000,
                Margin = new Thickness(0, -40, 0, 0)
            };
            toast.Show();
        }

        public static void ShowToast(string message)
        {
            var toast = new ToastPrompt
            {
                Message = message,
                MillisecondsUntilHidden = 1000,
                Margin = new Thickness(0, -40, 0, 0)
            };
            toast.Show();
        }

        public static void UpdateTile()
        {
            var goal = ViewModelLocator.GoalViewModel.MyGoals.FirstOrDefault(g => !g.IsFinishedToday);
            var tile = ShellTile.ActiveTiles.First();
            ShellTileData tileData;

            if (goal == null)
            {
                tileData = new FlipTileData
                {
                    Title = "Insist",
                    BackgroundImage = new Uri("/Assets/icon_336.png", UriKind.Relative),
                    BackTitle = "Insist",
                    BackContent = "You have finished all goals today"
                };
            }
            else
            {
                tileData = new FlipTileData
                {
                    BackgroundImage = new Uri("/Assets/icon_336.png", UriKind.Relative),
                    Title = "Insist",
                    BackTitle = "Insist",
                    BackContent = "Remember your goals today" + Environment.NewLine + goal.GoalName,
                };
            }

            tile.Update(tileData);
        }

        public static void ChangeNotificationSetting(bool value)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(Notification))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(Notification, value);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[Notification] = value;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static bool ReadNotificationSetting()
        {
            bool value;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(Notification, out value))
            {
                return value;
            }
            return true;
        }

        public static async void SendAllGoalJoin()
        {
            foreach (var goalJoin in ViewModelLocator.GoalViewModel.MyGoals)
            {
                await ServerApi.PostNewJoinAsync(Global.LoginUser.Token, goalJoin.GoalId);
            }
            ViewModelLocator.ExploreViewModel.LoadData(true);
        }
    }
}
