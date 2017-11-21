using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAPI;
using NetworkAPI.NetworkMessage;
namespace Server
{
    class ServerMain
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
            NetMessage message = obj as NetMessage;
            string[] d = message.Data as string[];
            Console.Out.WriteLine(d[0] + " " + d[1]);
            NetMessage sus = new NetMessage(NetMessage.MessageType.Success);
            await con.Send(sus);
            con.Close();
        }
        static void Main(string[] args)
        {
            TcpServer server = new TcpServer("127.0.0.1", 7777);
            server.AcceptConnectionEvent += AcceptConnectionConsole;
            server.AcceptConnectionEvent += AcceptConnectionFunc;
            server.StartListeningAsync();
            string message = "";
            while((message = Console.In.ReadLine()) != null)
            {

            }
        }
    }
}
