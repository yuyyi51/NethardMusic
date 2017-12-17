using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NetworkAPI
{
    namespace NetworkMessage
    {
        [Serializable]
        public partial class NetMessage
        {
            public enum MessageType
            {
                Error = 0,
                None = 1,

                SignIn = 11,
                SignUp = 12,
                CheckMD5 = 13,
                Upload = 14,
                GetList = 15,
                MusicPlayed = 16,

                SignInFail = 21,
                SignUpFail = 22,
                CheckMD5Fail = 23,
                GetListFail = 24,

                GetListSuccess = 30,
                Fail = 40,
                Success = 50
            }
            protected MessageType message;
            protected object data;

            public NetMessage(MessageType type)
            {
                message = type;
                data = null;
            }
            public NetMessage(MessageType type, object obj)
            {
                message = type;
                data = obj;
            }
            public MessageType Message
            {
                get { return message; }
            }
            public object Data
            {
                get { return data; }
            }
        }
    }
}

