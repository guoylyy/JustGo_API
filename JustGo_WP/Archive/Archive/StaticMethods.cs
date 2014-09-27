using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Archive.Datas;

namespace Archive
{
    public static class StaticMethods
    {
        private const string TokenName = "Token";
        private const string LoginUser = "LoginUser";

        /// <summary>
        /// 将token写入指定的文件中
        /// </summary>
        /// <param name="token">新的token字符串</param>
        public static void WriteToken(string token)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(TokenName))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(TokenName,token);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[TokenName] = token;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
        /// <summary>
        /// 读取已保存的token
        /// </summary>
        /// <returns>token字符串</returns>
        public static string ReadToken()
        {
            string token;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(TokenName, out token))
            {
                return token;
            }
            return string.Empty;
        }

        /// <summary>
        /// 删除本地的Token
        /// </summary>
        public static void DeleteToken()
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(TokenName);
        }

        public static void WriteUser(User user)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(LoginUser))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(LoginUser, user);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[LoginUser] = user;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static User ReadUser()
        {
            User loginUser;
            if(IsolatedStorageSettings.ApplicationSettings.TryGetValue(LoginUser,out loginUser))
            {
                return loginUser;
            }
            return null;
        }

        public static bool IsUserLogin()
        {
            return Global.LoginUser != null && Global.LoginUser.Token != null;
        }
    }
}
