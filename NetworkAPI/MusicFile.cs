using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAPI
{
    [Serializable]
    public class MusicFile
    {
        public byte[] file;
        public string name;
        public string singer;
        public string suffix;
    }
}
