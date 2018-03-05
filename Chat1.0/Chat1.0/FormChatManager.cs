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
        public FormChatManager()
        {
            InitializeComponent();
        }

        private void FormChatManager_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            FormLoginSignup FormLogin = new FormLoginSignup();
            FormLogin.ShowDialog();
        }
    }
}
