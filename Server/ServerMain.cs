using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAPI;
using NetworkAPI.NetworkMessage;
using static NetworkAPI.NetworkMessage.NetMessage;
namespace Server
{
    partial class ServerMain
    {
        static void AcceptConnectionConsole(TcpConnection con)
        {
            Console.Out.WriteLine(DateTime.Now.ToString() + " " + con.ToString() + " 已连接");
        }
        static void AcceptConnectionFunc(TcpConnection con)
        {
            con.ConnectionCloseEvent += (TcpConnection co) =>
            {
                if(co.Connected)
                    Console.Out.WriteLine(DateTime.Now.ToString() + " " + co.ToString() + " 已断开");
            };
            AcceptMessage(con);
        }
        static async void AcceptMessage(TcpConnection con)
        {
            object obj;
            try
            {
                obj = await con.ReceiveOnceAsync();
            }
            catch(Exception e)
            {
                //Console.Out.WriteLine("tag1");
                Console.Out.WriteLine(DateTime.Now.ToString() + " " + e.Message);
                con.Close();
                return;
            }
            if(obj == null)
            {
                Console.Out.WriteLine(DateTime.Now.ToString() + " " + "非指定程序的连接");
                con.Close();
                return;
            }
            NetMessage message = null;
            try
            {
                message = obj as NetMessage;
            }
            catch(Exception e)
            {
                Console.Out.WriteLine(DateTime.Now.ToString() + " " + e.Message);
                con.Close();
                return;
            }
            switch(message.Message)
            {
                case MessageType.SignIn:
                    SignIn(con, message);
                    break;
                case MessageType.SignUp:
                    SignUp(con, message);
                    break;
                case MessageType.GetList:
                    GetMusicList(con, message);
                    break;
                case MessageType.CheckMD5:
                    CheckMD5(con, message);
                    break;
                case MessageType.Upload:
                    AddMusic(con, message);
                    break;
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
            string host = "120.24.36.239";
            int port = 7777;
            string connectionstr = "server=localhost;user=root;database=test;port=3306;password=123sql";
            DatabaseAPI.API.Init(connectionstr);
            TcpServer server = new TcpServer(host, port);
            server.AcceptConnectionEvent += AcceptConnectionConsole;
            server.AcceptConnectionEvent += AcceptConnectionFunc;
            server.StartListeningAsync();
            string message = "";
            while((message = Console.In.ReadLine()) != null)
            {

            }
            server.StopListeningAsync();
            
        }
    }
}
