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
        private Socket sct;
        private char token = '|';
        private string eof = "<EOF>";

        // TODO: Add Server Address
        private string ServerAdress = "ec2-13-59-47-57.us-east-2.compute.amazonaws.com";
        private static ManualResetEvent connectMarker =new ManualResetEvent(false);

        public SocketController()
        {
            // TODO: set up the outgoing and incoming socket info
            sct = new Socket(SocketType.Stream, ProtocolType.Tcp);

                IPHostEntry ipHost = Dns.GetHostEntry(ServerAdress);
                IPAddress ip = ipHost.AddressList[0];
                IPEndPoint endPoint = new IPEndPoint(ip, 1100);

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
        public void MessageInterpreter(string message)
        {

        }

        //Input screener method
        //Screens for <EOF> and | then removes them
        public String Screen(string input)
        {
            if (input.Contains("<EOF>"))
            {
                //Find where "<EOF>" occurs
                int index = input.IndexOf("<EOF>");

                do
                {
                    //Removes "<EOF>" from input string
                    input = input.Remove(index, 5);

                    //Checks for another occurance
                    index = input.IndexOf("<EOF>");

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
        //Message template method
        private string Template(string action, string user, string message)
        {
            string output = (action + token + user + token + message + eof);
            return output;
        }

        //Sign up Method
        public bool UserSignUp(string username, string password)
        {
            bool SignUpSuccessful = false;

            string message = this.Template("signup", username, password);
            this.Send(message);

            //Todo if user created successfully then
            //SignUpSuccessful = true;


            return SignUpSuccessful;
        }

        //Log in Method
        public bool UserLogin(string username, string password)
        {
            bool LoginSuccessful = false;

            string message = this.Template("login", username, password);
            this.Send(message);

            //Todo if user login successfully then
            //LoginSuccessful = true;


            return LoginSuccessful;
        }

        //Join Chatroom method
        public bool JoinChatroom(string username, string chatroom)
        {
            bool JoinSuccessful = false;

            //Todo if join successful then
            //JoinSuccessful = true;

            return JoinSuccessful;
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
            SocketReceivedData DataReceiver = (SocketReceivedData)results.AsyncState;
            Socket sct = DataReceiver.Sct;

            int bytesRecieved = sct.EndReceive(results);

            // Checks for continued connection
            if (bytesRecieved > 0)
            {
                DataReceiver.Message += Encoding.ASCII.GetString(DataReceiver.DataStream);

                // Checks for end of file tag
                if(DataReceiver.Message.IndexOf("<EOF>") == -1)
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
    }
}
