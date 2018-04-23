using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    public static class MessageParser
    {
        // Holds the original message that's sent to the server before any tokenizing occurs
        private static string completeMessage;

        // Holds the tokenized message
        private static string[] tokenizedMessage;

        // Get and set for the completeMessage field. Once a new value is set, the tokenize method is called
        public static string CompleteMessage
        {
            get { return completeMessage; }
            set
            {
                completeMessage = value;
                TokenizeMessage();
            }
        }

        // Tokenize the original message and redirect the program flow
        private static void TokenizeMessage()
        {
            tokenizedMessage = completeMessage.Split('|');

            switch (tokenizedMessage[0])
            {
                case "<Message>":
                    ProcessMessage();
                    break;
                case "<Login>":
                    ProcessLogin();
                    break;
                case "<Signup>":
                    ProcessSignup();
                    break;
                case "ClientRequestInfo":
                    ProcessClientRequestInfo();
                    break;
            }
        }

        private static void ProcessMessage()
        {
            // TODO Entity framework code for placing message into database 
            
            // TODO request chatroom information about who is participating in the chat so we can redistribute the messages
            // This process will involve grabbing the usernames which will then be matched up with a list of all clients connected.
            // Loop logic will pick out the usernames that are connected to the chat and verify that their connections are still active,
            // then route the message to the active users.
        }

        private static void ProcessLogin()
        {
            // TODO entity code for checking login info against the accounts we hold in the DB

        }

        private static void ProcessSignup()
        {
            // TODO entity code for registering the user
        }

        private static void ProcessClientRequestInfo()
        {
            // TODO entity code for returning user information like the chatrooms they participate in and so on.
        }
    }
}
