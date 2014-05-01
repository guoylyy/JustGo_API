using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Archive
{
    public static class StaticMethods
    {
        private const string TokenFileName = "Token.txt";//token文件名

        /// <summary>
        /// 将token写入指定的文件中
        /// </summary>
        /// <param name="token">新的token字符串</param>
        public static async void WriteTokenAsync(string token)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //要写入Token的byte数据
                byte[] tokenDataBytes = Encoding.UTF8.GetBytes(token);

                //获取Token文件，不存在则新建一个
                var tokenFileStream = store.OpenFile(TokenFileName, FileMode.OpenOrCreate, FileAccess.Write);
                await tokenFileStream.WriteAsync(tokenDataBytes, 0, tokenDataBytes.Length);
                tokenFileStream.Close();
            }
        }
        /// <summary>
        /// 读取已保存的token
        /// </summary>
        /// <returns>token字符串</returns>
        public static async Task<string> ReadTokenAsync()
        {
            //using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    if (!store.FileExists(TokenFileName))
            //    {
            //        return "";
            //    }
            //    using (var tokenFileStream = store.OpenFile(TokenFileName, FileMode.Open, FileAccess.Read))
            //    {
            //        byte[] tokenBytes = new byte[(int)tokenFileStream.Length];
            //        await tokenFileStream.ReadAsync(tokenBytes, 0, tokenBytes.Length);
            //        return Encoding.UTF8.GetString(tokenBytes, 0, tokenBytes.Length);
            //    }
            //}


            StorageFile tokenFile = null;
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            byte[] tokenDataBytes;

            try
            {
                tokenFile = await localFolder.GetFileAsync(TokenFileName);

                using (Stream stream = await tokenFile.OpenStreamForReadAsync())
                {
                    tokenDataBytes = new byte[stream.Length];
                    await stream.ReadAsync(tokenDataBytes, 0, (int)stream.Length);
                    return Encoding.UTF8.GetString(tokenDataBytes, 0, tokenDataBytes.Length);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return "";
        }

        public static void DeleteToken()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(TokenFileName))
                {
                    store.DeleteFile(TokenFileName);
                }
            }
        }
    }
}
