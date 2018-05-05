using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ServerChatApplication
{
    public static class MessageParser
    {
        // Setup DB object
        private static ChatRoomEntities db = new ChatRoomEntities();

        // Holds the tokenized message
        private static string[] tokenizedMessage;

        public static string[] TokenizedMessage
        {
            set
            {
                tokenizedMessage = value;
                ParseMessage();
            }
        }

        // Tokenize the original message and redirect the program flow
        private static void ParseMessage()
        {

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
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;

            // Create a new message an populate the fields.
            // Once done, then message is saved to the database.
            Message m = new Message();
            m.UserName = tokenizedMessage[1];
            m.Chat_Id = Convert.ToInt32(tokenizedMessage[2]);
            m.Date = DateTime.Now;
            m.Time_Sent = DateTime.Now;
            m.Message_Body = tokenizedMessage[3];

            db.Messages.Add(m);

            // TODO uncomment this when code is check for validity.
            //db.SaveChanges();

            // Compiles a list of all users who are associated
            //  with the chatroom ID that was associated with the message.
            var users = from u in db.Users
                where u.ChatRooms.Any(c => c.Chat_Id == m.Chat_Id)
                select u;

            var replyList = new List<User>(users);




            // Loops through the roster of users and checks which users are also in the active user list.
            // When it finds a match, it sends the message to said user and breaks to the outer loop 
            // to find the next user.
            foreach (User r in replyList)
            {
                foreach (StateObject u in UserList.userList)
                {
                    if (u.userID == r.UserName)
                    {
                        AsynchronousSocketListener.Send(u.workSocket, m.Message_Body);
                        break;
                    }
                }
            }
        }

        private static void ProcessLogin()
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;

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
            int dataStartLocation = 1;

            // Holds result of signup attempt
            bool isValidSignup = false;

            User u = new User();
            u.UserName = tokenizedMessage[dataStartLocation];


            // Checks if the username exists in the database
            if (!db.Users.Any(x => x.UserName == u.UserName))
            {
                byte[] salt = SaltedHash.CreateSalt();                        //Store this value in the database for each user
                string pw = "user entered password";                          //this needs to be the user entered password sent from client
                byte[] saltedpw = SaltedHash.CreateSaltedHash(salt, pw);      //They will then be passed into the method to convert to the saltedhash
                //both salt and saltedpw need to be stored in db for each user
            
                string saltedHash = ""; // will be hooked into Aaron's salted class 
                
                // Set boolean if the operation succeeds  
            }
        }

        private static void ProcessChatroomsRequest()
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;

            // Grabs all of the chatroom rosters that match the userName passed in
            var chatRooms = from c in db.ChatRooms
                where c.Users.Any(u => u.UserName == tokenizedMessage[dataStartLocation])
                select c;

            // List that holds all of the chatroomID's and names to be passed back to the client
            var chatRoomInfo = new List<ChatRoom>(chatRooms);

            List<string> outputList = new List<string>();

            // Iterates through the roster list and grabs the ID's and names while also dividing the
            // data with two different sets of tokens. The foward slash is meant to divide sets of data
            // and the vertical bar divides the ID and name in each set of data.
            foreach (ChatRoom c in chatRoomInfo)
            {
                outputList.Add(c.Chat_Id + "|" + c.ChatName + "|");    
            }

            string concatMessage = String.Join("", chatRoomInfo);
        }
    }
}
