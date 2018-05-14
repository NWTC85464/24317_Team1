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
    public partial class FormChatRoom : Form
    {

        public FormChatRoom(SocketController sctctrl, string chatRoomID)
        {
            InitializeComponent();
            this.chatRoomID = chatRoomID;
            this.sctctrl = sctctrl;
            this.FormClosing += formChatRoom_FormClosing;
            this.lblChatRoomName.Text = chatRoomID;
        }

        // Private data fields
        private string chatRoomID;
        SocketController sctctrl;

        // Gets and Sets to access private fields
        public string ChatRoomID
        {
            get {return chatRoomID;}
            set {chatRoomID = value;}
        }

        public void AddMessageToChatBox(string username, string message) {
            this.messageBox.Invoke(new Action(() =>
            {
                this.messageBox.Items.Add(username + ": " + DateTime.Now.TimeOfDay);
            }));
            
            for (int i = 0;; i += 100)
            {
                if (i < message.Length - 100)
                {
                    this.messageBox.Invoke(new Action(() =>
                    {
                        this.messageBox.Items.Add(message.Substring(i, 100));
                        this.messageBox.SelectedIndex = messageBox.Items.Count - 1;
                    }));
                    
                }
                else
                {
                    this.messageBox.Invoke(new Action(() =>
                    {
                        this.messageBox.Items.Add(message.Substring(i));
                        this.messageBox.SelectedIndex = messageBox.Items.Count - 1;
                    }));
                    break;
                }
                
            }
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (messageText.Text.Length <= 500)
            {
                //send message to method in socket controller
                sctctrl.SendMessage(ChatRoomID, messageText.Text);
                //Empty textbox after sent to listbox
                messageText.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Messages are limited to 500 characters or less");
            }

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            // Close Application
            this.Close();
        }

        public void formChatRoom_FormClosing(Object sender, FormClosingEventArgs e)
        {
            this.sctctrl.SendLeaveChatroomRequest(this.chatRoomID);
        }
    }
}