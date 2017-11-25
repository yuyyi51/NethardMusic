using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAPI.NetworkMessage;
using DatabaseAPI;

namespace Server
{
    partial class ServerMain
    {
        private static async Task<NetMessage> SignInAsync(NetMessage message)
        {
            //登录
            string[] data = message.Data as string[];
            bool success = await API.SignInAsync(data[0], API.Encrypt(data[1]));
            if(success)
            {
                return NetMessage.GetSuccessMessage();
            }
            else
            {
                return NetMessage.GetFailMessage();
            }
        }
        private static async Task<NetMessage> SignUpAsync(NetMessage message)
        {
            //注册
            string[] data = message.Data as string[];
            bool success = await API.SignUpAsync(data[0], API.Encrypt(data[1]));
            if (success)
            {
                return NetMessage.GetSuccessMessage();
            }
            else
            {
                return NetMessage.GetFailMessage();
            }
    }
}
