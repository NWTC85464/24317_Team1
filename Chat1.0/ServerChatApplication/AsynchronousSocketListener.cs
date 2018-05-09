using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    public class AsynchronousSocketListener
    {
        // Documentation lists as "thread signal" still not sure exactly what it does.
        public static ManualResetEvent allDone = new ManualResetEvent(false);


        // This needs to be left empty for now "maybe forever".
        public AsynchronousSocketListener()
        {
        }

        public static void StartListening()
        {
            // Data buffer for incoming data
            byte[] bytes = new byte[1024];

            // This code is setting up the local endpoint for the socket.
            // Changes may be necessary since hosting is AWS.
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            Console.WriteLine("List of IP addresses on this machine.");

            // Loop through the list of IP addresses and output them to console with their appropriate number attached
            for (int i = 0; i < ipHostInfo.AddressList.Length; i++)
            {
                Console.WriteLine("IP Address #" + i.ToString() + " " + ipHostInfo.AddressList[i]);
            }

            Console.WriteLine("Please select the ip Address you desire by inputting the appropriate number after this line.");
            Console.WriteLine("Select the address that matches the ipv4 pattern.");

            // Grabs user input for the ip address they want to select.
            int userInput = Convert.ToInt32(Console.ReadLine());

            // Sets the ip address to the choice of the user.
            IPAddress ipAddress = ipHostInfo.AddressList[userInput];

            // 1100 seems to work well for now. Still subject to change if issues arise.
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1100);

            // Create the TCP/IP socket.
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                // Currently this allows a 100 pending connections.
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state. 
                    allDone.Reset();

                    // Start an async socket to listen for connections
                    Console.WriteLine("waiting for connection..");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until connection is made before continuing 
                    allDone.WaitOne();
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());   
            }

            Console.WriteLine("\nPress enter to continue");
            Console.Read();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal main thread to continue 
            allDone.Set();

            // Get the socket that handles the client request
            Socket listener = (Socket) ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallBack), state);
        }

        // Either here or one step back, I'm going to add the client side socket to the userList,
        // or skip this step if the user is already in the list
        // then redirect this method so it connects to message parser.
        // Message parser will then call the send callback method after it does its work
        // and choose the correct socket out of the userList
        public static void ReadCallBack(IAsyncResult ar)
        {
            string content = string.Empty;

            // Retrieve state object and handler socket
            // from async state object.
            StateObject state = (StateObject) ar.AsyncState;
            Socket handler = state.workSocket;

            // read data from the client socket
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far. 
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead)); 

                // Check for end of file tag. if it's not there, read more data
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All of the data has been read from the client.
                    // Diplay it on the console
                    // TODO I'm leaving this in until we setup the actual process 
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        content.Length, content);

                    // Tokenize the incoming string to prep for messageParser
                    string[] tokenizedContent = content.Split('|');

                    // Assign the stateID to the second element in the string 
                    // array which "should" always hold the userName 
                    state.userID = tokenizedContent[1];

                    // iterates through the user list and checks if user socket is already in the list
                    // If so, then that specific entry is replaced with the in memory object.
                    bool userInList = false;
                    for (int i = 0; i < UserList.userList.Count; i++)
                    {
                        if (UserList.userList[i].userID == tokenizedContent[1])
                        {
                            userInList = true;
                            UserList.userList[i] = state;
                            break;
                        }
                    }

                    // If the state object was not found in the loop above, it's then added to the list.
                    if (!userInList)
                    {
                        UserList.userList.Add(state);    
                    }
                    
                    // Calls the static messageParser class where the incoming message is sent to be parsed.
                    MessageParser.TokenizedMessage = tokenizedContent;
                }

                else
                {
                    // Not all data received, Get more.
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallBack), state);
                }
            }
        }

        public static void Send(Socket handler, string data)
        {
            // Convert the string data to byte data using ASCII
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallBack), handler);
        }

        // TODO Modify this method to avoid socket shutdown
        private static void SendCallBack(IAsyncResult ar)
        {
            try
            {
                // Retrieve socket from state object
                Socket handler = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to the client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static ChatRoomEntities db = new ChatRoomEntities();

        public static void Main(string[] args)
        {
            StartListening();   

        }
    }
}
