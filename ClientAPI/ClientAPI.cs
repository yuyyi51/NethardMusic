using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAPI.NetworkMessage;
using static NetworkAPI.NetworkMessage.NetMessage;
using NetworkAPI;
using System.Security.Cryptography;
using System.IO;
namespace ClientAPI
{
    public class ClientAPI
    {
        protected static string salt = "IORGHTJBVFDNSKJFGRTKDSJMCVNDFJRHSFKDSGDT";
        protected static string ip = "120.24.36.239";
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
            await connection.Send(GetSignUpMessage(username, md5pass));
            object obj = await connection.ReceiveOnceAsync();
            NetMessage message = obj as NetMessage;
            connection.Close();
            if(message.Message == MessageType.Success)
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
            await connection.Send(GetSignInMessage(username, md5pass));
            object obj = await connection.ReceiveOnceAsync();
            connection.Close();
            NetMessage message = obj as NetMessage;
            if (message.Message == MessageType.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<bool> CheckMD5(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            TcpConnection con = new TcpConnection();
            await con.Connect(ip, port);
            await con.Send(GetCheckMD5Message(sb.ToString()));
            object obj = await con.ReceiveOnceAsync();
            con.Close();
            NetMessage message = obj as NetMessage;
            if(message.Message == MessageType.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static byte[] GetFileByte(string url)
        {
            FileStream fs = File.Open(url, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            fs.Close();
            return buffer;
        }
        public static async Task<bool> UploadMusic(string path, string name, string singer, string uname)
        {
            MusicFile file = new MusicFile();
            file.name = name;
            file.singer = singer;
            file.suffix = Path.GetExtension(path);
            file.file = GetFileByte(path);
            file.uname = uname;
            //file.file = null;
            TcpConnection con = new TcpConnection();
            await con.Connect(ip, port);
            await con.Send(GetUploadMessage(file));
            object obj = await con.ReceiveOnceAsync();
            con.Close();
            NetMessage message = obj as NetMessage;
            if(message.Message == MessageType.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task<MusicInfo[]> GetMusicList()
        {
            TcpConnection con = new TcpConnection();
            await con.Connect(ip, port);
            await con.Send(GetGetListMessage());
            NetMessage message = await con.ReceiveOnceAsync() as NetMessage;
            con.Close();
            if(message.Message == MessageType.GetListSuccess)
            {
                return message.Data as MusicInfo[];
            }
            else
            {
                return null;
            }
        }
        public static async Task<MusicInfo[]> GetFavoriteList(string uname)
        {
            TcpConnection con = new TcpConnection();
            await con.Connect(ip, port);
            await con.Send(GetGetFavoriteMessage(uname));
            NetMessage message = await con.ReceiveOnceAsync() as NetMessage;
            con.Close();
            if (message.Message == MessageType.GetListSuccess)
            {
                return message.Data as MusicInfo[];
            }
            else
            {
                return null;
            }
        }
        public static async Task MusicPlayed(string url)
        {
            string md5 = GetMD5FromUrl(url);
            TcpConnection con = new TcpConnection();
            await con.Connect(ip, port);
            await con.Send(GetMusicPlayedMessage(md5));
            //Console.Out.WriteLine(md5);
            con.Close();
        }
        public static string GetMD5FromUrl(string url)
        {
            string md5;
            string[] tem = url.Split('/');
            string tem2 = tem[tem.Length - 1];
            md5 = tem2.Split('.')[0];
            return md5;
        }
        public static async Task<bool> AddFavorite(string uname, string url)
        {
            string md5 = GetMD5FromUrl(url);
            TcpConnection con = new TcpConnection();
            await con.Connect(ip, port);
            await con.Send(GetAddFavoriteMessage(uname, md5));
            NetMessage mess = await con.ReceiveOnceAsync() as NetMessage;
            con.Close();
            if(mess.Message == MessageType.Success)
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
