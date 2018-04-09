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
            sct = new Socket(SocketType.Stream, ProtocolType.Tcp);

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
        
        // Sets up Async object recieve
        private void Recieve()
        {
            SocketRecievedData DataReciever = new SocketRecievedData();

            sct.BeginReceive(DataReciever.DataStream, 0, DataReciever.DataSize, 0, new AsyncCallback(RecieveCallBack), DataReciever);
        } 


        // Data preperation and interpretation methods
        public void MessageInterpreter(string message)
        {

        }
        public bool UserSignUp(string username, string password)
        {
            bool SignUpSuccessful = false;
            
            // TODO: Set up server communications for signup;

            return SignUpSuccessful;
        }

        // Callback methods
        // Connect callback opperation
        private void ConnectCallBack(IAsyncResult results)
        {
            Socket sct = (Socket)results.AsyncState;
            sct.EndConnect(results);
            connectMarker.Set();

        }

        // Send callback opperation;
        private void SendCallBack(IAsyncResult results)
        {
            Socket sct = (Socket)results.AsyncState;
            sct.EndSend(results);

        }
        // Recieve callback opperation
        private void RecieveCallBack(IAsyncResult results)
        {
            SocketRecievedData DataReciever = (SocketRecievedData)results.AsyncState;
            Socket sct = DataReciever.Sct;

            int bytesRecieved = sct.EndReceive(results);

            // Checks for continued connection
            if (bytesRecieved > 0)
            {
                DataReciever.Message += Encoding.ASCII.GetString(DataReciever.DataStream);

                // Checks for end of file tag
                if(DataReciever.Message.IndexOf("<EOF>") == -1)
                {
                    // Continues Reading
                    sct.BeginReceive(DataReciever.DataStream, 0, DataReciever.DataSize, 0, new AsyncCallback(RecieveCallBack), DataReciever);
                }
                // If end of file tag is found
                else
                {
                    // Runs message interpreter
                    MessageInterpreter(DataReciever.Message);
                }
            }

        }
    }
}
