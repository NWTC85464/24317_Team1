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
    public class SocketController
    {

        // Data fields.
        private FormChatManager chatManager;
        private Socket sct;
        private char token = '|';
        private string eof = "<EOF>";
        private string ServerAdress = "ec2-13-59-47-57.us-east-2.compute.amazonaws.com";
        private static ManualResetEvent connectMarker =new ManualResetEvent(false);
        private static ManualResetEvent loginMarker = new ManualResetEvent(false);
        private bool loginSuccessful = false;
        private string userID;
        private IPEndPoint endPoint;

        public SocketController(FormChatManager chatManager)
        {
            this.chatManager = chatManager;
            // TODO: set up the outgoing and incoming socket info
            sct = new Socket(SocketType.Stream, ProtocolType.Tcp);

                IPHostEntry ipHost = Dns.GetHostEntry(ServerAdress);
                IPAddress ip = ipHost.AddressList[0];
                endPoint = new IPEndPoint(ip, 1100);

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
            SocketReceivedData DataReciever = new SocketReceivedData();

            sct.BeginReceive(DataReciever.DataStream, 0, DataReciever.DataSize, 0, new AsyncCallback(RecieveCallBack), DataReciever);
        } 


// Data preperation and interpretation methods
// Data prep methods;

        //Input screener method
        //Screens for <EOF> and | then removes them
        public String Screen(string input)
        {
            if (input.Contains(eof))
            {
                //Find where "<EOF>" occurs
                int index = input.IndexOf(eof);

                do
                {
                    //Removes "<EOF>" from input string
                    input = input.Remove(index, 5);

                    //Checks for another occurance
                    index = input.IndexOf(eof);

                } while (index != -1); //Continues until all "<EOF>"'s are removed

            }

            if (input.Contains('|'))
            {
                //Find where '|' occurs
                int index = input.IndexOf('|');
                do
                {                    
                    //Removes '|' from input string
                    input = input.Remove(index, 1);

                    //Checks for another occurance
                    index = input.IndexOf('|');

                } while (index != -1); //Continues until all '|'s are removed

            }
            return input;
            
        }
        //Message template method for Login and SignUp
        private string Template(string action, string user, string message)
        {
            string output = (action + token + user + token + message + eof);
            return output;
        }

        // Message template for sending messages
        private string Template(string chatroom, string message)
        {
            string output = ("<Message>" + token + userID + token + chatroom + token + message + token + eof);
            return output;
        }

        //Sign up Method
        public bool UserSignUp(string username, string password)
        {

            string message = this.Template("<Signup>", this.Screen(username), this.Screen(password));
            this.Send(message);
            loginMarker.WaitOne();
            if (loginSuccessful)
            {
                this.userID = this.Screen(username);
            }
            return loginSuccessful;
        }

        //Log in Method
        public bool UserLogin(string username, string password)
        {

            string message = this.Template("<Login>", this.Screen(username), this.Screen(password));
            this.Send(message);
            loginMarker.WaitOne();
            if (loginSuccessful)
            {
                this.userID = this.Screen(username);
            }
            return loginSuccessful;
        }

        //Join Chatroom method
        public bool JoinChatroom(string username, string chatroom)
        {
            bool JoinSuccessful = false;

            //Todo if join successful then
            //JoinSuccessful = true;

            return JoinSuccessful;
        }

        // Sending Message method
        public void SendMessage(string chatroom, string message)
        {
            this.Send(this.Template(chatroom, message));
        }

// Recieved message interpretation;
        private void MessageInterpreter(string message) {
            string[] splitMessage = message.Split(token);

            // Switch for determining message type
            switch (splitMessage[0]) 
            {
                case "<Message>":
                    ChatMessageHandler(splitMessage);
                    break;
                case "<RoomJoin>":

                    break;
                case "<Login>":
                    LoginHandler(splitMessage);
                    break;
                case "<SignUp>":

                    break;
                case "<Friends>":

                    break;
                case "<FriendRequest>":

                    break;
                case "<Chatrooms>":

                    break;
                default:
                    UnknownMessage(splitMessage);
                    break;
            }

        }

        private void LoginHandler(string[] message) {

            if (message[1] == "fail") 
            {
                MessageBox.Show("Login Failed: " + message[2]);
                loginSuccessful = false;
                loginMarker.Set();
                loginMarker.Reset();
            }
            else 
            {
                chatManager.FillChatList(message);
            }
            
        }

        private void ChatMessageHandler(string[] message) {

            chatManager.MessageReciever(message[1], message[2], message[3]);
        }

        private void UnknownMessage(string[] message) {
            MessageBox.Show("Unknown Message Recieved. Tag: " + message[0]);
        }
// Callback methods
        // Connect callback opperation
        private void ConnectCallBack(IAsyncResult results)
        {
            try
            {
                Socket sct = (Socket)results.AsyncState;
                sct.EndConnect(results);
                connectMarker.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                connectMarker.Set();
            }
            this.ConnectionMaintinence();
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
            SocketReceivedData DataReceiver = (SocketReceivedData)results.AsyncState;
            Socket sct = DataReceiver.Sct;

            int bytesRecieved = sct.EndReceive(results);

            // Checks for continued connection
            if (bytesRecieved > 0)
            {
                DataReceiver.Message += Encoding.ASCII.GetString(DataReceiver.DataStream);

                // Checks for end of file tag
                if(DataReceiver.Message.IndexOf(eof) == -1)
                {
                    // Continues Reading
                    sct.BeginReceive(DataReceiver.DataStream, 0, DataReceiver.DataSize, 0, new AsyncCallback(RecieveCallBack), DataReceiver);
                }
                // If end of file tag is found
                else
                {
                    // Runs message interpreter
                    MessageInterpreter(DataReceiver.Message);
                }
            }

        }

        // Checks that the socket is still connected to the server;
        public void ConnectionMaintinence()
        {
            do
            {
                Thread.Sleep(10000);
            } while (sct.Poll(2000000, SelectMode.SelectWrite));
            connectMarker.Reset();
            sct.BeginConnect(endPoint, new AsyncCallback(ConnectCallBack), sct);
        }
    }
}
