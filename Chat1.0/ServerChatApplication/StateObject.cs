using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{

    // Code used for socket comes from here https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
    // This class handles the incoming data from the client
    public class StateObject
    {
        // Client socket
        public Socket workSocket = null;

        // Buffer size
        public const int BufferSize = 1024;

        // Buffer that holds incoming client data
        public byte[] buffer = new byte[BufferSize];

        // Holds incoming messages. Example used has the messages sent back to the client,
        // but this will be adapted to save into the database once it's up
        public StringBuilder sb = new StringBuilder();

        public string userID = String.Empty;

    }
}
