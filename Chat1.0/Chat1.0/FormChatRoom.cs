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
        public FormChatRoom()
        {
            InitializeComponent();
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            //Send textbox text to listbox
            messageBox.Items.Add(messageText.Text);

            //Empty textbox after sent to listbox
            messageText.Text = string.Empty;

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            // Close Application
            Application.Exit();
        }

        private void chatManagerButton_Click(object sender, EventArgs e)
        {
            // TODO: open chat manager form back up and close this form
            this.Close();
            
            
        }
    }
}
