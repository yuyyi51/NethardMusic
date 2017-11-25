using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAPI.NetworkMessage;
using NetworkAPI;
using System.Security.Cryptography;
namespace ClientAPI
{
    public class ClientAPI
    {
        protected static string salt = "IORGHTJBVFDNSKJFGRTKDSJMCVNDFJRHSFKDSGDT";
        protected static string ip = "127.0.0.1";
        protected static int port = 7777;
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
        public static async Task<bool> SignUp(string username, string password)
        {
            string md5pass = Encrypt(password);
            TcpConnection connection = new TcpConnection();
            await connection.Connect(ip, port);
            await connection.Send(NetMessage.GetSignUpMessage(username, md5pass));
            object obj = await connection.ReceiveOnceAsync();
            connection.Close();
            NetMessage message = obj as NetMessage;
            if(message.Message == NetMessage.MessageType.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<bool> SignIn(string username, string password)
        {
            string md5pass = Encrypt(password);
            TcpConnection connection = new TcpConnection();
            await connection.Connect(ip, port);
            await connection.Send(NetMessage.GetSignInMessage(username, md5pass));
            object obj = await connection.ReceiveOnceAsync();
            connection.Close();
            NetMessage message = obj as NetMessage;
            if (message.Message == NetMessage.MessageType.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
