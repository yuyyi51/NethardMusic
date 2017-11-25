using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAPI;
using NetworkAPI.NetworkMessage;
using DatabaseAPI;
namespace Server
{
    partial class ServerMain
    {
        static void AcceptConnectionConsole(TcpConnection con)
        {
            Console.Out.WriteLine(con.ToString() + " 已连接");
        }
        static void AcceptConnectionFunc(TcpConnection con)
        {
            con.ConnectionCloseEvent += (TcpConnection co) => Console.Out.WriteLine(co.ToString() + "已断开");
            con.LostConnectionEvent += (TcpConnection co) => Console.Out.WriteLine(co.ToString() + "已断开");
            AcceptMessage(con);
        }
        static async void AcceptMessage(TcpConnection con)
        {
            object obj = await con.ReceiveOnceAsync();
            NetMessage message = null;
            try
            {
                message = obj as NetMessage;
            }
            catch(Exception e)
            {
                Console.Out.WriteLine(e.Message);
                con.Close();
                return;
            }
            NetMessage returnMessage;
            switch(message.Message)
            {
                case NetMessage.MessageType.SignIn:
                    Console.Out.WriteLine();
                    returnMessage = await SignInAsync(message);
                    break;
                case NetMessage.MessageType.SignUp:
                    returnMessage = await SignUpAsync(message);
                    break;
                default:
                    returnMessage = new NetMessage(NetMessage.MessageType.Error);
                    break;
            }
            /*string[] d = message.Data as string[];
            Console.Out.WriteLine(d[0] + " " + d[1]);
            Console.Out.WriteLine("再次加密后 " + DatabaseAPI.DatabaseAPI.Encrypt(d[1]));
            NetMessage sus = new NetMessage(NetMessage.MessageType.Success);
            await con.Send(sus);*/
            con.Close();
        }
        static void Main(string[] args)
        {
            string host = "127.0.0.1";
            int port = 7777;
            string connectionstr = "server=localhost;user=root;database=test;port=3307;password=111qqq";
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
