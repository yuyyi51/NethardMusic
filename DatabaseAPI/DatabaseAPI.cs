using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlHelperY;
using System.Security.Cryptography;
using System.Data;

namespace DatabaseAPI
{
    public class API
    {
        protected static MySqlHelper helper;
        protected static string salt = "QPOWIRUHRTJA3TU5JDVKMCJHBIJ3OR4HFE5T";
        public static void Init(string str)
        {
            helper = new MySqlHelper(str);
        }
        public static string Encrypt(string password)
        {
            password = salt + password + salt;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] value = Encoding.UTF8.GetBytes(password);
            byte[] hash = md5.ComputeHash(value);
            md5.Clear();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i != hash.Length; ++i)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            string md5pass = builder.ToString();
            return md5pass;
        }
        public static async Task<bool> SignUpAsync(string username, string password)
        {
            //注册
            string command = string.Format("select count(*) from user where username = '{0}';",username);
            object obj = await helper.ExecuteScalarAsync(command);
            long count = (long)obj;
            if(count > 0)
            {
                return false;
            }
            command = string.Format("insert into user(username,`password`) value('{0}','{1}');", username, password);
            try
            {
                count = await helper.ExecuteNonQueryAsync(command);
            }
            catch(Exception)
            {
                return false ;
            }
            return count > 0;
        }
        public static async Task<bool> SignInAsync(string username, string password)
        {
            //登录
            string command = string.Format("select count(*) from user where username = '{0}' and `password` = '{1}';", username, password);
            object obj = await helper.ExecuteScalarAsync(command);
            //Console.Out.WriteLine( obj.GetType());
            long count = (long)obj;
            return count > 0;
        }
        public static async Task<bool> CheckMD5Async(string md5)
        {
            //检查md5
            string command = string.Format("select count(*) from u_music where MD5 = '{0}';", md5);
            object obj = await helper.ExecuteScalarAsync(command);
            long count = (long)obj;
            return count > 0;
        }
        public static async Task<bool> AddMusicAsync(string name, string singer, string suffix, string md5, string uname = "")
        {
            //加入歌曲
            bool exist = await CheckMD5Async(md5);
            if (exist)
                return false;
            string command = string.Format("insert into u_music value('{0}','{1}','{2}','{3}');", name, singer, suffix, md5);
            bool result = false;
            try
            {
                int t1 = await helper.ExecuteNonQueryAsync(command);
                command = string.Format("insert into upload value('{0}','{1}');", uname, md5);
                int t2 = await helper.ExecuteNonQueryAsync(command);
                result = (t1 > 0) && (t2 > 0);
            }
            catch(Exception)
            {
                return false;
            }
            return result;
        }
        public static async Task<bool> AddFavoriteAsync(string uname, string md5)
        {
            string command = string.Format("insert into favorite value('{0}','{1}');", uname, md5);
            int re;
            try
            {
                re = await helper.ExecuteNonQueryAsync(command);
            }
            catch(Exception)
            {
                return false;
            }
            return re > 0;
        }
        public static async Task<DataSet> GetMusicList()
        {
            string command = "select * from u_music left join upload on u_music.MD5 = upload.MD5; ";
            try
            {
                return await helper.ExecuteDataSetAsync(command);
            }
            catch(Exception)
            {
                return null;
            }
        }
        public static async Task<bool> MusicPlayed(string md5)
        {
            string command = string.Format("update u_music set playedtimes = playedtimes + 1 where MD5 = '{0}' ;", md5);
            try
            {
                return await helper.ExecuteNonQueryAsync(command) > 0;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public static async Task<DataSet> GetFavoriteList(string uname)
        {
            string command = string.Format("select * from (select m_name,m_singer,m_suffix,u_music.MD5,playedtimes from u_music inner join (select * from favorite where username = '{0}') as tfavorite on u_music.MD5 = tfavorite.MD5) as fav left join upload on fav.MD5 = upload.MD5;", uname);
            try
            {
                return await helper.ExecuteDataSetAsync(command);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
