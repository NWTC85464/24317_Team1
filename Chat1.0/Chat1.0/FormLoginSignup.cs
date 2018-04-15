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
        //Create textbox
        TextBox txtPasswordConfirm = new TextBox();

        public FormLoginSignup(SocketController sctctrl)
        {
            InitializeComponent();
            this.sctctrl = sctctrl;
        }

        // Declares the reference variable for the SocketControler class in this form
        SocketController sctctrl;

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
            this.Height += 51;

            // Adjusts the controls of the form
            Point p = new Point(9, 161);
            this.btnExit.Location = p;
            p = new Point(111, 161);
            this.btnSignUp.Location = p;
            this.btnLogin.Visible = false;

            // Adds the confirm password control label
            Label lblPasswordConfirm = new Label();
            lblPasswordConfirm.Width = 89;
            lblPasswordConfirm.Height = 45;
            p = new Point(12,112);
            lblPasswordConfirm.Location = p;
            lblPasswordConfirm.Text = "Confirm Password:";
            lblPasswordConfirm.Font = new Font("Arial Black", 10, FontStyle.Bold);
            this.Controls.Add(lblPasswordConfirm);

            // Adds the Password confirmation box
            this.txtPasswordConfirm.Width = 100;
            this.txtPasswordConfirm.Height = 20;
            p = new Point(103, 124);
            this.txtPasswordConfirm.Location = p;
            this.txtPasswordConfirm.PasswordChar = '*';
            this.Controls.Add(txtPasswordConfirm);

            // Changes the click event on Signup button
            this.btnSignUp.Click -= btnSignUp_Click;
            this.btnSignUp.Click += btnSignUp_SecondClick;
        }

        // New signup method button click
        private void btnSignUp_SecondClick(object sender, EventArgs e)
        {
            if(sctctrl.Screen(txtPassword.Text) == sctctrl.Screen(txtPasswordConfirm.Text))
            {
                if (sctctrl.UserSignUp(sctctrl.Screen(this.txtUsername.Text), sctctrl.Screen(this.txtPassword.Text)))
                {
                    MessageBox.Show("Sign Up successful. Please Login.");
                }
                else
                {
                    MessageBox.Show("Sign up Unsuccessful.");
                }
            }
            else
            {
                MessageBox.Show("Passwords don't match.");
            }
        }

        // Exit Button Closes the Application
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Login Button Logs user into server after checking username and password
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(sctctrl.UserLogin(sctctrl.Screen(txtUsername.Text), sctctrl.Screen(txtPassword.Text)))
            {
                //Login successful-Open Chat Manager
                User user1 = new User(sctctrl.Screen(txtUsername.Text));
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Unsuccessful.");

                //For now have login go to chat manager for testing purposes
                //TODO remove before usage or when testing signup/login
                this.Close();
            }
            
        }


        //Password show checkbox if checked then it will show password
        private void pwd_show_chk_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = (pwd_show_chk.Checked ? '\0' : '*');
        }
    }
}
