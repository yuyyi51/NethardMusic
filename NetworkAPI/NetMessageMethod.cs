using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAPI
{
    namespace NetworkMessage
    {
        public partial class NetMessage
        {
            public static NetMessage GetSignInMessage(string username, string md5password)
            {
                string[] strs = { username, md5password };
                return new NetMessage(MessageType.SignIn, strs);
            }
            public static NetMessage GetSignUpMessage(string username, string md5password)
            {
                string[] strs = { username, md5password };
                return new NetMessage(MessageType.SignUp, strs);
            }
            public static NetMessage GetCheckMD5Message(string md5)
            {
                return new NetMessage(MessageType.CheckMD5, md5);
            }
            public static NetMessage GetUploadMessage(MusicFile file)
            {
                return new NetMessage(MessageType.Upload, file);
            }
            public static NetMessage GetGetListMessage()
            {
                return new NetMessage(MessageType.GetList);
            }
            public static NetMessage GetGetListSuccessMessage(MusicInfo[] info)
            {
                return new NetMessage(MessageType.GetListSuccess, info);
            }
            public static NetMessage GetSignInFailMessage()
            {
                return new NetMessage(MessageType.SignInFail);
            }
            public static NetMessage GetSignUpFailMessage()
            {
                return new NetMessage(MessageType.SignUpFail);
            }
            public static NetMessage GetCheckMD5FailMessage()
            {
                return new NetMessage(MessageType.CheckMD5Fail);
            }
            public static NetMessage GetGetListFailMessage()
            {
                return new NetMessage(MessageType.GetListFail);
            }
            public static NetMessage GetFailMessage()
            {
                return new NetMessage(MessageType.Fail);
            }
            public static NetMessage GetSuccessMessage()
            {
                return new NetMessage(MessageType.Success);
            }
            public static NetMessage GetMusicPlayedMessage(string md5)
            {
                return new NetMessage(MessageType.MusicPlayed, md5);
            }
        }
    }
}
