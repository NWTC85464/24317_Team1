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
    public partial class FormLoginSignup : Form
    {
        public FormLoginSignup()
        {
            InitializeComponent();
        }
        //TODO Add verify password field and associated code for sign up attempts

        private void FormLoginSignup_Load(object sender, EventArgs e)
        {
            // I have these hidden on form load instead of on design view
            // so the labels can be seen and modified without going under all controls
            lblDisplayUsernameError.Visible = false;
            lblDisplayPasswordError.Visible = false;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            // Resize the form to allow for addition of a verify password field
            // when the user selects sign up 
            this.Height += 200;
        }
    }
}
