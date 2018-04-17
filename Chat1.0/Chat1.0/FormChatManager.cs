using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Chat1._0
{
    public partial class FormChatManager : Form
    {

        // Structure for storing friend's information
        struct friend
        {
            public int id;
            public string fname;
            public string lname;
            public bool online;
            public string user_name;

        }
        // Array for soring friend's data 
        friend[] frnd_data;

        // SQLite connection object
        // Making database connection
        public System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=teamChat.sqlite");

        // User object to store logged in user information
        public User user;

        // Parameterized constructor
        public FormChatManager()
        {
            InitializeComponent();
        }

        // Private Fields of the Chat manager
        SocketController sctctrl;
        List<FormChatRoom> ChatRoomForms = new List<FormChatRoom>();

        private void FormChatManager_Load(object sender, EventArgs e)
        {
            this.Visible = false;

            //TODO: Testing will be done after database completion

            // Why is this here? The Client will have no connection to the database and this is even before the socket connects to the server or the user logs in.
            // Finding friends
            //findFriends();

            // Making list
            //makeFriendList();

            // Try catch block for Server connection
            try
            {
                sctctrl = new SocketController(this);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                Application.Exit();
            }

            // creates and displays the login form
            FormLoginSignup FormLogin = new FormLoginSignup(sctctrl);
            FormLogin.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Closes Application
            Application.Exit();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            // TODO: Add code to show only items that contain searched phrase


        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            //check list box for selected chatroom
            string chatroom = chatList.SelectedItems.ToString();
             
            // Need to use join method in socket controller
            if (true)
            {
                //Make chat manager invisible
                this.Visible = false;

                // Open Chat Manager form if connection is complete
                FormChatRoom FormChat = new FormChatRoom(chatroom);
                ChatRoomForms.Add(FormChat);
                FormChat.Show();
            }

            // Tell user to enter alias
            else
            {
                MessageBox.Show("Join unsuccessful");
            }

            
        }

        // Method for sending recieved messages to the propper chat window.
        public void MessageReciever(string username, string chatRoomID, string message) 
        {

            int i = 0;
            while ((i < ChatRoomForms.Count) && (ChatRoomForms.ElementAt(i).ChatRoomID != chatRoomID))
            {
                i++;
            }

            if (i == ChatRoomForms.Count) {
                MessageBox.Show("Message sent to nonexistent chat room " + chatRoomID);
            }
            else 
            {
                ChatRoomForms.ElementAt(i).AddMessageToChatBox(username, message);
            }
        }

        public void FillChatList(string[] recievedChatList) 
        {
            for( int i = 2; i < recievedChatList.Length; i++) 
            {
                this.chatList.Items.Add(recievedChatList[i]);
            }
        }

        private void friendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(friendList.Items.Count != 0)
            {
                FormChatRoom room = new FormChatRoom();
#pragma warning disable CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
                room.frnd.id = frnd_data[friendList.SelectedIndex].id;
#pragma warning restore CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
#pragma warning disable CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
                room.frnd.online = frnd_data[friendList.SelectedIndex].online;
#pragma warning restore CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
#pragma warning disable CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
                room.frnd.user_name = frnd_data[friendList.SelectedIndex].user_name;
#pragma warning restore CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
                room.user = user;
                room.ShowDialog();
                this.Close();
            }
            //TODO: close ChatManger form
        }


        // Function for making friend list 
        private void makeFriendList()
        {
            if(frnd_data != null)
            {
                friendList.BeginUpdate();
                for (int i = 0; i < frnd_data.Count<friend>(); i++)
                {
                    using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();
                        // Getting each friend details
                        cmd.CommandText = "select * from User where userID = " + frnd_data[i].id.ToString() + ";";

                        using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Checking if friend is online or not
                                if (int.Parse(reader["user_online"].ToString()) == 1)
                                {
                                    frnd_data[i].online = true;
                                }

                                // If friend is offline
                                else
                                {
                                    frnd_data[i].online = false;
                                }
                                frnd_data[i].user_name = reader["user_name"].ToString();
                            }
                        }
                    }
                    con.Close();

                    //Adding friend in friend list
                    friendList.Items.Add(frnd_data[i].user_name + (frnd_data[i].online ? " Online" : " Offline"));
                }
                friendList.EndUpdate();
            }
        }

        // Function for finding friends from database
        private void findFriends()
        {
            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(con))
            {
                int j = 0;
                con.Open();
                if (user != null)
                {
                    // Finding friends of logged in user
                    cmd.CommandText = "select * from friend where user1_ID = " + user.user_ID + " or user2_ID = " + user.user_ID + ";";
                    using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Resizing friend array
                            Array.Resize<friend>(ref frnd_data, frnd_data.Count<friend>() + 1);
                            // If friend's ID isn't in user1 field
                            if (int.Parse(reader["user1_ID"].ToString()) == user.user_ID)
                            {
                                frnd_data[j++].id = int.Parse(reader["user2_ID"].ToString());
                            }
                            // If friend's ID isn't in user2 field
                            else
                            {
                                frnd_data[j++].id = int.Parse(reader["user1_ID"].ToString());
                            }
                        }
                    }
                }

                con.Close();
            }
        }
    }
}
