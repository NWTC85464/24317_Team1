using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat1._0
{
    public partial class FormChatManager : Form
    {
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
            sctctrl.SendChatroomsRequest();
            sctctrl.SendFriendslistRequest();
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
            //check for selection
            if (chatList1.SelectedItem != null)
                {
                    //check list box for selected chatroom
                    string chatroom = chatList1.SelectedItem.ToString();
                    chatroom = chatroom.Substring(0, chatroom.IndexOf(" "));
                    //Need to use join method in socket controller
                    if (sctctrl.SendJoinChatroomRequest(chatroom))
                         {

                            //Open Chat Manager form if connection is complete
                            FormChatRoom F = new FormChatRoom(sctctrl, chatroom);
                            ChatRoomForms.Add(F);
                            F.Show();
                          }

                         //tell user join failed
                        else
                        {
                        MessageBox.Show("Join unsuccessful");
                        }
                }
                else
                {
                    MessageBox.Show("Select a Chatroom to join.");
                }

            //Uncomment below and comment above for testing ChatRoom form
            //FormChatRoom FormChat = new FormChatRoom(sctctrl, "chatroom");
            //ChatRoomForms.Add(FormChat);
            //FormChat.Show();
        }

        public void LeaveChatroom(string roomId)
        {
            bool found = false;
            int count = 0;
            foreach (FormChatRoom f in ChatRoomForms)
            {
                if (roomId == f.ChatRoomID)
                {
                    found = true;
                    break;
                }
                count++;
            }
            if (found)
            {
                ChatRoomForms.RemoveAt(count);
            }
            else
            {
                MessageBox.Show("Error: Room not Found in Chatrooms List");
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

            if (i == ChatRoomForms.Count)
            {
                MessageBox.Show("Message sent to nonexistent chat room " + chatRoomID);
            }
            else
            {
                ChatRoomForms.ElementAt(i).AddMessageToChatBox(username, message);
            }
        }

        // Method for filling chatlist with array of string data recieved from server side application.
        public void FillChatList(string[] recievedChatList)
        {
            // Clears out old data and refills with new data.
            chatList1.Invoke(new Action(() =>
            {
                chatList1.Items.Clear();
            }));

            
            for (int i = 1; i < recievedChatList.Length - 2; i += 2)
            {
                chatList1.Invoke(new Action(() =>
                {
                    chatList1.Items.Add(recievedChatList[i] + " " + recievedChatList[i + 1]);
                }));
                
            }
        }



        // Method for filling friends list with string data recieved from server side application.
        public void FillFriendsList(string[] recievedFriendsList)
        {
            this.friendList.Items.Clear();
            for (int i = 2; i < recievedFriendsList.Length - 1; i++)
            {
                this.friendList.Items.Add(recievedFriendsList[i]);
            }
        }

        //Create chat Button first click
        private void createChatBtn_Click(object sender, EventArgs e)
        {
            this.createText.Visible = true;
            this.createChatBtn.Text = "Create";
            Point p = new Point(250, 106);
            this.joinButton.Location = p;

            //changes click even for second click
            this.createChatBtn.Click -= createChatBtn_Click;
            this.createChatBtn.Click += createChatBtn_SecondClick;
        }

        //create chat button second click
        private void createChatBtn_SecondClick(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(createText.Text))
            {
                //create chat method in socket controller
                string newChatroomID = sctctrl.sendCreateChatroomRequest(createText.Text);
                if (newChatroomID != "")
                {
                    FormChatRoom tempform = new FormChatRoom(sctctrl, newChatroomID);
                    tempform.Show();
                    this.ChatRoomForms.Add(tempform);
                    this.sctctrl.SendChatroomsRequest();
                }
                else
                {
                    MessageBox.Show("Failed to create new Chatroom.");
                }
            }
            else
            {
                MessageBox.Show("Please name Chat.");
            }
        }
    }
}
