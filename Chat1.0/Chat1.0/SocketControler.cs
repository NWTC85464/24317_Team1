﻿using System;
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
        private static ManualResetEvent processSync = new ManualResetEvent(false);
        private bool loginSuccessful = false;
        private bool roomJoinSuccessful = false;
        private string userID = "default";
        private IPEndPoint endPoint;
        private string newRoomid;

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
            try
            {
                byte[] sentByte = Encoding.ASCII.GetBytes(sentString);

                sct.BeginSend(sentByte, 0, sentByte.Length, 0, new AsyncCallback(SendCallBack), sct);
            }
            catch
            {
                MessageBox.Show("Server Connection Failed.");
            }
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
            string output = (action + token + user + token + message + token + eof);
            return output;
        }

        // Message template for sending messages
        private string Template(string chatroom, string message)
        {
            string output = ("<Message>" + token + userID + token + chatroom + token + this.Screen(message) + token + eof);
            return output;
        }

        //Sign up Method
        public bool SendUserSignUpRequest(string username, string password)
        {

            string message = this.Template("<Signup>", this.Screen(username), this.Screen(password));
            this.Send(message);
            processSync.WaitOne();
            if (loginSuccessful)
            {
                this.userID = this.Screen(username);
            }
            return loginSuccessful;
        }

        //Log in Method
        public bool SendUserLoginRequest(string username, string password)
        {

            string message = this.Template("<Login>", this.Screen(username), this.Screen(password));
            this.Send(message);
            processSync.WaitOne();
            if (loginSuccessful)
            {
                this.userID = this.Screen(username);
            }
            return loginSuccessful;
        }

        //Join Chatroom method
        public bool SendJoinChatroomRequest(string chatroom)
        {
            bool JoinSuccessful;
            string message = this.Template("<RoomJoin>", userID, chatroom);
            this.Send(message);
            processSync.WaitOne();
            JoinSuccessful = roomJoinSuccessful;
            roomJoinSuccessful = false;
            return JoinSuccessful;
        }

        //Create Chatroom method
        public string sendCreateChatroomRequest(string name)
        {
            string message = this.Template("<RoomCreate>", userID, "00" + token + this.Screen(name));
            this.Send(message);
            processSync.WaitOne();
            string CreateSuccessful = this.newRoomid;
            this.newRoomid = "";
            return CreateSuccessful;
        }

        // Sending Message method
        public void SendMessage(string chatroom, string message)
        {
            this.Send(this.Template(chatroom, message));
        }

        // Request chatroom data method
        public void SendChatroomsRequest()
        {
            this.Send(this.Template("<Chatrooms>", this.userID, "Request for active Chatrooms"));
        }

        public void SendFriendslistRequest()
        {
            this.Send(this.Template("<FriendsList>", this.userID, "Request for current friends"));
        }
        
        public void SendFriendRequest(string friendID)
        {
            this.Send(this.Template("<FriendRequest>", this.userID, friendID));
        }

        public void SendLeaveChatroomRequest(string roomId)
        {
            this.Send(this.Template("<RoomLeave>", this.userID, roomId));
            this.chatManager.LeaveChatroom(roomId);
        }

// Recieved message interpretation;
        private void MessageInterpreter(string message)
        {
            string[] splitMessage = message.Split(token);

            // Switch for determining message type
            switch (splitMessage[0]) 
            {
                case "<Message>":
                    this.ChatMessageHandler(splitMessage);
                    break;
                case "<RoomJoin>":
                    this.RoomJoinHandler(splitMessage);
                    break;
                case "<RoomCreate>":
                    this.RoomCreateHandler(splitMessage);
                    break;
                case "<Login>":
                    this.LoginHandler(splitMessage);
                    break;
                case "<SignUp>":
                    this.LoginHandler(splitMessage);
                    break;
                case "<FriendsList>":
                    this.FriendsListHandler(splitMessage);
                    break;
                case "<FriendRequest>":
                    this.FriendRequestHandler(splitMessage);
                    break;
                case "<Chatrooms>":
                    this.ChatroomsListHandler(splitMessage);
                    break;
                default:
                    this.UnknownMessage(splitMessage);
                    break;
            }

        }

        private void ChatMessageHandler(string[] message)
        {

            chatManager.MessageReciever(message[1], message[2], message[3]);
        }

        private void RoomJoinHandler(string[] message)
        {
            if (message[1] == "false")
            {
                roomJoinSuccessful = false;
                processSync.Set();
                processSync.Reset();
            }
            else
            {
                roomJoinSuccessful = true;
                processSync.Set();
                processSync.Reset();
            }
        }

        private void RoomCreateHandler(string[] message)
        {
            //TODO: add handler code
        }

        private void LoginHandler(string[] message)
        {

            if (message[1] == "fail") 
            {
                loginSuccessful = false;
                processSync.Set();
                processSync.Reset();
            }
            else 
            {
                loginSuccessful = true;
                processSync.Set();
                processSync.Reset();
            }
            
        }

        private void FriendsListHandler(string[] message)
        {
            this.chatManager.FillFriendsList(message);
        }

        private void FriendRequestHandler(string[] message)
        {
            if(message[1] == "true")
            {
                MessageBox.Show("Friend Request successful.");
                this.SendFriendslistRequest();
            }
        }
        private void ChatroomsListHandler(string[] message)
        {
            this.chatManager.FillChatList(message);
        }

        private void UnknownMessage(string[] message)
        {
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
                this.Recieve();
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
            this.Recieve();

        }

        // Checks that the socket is still connected to the server;
        public void ConnectionMaintinence()
        {
            while (sct.Poll(2000000, SelectMode.SelectWrite))
            {
                Thread.Sleep(10000);
            }
            MessageBox.Show("Connection lost.");
            // TODO: line below commented out for testing.
            //Application.Restart();
        }
    }
}
