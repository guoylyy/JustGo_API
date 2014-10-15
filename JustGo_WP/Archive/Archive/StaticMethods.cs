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
            if (goal == null) return;

            var tile = ShellTile.ActiveTiles.First();
            var data = new FlipTileData
            {
                BackgroundImage = new Uri("/Assets/icon_336.png", UriKind.Relative),
                Title = "Insist",
                BackTitle = "Insist",
                BackContent = goal.GoalName,
            };
            tile.Update(data);
        }
    }
}
