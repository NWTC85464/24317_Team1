using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    public static class MessageParser
    {
        // Setup DB object
        private static ChatRoomEntities1 db = new ChatRoomEntities1();

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
                case "Chatrooms":
                    ProcessChatroomsRequest();
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
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 0;

            // The bool that's returned to the client stating if the login has succeeded or failed.
            bool isValidLogin = false;

            // To list the user table to allow for easier access
            List<User> users = db.Users.ToList();
            
            foreach (User u in users)
            {
                if (u.UserName == tokenizedMessage[dataStartLocation])
                {
                    // TODO run password through salted hash system to see if there's a match on the password.   
                    // Aaron, all I need is a method call where I can place the incoming password as a parameter
                    // so it's run through the salted hash functions and a return value is setup to 
                    // receive the result of the crypto function for comparison to verify that this user 
                    // has the correct password.
                    byte[] salt = new byte[0]; //needs to be retrieved from database
                    string pw = "user entered password"; //user entered password from client
                    byte[] saltedHash = new byte[0]; //needs to be retrieved from database
                    isValidLogin = SaltedHash.Validate(salt, pw, saltedHash);  //Pass in salt, user password, then salted hash. this should return true/false depending on if password validates
                    //These methods will need to be tested and tweaked if necessary. I'm not sure if they work 100% as I am not able to test them
                } 
            }
        }

        private static void ProcessSignup()
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 0;

            User u = new User();
            u.UserName = tokenizedMessage[dataStartLocation];

            /* TODO Aaron. Could you hook your salted hash function into here and another
            spot in the login method so we can compare the user's password to a stored salted hash 
            and also create said salted hash during signup attempt. Just return a string or variable
            holding the result of said salted hash function so I can place it in the database. */
            byte[] salt = SaltedHash.CreateSalt();                        //Store this value in the database for each user
            string pw = "user entered password";                          //this needs to be the user entered password sent from client
            byte[] saltedpw = SaltedHash.CreateSaltedHash(salt, pw);      //They will then be passed into the method to convert to the saltedhash
            //both salt and saltedpw need to be stored in db for each user
            
        }

        private static void ProcessChatroomsRequest()
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 0;

            IEnumerable<ChatRoomRoster> roster = db.ChatRoomRosters
            .ToList()
            .Where(x => x.UserName == tokenizedMessage[dataStartLocation]);

            List<ChatRoom> chatRooms = null;
            var temp = db.ChatRooms.ToList();
        }
    }
}
