using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlHelperY;
using System.Security.Cryptography;

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
            string command = string.Format("select count(*) from user where username = '{0}'; ",username);
            int count = await helper.ExecuteNonQueryAsync(command);
            if(count > 0)
            {
                return false;
            }
            command = string.Format("insert into user(username,password)value({0},{1});", username, password);
            count = await helper.ExecuteNonQueryAsync(command);
            if (count == 0)
                return false;
            return true;
        }
        public static async Task<bool> SignInAsync(string username, string password)
        {
            //登录
            string command = string.Format("select count(*) from user where username = '{0}' and password = '{1}'; ", username, password);
            int count = await helper.ExecuteNonQueryAsync(command);
            return count > 0;
        }
    }
}
