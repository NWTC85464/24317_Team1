using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Infrastructure;
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
            Console.WriteLine("Checking header");
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
                case "<Chatrooms>":
                    ProcessChatroomsRequest();
                    break;
                case "<RoomJoin>":
                    ProcessRoomJoin();
                    break;
                case "<RoomCreate>":
                    ProcessRoomCreate();
                    break;
                case "<RoomLeave>":
                    ProcessRoomLeave();
                    break;
                case "<FriendsList>":
                    break;
            }
        }
        
        private static void ProcessMessage()
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;
            ChatRoomEntities1 db = new ChatRoomEntities1();
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
            var replyList = db.Users.ToList().Where(x => x.ChatRoomRosters.Any(u => u.Chat_Id == m.Chat_Id));

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
                CheckStatusAndSend(output, r.UserName);
            }
        }

        private static void ProcessLogin()
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;
            ChatRoomEntities1 db = new ChatRoomEntities1();
            // The bool that's returned to the client stating if the login has succeeded or failed.
            bool isValidLogin = false;

            // To list the user table to allow for easier access
            List<User> users = db.Users.ToList();
            
            foreach (User u in users)
            {
                if (u.UserName == tokenizedMessage[dataStartLocation])
                {
                    byte[] salt = u.Salt; 
                    string pw = tokenizedMessage[dataStartLocation + 1]; 
                    byte[] saltedHash = u.Password; 
                    isValidLogin = SaltedHash.Validate(salt, pw, saltedHash);  
                    break;
                } 
            }
            string output = "<Login>" + "|" + isValidLogin + "|" + "<EOF>";

            CheckStatusAndSend(output, tokenizedMessage[dataStartLocation]);
        }

        private static void ProcessSignup()
        {
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;
            ChatRoomEntities1 db = new ChatRoomEntities1();
            // Holds result of signup attempt
            bool isValidSignup = false;

            User u = new User();
            u.UserName = tokenizedMessage[dataStartLocation];


            // Checks if the username exists in the database
            if (!db.Users.Any(x => x.UserName == u.UserName))
            {
                byte[] salt = SaltedHash.CreateSalt();                      
                string pw = tokenizedMessage[2];                          
                byte[] saltedpw = SaltedHash.CreateSaltedHash(salt, pw);      
                

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
           
            CheckStatusAndSend(output, u.UserName);
        }

        private static void CheckStatusAndSend(string output, string userName)
        {
            int indexCount = 0;

            foreach (StateObject s in UserList.userList)
            {
                if (s.userID == userName)
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

        private static void ProcessChatroomsRequest(string userName = "")
        {
            Console.WriteLine("Processing chatroom request");
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;
            ChatRoomEntities1 db = new ChatRoomEntities1();

            // Grabs all of the chatrooms
            var chatRooms = db.ChatRooms.ToList();

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

            string concatMessage = string.Join("", outputList);

            Console.WriteLine("Concat message: " + concatMessage);

            string output = $"<Chatrooms>|{concatMessage}<EOF>";

            if (userName == "")
                userName = tokenizedMessage[dataStartLocation];

            CheckStatusAndSend(output, userName);
        }

        private static void ProcessRoomJoin(bool isChatroomCreationJoin = false)
        {
            // TDB based off tokenizing pattern. When design is concluded,
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;
            ChatRoomEntities1 db = new ChatRoomEntities1();
            var chatRoster = new ChatRoomRoster();
            long tempChatID;

            // If this is true, then the room join is because the room was just created and the client doesn't have access to the 
            // chatID yet, so this is a workaround that will grab the value of the last created chatroom.
            if (isChatroomCreationJoin)
                tempChatID = db.ChatRooms.Max(c => c.Chat_Id);
            else 
                tempChatID = long.Parse(tokenizedMessage[2]);

            chatRoster.Chat_Id = tempChatID;
            chatRoster.UserName = tokenizedMessage[1];
            string chatID = chatRoster.Chat_Id.ToString();
            string userName = tokenizedMessage[dataStartLocation];
            bool isValid = true;

            // Checks if the relationship between user and chatroom has already been created.
            if (db.ChatRoomRosters.Any(x => x.Chat_Id.ToString() == chatID && x.UserName == userName))
                Console.WriteLine("Matching relationship already existed");
            else
            {
                try
                {
                    db.ChatRoomRosters.Add(chatRoster);
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    Console.WriteLine("Caught DB update exception on room join");
                    isValid = false;
                }
            }


            string output = $"<RoomJoin>|{isValid}|<EOF>";
            CheckStatusAndSend(output, tokenizedMessage[dataStartLocation]); 
        }

        private static void ProcessRoomCreate()
        {
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;
            ChatRoomEntities1 db = new ChatRoomEntities1();
            ChatRoom c = new ChatRoom();
            c.Chat_Id = db.ChatRooms.Count();
            c.ChatName = tokenizedMessage[dataStartLocation + 2];
            c.Active = true;

            db.ChatRooms.Add(c);
            db.SaveChanges();

            string output = $"<RoomCreate>|True|{c.Chat_Id}|<EOF>";
            CheckStatusAndSend(output, tokenizedMessage[dataStartLocation]);

            string userName = tokenizedMessage[dataStartLocation];
            IEnumerable<User> replyList = db.Users.Where(x => x.UserName != userName);

            foreach (User u in replyList)
            {
                ProcessChatroomsRequest(u.UserName);    
            }

            // Since the process for roomJoining is slightly different when preceded by roomCreation
            // This is accounted by sending the RoomJoin method a boolean value that will tell it to pursue
            // a slightly different set of actions.
            ProcessRoomJoin(true);
        }

        private static void ProcessRoomLeave()
        {
            // variable dataStartLocation will indicate where the data portion is held in the array
            int dataStartLocation = 1;

            var chatRost = new ChatRoomRoster();
            chatRost.Chat_Id = Convert.ToInt64(tokenizedMessage[2]);
            chatRost.UserName = tokenizedMessage[dataStartLocation];
            ChatRoomEntities1 db = new ChatRoomEntities1();
            try
            {
                db.ChatRoomRosters.Attach(chatRost);
                db.ChatRoomRosters.Remove(chatRost);
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine("Caught DB exception on leaving room");    
            }


            string output = $"<RoomLeave>|True|<EOF>";
            CheckStatusAndSend(output, tokenizedMessage[dataStartLocation]);
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
