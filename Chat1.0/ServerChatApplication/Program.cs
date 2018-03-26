using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ServerChatApplication
{
    // Code used for socket comes from here https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
    class Program
    {
        static void Main(string[] args)
        {
            AsynchronousSocketListener.StartListening();    
            return 0;
        }
    }
}
