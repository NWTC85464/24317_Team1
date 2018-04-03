using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace Chat1._0
{
    public class SocketControler
    {

        // Data fields.
        private Socket sct;
        // TODO: Add Server Address
        private string ServerAdress = "";
        private static ManualResetEvent connectMarker =new ManualResetEvent(false);

        public SocketControler()
        {
            // TODO: set up the outgoing and incoming socket info
            sct = new Socket(SocketType.Stream, ProtocolType.IPv4);

                IPHostEntry ipHost = Dns.GetHostEntry(ServerAdress);
                IPAddress ip = ipHost.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(ip, 80);

                // Conect to server with a wait to confirm connection;
                sct.BeginConnect(endPoint, new AsyncCallback(ConnectCallBack ) , sct);
                connectMarker.WaitOne();

        }

        // Basic data send and recieve methods of the socket; 
        // Can only accept formatted strings
        private void Send(string sentString)
        {
            byte[] sentByte = Encoding.ASCII.GetBytes(sentString);

            sct.BeginSend(sentByte, 0, sentByte.Length, 0, new AsyncCallback(SendCallBack),  sct);
        }
        
        private void Recieve()
        {

            sct.BeginReceive();
        } 

        public bool UserSignUp(string username, string password)
        {
            bool SignUpSuccessful = false;
            
            // TODO: Set up server communications for signup;

            return SignUpSuccessful;
        }

        // Callback opperations
        // Connect Callback opperation
        private void ConnectCallBack(IAsyncResult results)
        {
            Socket sct = (Socket)results.AsyncState;
            sct.EndConnect(results);
            connectMarker.Set();

        }

        // Send Callback opperation;
        private void SendCallBack(IAsyncResult results)
        {
            Socket sct = (Socket)results.AsyncState;
            sct.EndSend(results);

        }
    }
}
