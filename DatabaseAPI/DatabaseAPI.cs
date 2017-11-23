using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlHelperY;

namespace DatabaseAPI
{
    public class DatabaseAPI
    {
        static MySqlHelper helper;
        static void Init(string str)
        {
            helper = new MySqlHelper(str);
        }
        static bool SignUp(string username, string password)
        {
            //注册
            
        }
        static bool SignIn(string username, string password)
        {
            //登录

        }
    }
}
