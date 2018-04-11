using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Chat1._0
{
    class SocketReceivedData
    {
        private const int dataSize = 256;
        private Byte[] dataStream = new byte[dataSize];
        private string message = "";
        private Socket sct;

        // Field gets and sets
        public int DataSize
        {
            get {return dataSize;}
        }

        public byte[] DataStream
        {
            get {return dataStream;}
            set {dataStream = value;}
        }

        public string Message
        {
            get {return message;}
            set {message = value;}
        }

        public Socket Sct
        {
            get {return sct;}
            set {sct = value;}
        }
    }
}
