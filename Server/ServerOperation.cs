using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAPI;
using NetworkAPI.NetworkMessage;
using static NetworkAPI.NetworkMessage.NetMessage;
using DatabaseAPI;
using System.Security.Cryptography;
using System.IO;
using System.Data;
namespace Server
{
    partial class ServerMain
    {
        private static string path = "C:\\Website\\music\\";
        private static string outpath = "http://int-2.cn/music/";
        private static void WriteLog(TcpConnection con, string message)
        {
            Console.Out.WriteLine(DateTime.Now.ToString() + " " + con.ToString() + " " + message);
        }
        private static void WriteLog(TcpConnection con, string message, object obj)
        {
            Console.Out.WriteLine(DateTime.Now.ToString() + " " + con.ToString() + " " + string.Format(message, obj));
        }
        private static void WriteLog(TcpConnection con, string message, params object[] objs)
        {
            Console.Out.WriteLine(DateTime.Now.ToString() + " " + con.ToString() + " " + string.Format(message,objs));
        }
        private static string GetMusicMD5(byte[] file)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(file);
            md5.Clear();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i != hash.Length; ++i)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            string result = builder.ToString();
            return result;
        }
        private static bool SaveFile(string filepath, byte[] file)
        {
            try
            {
                FileStream fs = File.Open(filepath, FileMode.CreateNew);
                fs.Write(file, 0, file.Length);
                fs.Flush();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static async void SignIn(TcpConnection con, NetMessage message)
        {
            string[] data = message.Data as string[];
            WriteLog(con, "{0} 尝试登录", data[0]);
            bool success = await API.SignInAsync(data[0], API.Encrypt(data[1]));
            NetMessage mess;
            if(success)
            {
                WriteLog(con, "{0} 登录成功", data[0]);
                mess = GetSuccessMessage();
            }
            else
            {
                WriteLog(con, "{0} 登录失败", data[0]);
                mess = GetFailMessage();
            }
            try
            {
                await con.Send(mess);
            }
            catch(Exception)
            {
                con.Close();
                return;
            }
            con.Close();
        }

        private static async void SignUp(TcpConnection con, NetMessage message)
        {
            string[] data = message.Data as string[];
            WriteLog(con, "{0} 尝试注册",data[0]);
            bool success = await API.SignUpAsync(data[0], API.Encrypt(data[1]));
            NetMessage mess;
            if (success)
            {
                WriteLog(con, "{0} 注册成功", data[0]);
                mess = GetSuccessMessage();
            }
            else
            {
                WriteLog(con, "{0} 注册失败", data[0]);
                mess = GetFailMessage();
            }
            try
            {
                await con.Send(mess);
            }
            catch(Exception)
            {
                con.Close();
                return;
            }
            con.Close();
        }

        private static async void CheckMD5(TcpConnection con, NetMessage message)
        {
            string md5 = message.Data as string;
            WriteLog(con, "尝试检查MD5值:{0}", md5);
            bool success = await API.CheckMD5Async(md5);
            NetMessage mess;
            if (!success)
            {
                WriteLog(con, "不存在MD5值:{0}", md5);
                mess = GetSuccessMessage();
            }
            else
            {
                WriteLog(con, "MD5值已存在:{0}", md5);
                mess = GetFailMessage();
            }
            try
            {
                await con.Send(mess);
            }
            catch (Exception)
            {
                con.Close();
                return;
            }
            con.Close();
        }

        private static async void AddMusic(TcpConnection con, NetMessage message)
        {
            //byte[] file = message.Data as byte[];
            MusicFile file = message.Data as MusicFile;
            string md5 = GetMusicMD5(file.file);
            WriteLog(con, "用户 {0} 尝试上传音乐 {1}-{2}{3}:{4}", file.uname, file.singer, file.name, file.suffix, md5);
            bool success = await API.AddMusicAsync(file.name, file.singer, file.suffix, md5, file.uname);
            NetMessage mess;
            if (success)
            {
                string filename = string.Format("{0}{1}{2}", path, md5, file.suffix);
                bool success2 = SaveFile(filename, file.file);
                if(success2)
                {
                    WriteLog(con, "上传音乐成功，保存为{0}", filename);
                    mess = GetSuccessMessage();
                }

                else
                {
                    WriteLog(con, "保存文件失败");
                    mess = GetFailMessage();
                }
            }
            else
            {
                WriteLog(con, "上传音乐失败");
                mess = GetFailMessage();
            }
            try
            {
                await con.Send(mess);
            }
            catch(Exception)
            {
                con.Close();
                return;
            }
            con.Close();
        }

        private static async void GetMusicList(TcpConnection con, NetMessage message)
        {
            WriteLog(con, "尝试获取音乐列表");
            DataSet set = await API.GetMusicList();
            NetMessage mess;
            if(set == null)
            {
                WriteLog(con, "获取音乐列表失败");
                mess = GetFailMessage();
            }
            else
            {
                WriteLog(con, "获取音乐列表成功");
                MusicInfo[] info = new MusicInfo[set.Tables[0].Rows.Count];
                for(int i = 0; i != set.Tables[0].Rows.Count; ++i)
                {
                    info[i] = new MusicInfo();
                    object[] objs = set.Tables[0].Rows[i].ItemArray;
                    info[i].name = objs[0] as string;
                    info[i].singer = objs[1] as string;
                    string url = outpath + objs[3] + objs[2];
                    info[i].url = url;
                    info[i].playedtimes = (int)objs[4];
                    info[i].uname = objs[5] as string;
                }
                mess = GetGetListSuccessMessage(info);
            }
            try
            {
                await con.Send(mess);
            }
            catch
            {
                con.Close();
                return;
            }
            con.Close();
        }
        private static async void MusicPlayed(TcpConnection con, NetMessage message)
        {
            string md5 = message.Data as string;
            WriteLog(con, "{0}被播放", md5);
            bool sus = await API.MusicPlayed(md5);
            if(sus)
            {
                WriteLog(con, "{0}增加播放量成功", md5);
            }
            else
            {
                WriteLog(con, "{0}增加播放量失败", md5);
            }
            con.Close();
        }
    }
}
