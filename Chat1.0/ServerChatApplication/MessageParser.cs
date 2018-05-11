using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.SQLite;
using System.Linq;
using System.Net.Sockets;
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
                case "<SignUp>":
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
            db.SaveChanges();

            // Compiles a list of all users who are associated
            //  with the chatroom ID that was associated with the message.
            var users = from u in db.Users
                where u.ChatRooms.Any(c => c.Chat_Id == m.Chat_Id)
                select u;

            var replyList = new List<User>(users);

            // Builds the output message that will be returned to all of the connected users
            string output = "<Message>" + "|" +
                            tokenizedMessage[dataStartLocation] + "|" +
                            tokenizedMessage[dataStartLocation + 1] + "|" +
                            m.Message_Body + "|" + "<EOF>";

            // Loops through the roster of users and checks which users are also in the active user list.
            // When it finds a match, it sends the message to said user and breaks to the outer loop 
            // to find the next user.
            foreach (User r in replyList)
            {
                // This variable keeps track of the current selected element in the userList.
                // That lets us know where to remove a list entry if a connection fails.
                int indexCount = 0;

                foreach (StateObject u in UserList.userList)
                {
                    if (u.userID == r.UserName)
                    {
                        if (checkSocketStatus(u.workSocket))
                        {
                            AsynchronousSocketListener.Send(u.workSocket, output);
                            break;
                        }
                            UserList.userList.RemoveAt(indexCount);    
                    }
                    indexCount++;
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
                    byte[] salt = u.Salt; //needs to be retrieved from database
                    string pw = tokenizedMessage[dataStartLocation + 1]; //user entered password from client
                    byte[] saltedHash = u.Password; //needs to be retrieved from database
                    isValidLogin = SaltedHash.Validate(salt, pw, saltedHash);  //Pass in salt, user password, then salted hash. this should return true/false depending on if password validates
                    //These methods will need to be tested and tweaked if necessary. I'm not sure if they work 100% as I am not able to test them
                    break;
                } 
            }

            foreach (StateObject s in UserList.userList)
            {
                if (s.userID == tokenizedMessage[dataStartLocation])
                {
                    string output = "<Login>" + "|" + isValidLogin + "<EOF>";
                    AsynchronousSocketListener.Send(s.workSocket, output);
                    break;
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
                string pw = tokenizedMessage[2];                          //this needs to be the user entered password sent from client
                byte[] saltedpw = SaltedHash.CreateSaltedHash(salt, pw);      //They will then be passed into the method to convert to the saltedhash
                //both salt and saltedpw need to be stored in db for each user

                u.Active = true;
                u.Salt = salt;
                u.Password = saltedpw;
                u.Start_Date = DateTime.Now;
                u.IP_Address = 0m;
                
                db.Users.Add(u);
                db.SaveChanges();

                isValidSignup = true;
            }

            string output = "<SignUp>" + "|" + isValidSignup + "|" + "<EOF>";
            // Holds the current index value of the userList as it iterates
            int indexCount = 0;

            foreach (StateObject s in UserList.userList)
            {
                if (s.userID == u.UserName)
                {
                    if (checkSocketStatus(s.workSocket))
                    {
                        AsynchronousSocketListener.Send(s.workSocket, output);
                        break;
                    }
                    UserList.userList.RemoveAt(indexCount);
                }
                indexCount++;
            }
        }

        private static void ProcessChatroomsRequest()
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;

            // If the signal for chatroom creation is passed in, this should be true
            if (tokenizedMessage[dataStartLocation + 1] == "000000")
            {

            }
            else
            {
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

        // Checks if the passed in socket is still active
        private static bool checkSocketStatus(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
    }
}
