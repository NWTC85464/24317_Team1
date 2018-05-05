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
        TextBox txtPasswordConfirm;
        Label lblPasswordConfirm;

        //boolean for signup label
        bool signup = false;

        public FormLoginSignup(SocketController sctctrl)
        {
            InitializeComponent();
            this.sctctrl = sctctrl;
            this.FormClosing += this.loginForm_FormClosing;
        }

        // Declares the reference variable for the SocketControler class in this form
        SocketController sctctrl;

        //TODO Add verify password field and associated code for sign up attempts

        private void FormLoginSignup_Load(object sender, EventArgs e)
        {
            // I have these hidden on form load instead of on design view
            // so the labels can be seen and modified without going under all controls
            //lblDisplayUsernameError.Visible = false;
            //lblDisplayPasswordError.Visible = false;
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            //set sign up to true
            signup = true;

            // Resize the form to allow for addition of a verify password field
            // when the user selects sign up 
            this.Height += 31;

            // Adjusts the controls of the form
            Point p = new Point(9, 161);
            this.btnExit.Location = p;
            p = new Point(111, 161);
            this.btnSignUp.Location = p;
            p = new Point(213, 161);
            this.btnLogin.Location = p;

            // Adds the confirm password control label
            this.lblPasswordConfirm = new Label();
            this.lblPasswordConfirm.Width = 89;
            this.lblPasswordConfirm.Height = 45;
            p = new Point(12,112);
            this.lblPasswordConfirm.Location = p;
            this.lblPasswordConfirm.Text = "Confirm Password:";
            this.lblPasswordConfirm.Font = new Font("Arial Black", 10, FontStyle.Bold);
            this.Controls.Add(lblPasswordConfirm);

            // Adds the Password confirmation box
            this.txtPasswordConfirm = new TextBox();
            this.txtPasswordConfirm.Width = 100;
            this.txtPasswordConfirm.Height = 20;
            p = new Point(103, 128);
            this.txtPasswordConfirm.Location = p;
            this.txtPasswordConfirm.PasswordChar = '*';
            this.Controls.Add(txtPasswordConfirm);

            // Changes the click event on Signup and Login button
            this.btnSignUp.Click -= btnSignUp_Click;
            this.btnSignUp.Click += btnSignUp_SecondClick;
            this.btnLogin.Click -= btnLogin_Click;
            this.btnLogin.Click += btnLogin_SecondClick;
        }

        // New signup method button click
        private void btnSignUp_SecondClick(object sender, EventArgs e)
        {
            //Empty error message texts
            lblDisplayPasswordError.Text = String.Empty;
            lblDisplayUsernameError.Text = String.Empty;

            //Username and password validation
            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    if (txtPassword.Text == txtPasswordConfirm.Text)
                    {
                        if (sctctrl.SendUserSignUpRequest(this.txtUsername.Text, this.txtPassword.Text))
                        {
                            this.FormClosing -= this.loginForm_FormClosing;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Sign up Unsuccessful.");
                        }
                    }
                    else
                    {
                        lblDisplayPasswordError.Text = "Passwords Must Match";
                    }
                }
                else
                {
                    lblDisplayPasswordError.Text = "Enter Password";
                }
            }
            else
            {
                lblDisplayUsernameError.Text = "Enter Username";
            }
        }

        // New Login button click to reset the form;
        private void btnLogin_SecondClick(object sender, EventArgs e)
        {
            //set signup to false
            signup = false;

            this.Height -= 31;
            Point p = new Point(9, 131);
            this.btnExit.Location = p;
            p = new Point(111, 131);
            this.btnSignUp.Location = p;
            p = new Point(213, 131);
            this.btnLogin.Location = p;

            // Hides password confirmation
            this.txtPasswordConfirm.Visible = false;
            this.lblPasswordConfirm.Visible = false;

            // Changes the click event on Signup and Login button
            this.btnSignUp.Click += btnSignUp_Click;
            this.btnSignUp.Click -= btnSignUp_SecondClick;
            this.btnLogin.Click += btnLogin_Click;
            this.btnLogin.Click -= btnLogin_SecondClick;

        }

        // Exit Button Closes the Application
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Login Button Logs user into server after checking username and password
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Empty error message texts
            lblDisplayPasswordError.Text = String.Empty;
            lblDisplayUsernameError.Text = String.Empty;
            // Username and password validation
            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {

                    //if (sctctrl.SendUserLoginRequest(txtUsername.Text, txtPassword.Text))
                    //{
                    //    //Login successful-Open Chat Manager
                    //    this.FormClosing -= this.loginForm_FormClosing;
                    //    this.Close();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Login Unsuccessful.");

                    //    //For now have login go to chat manager for testing purposes
                    //    //TODO remove before usage or when testing signup/login
                    //    this.FormClosing -= this.loginForm_FormClosing;
                    //    this.Close();
                    //}
                    //for testing-delete when need to test login
                    // This removes the application.exit event handler from the formclosing event;
                    this.FormClosing -= this.loginForm_FormClosing;
                    this.Close();
                }
                else
                {
                    lblDisplayPasswordError.Text = "Enter Password";
                }

            }
            else
            {
                lblDisplayUsernameError.Text = "Enter Username";
            }
                
            
            
            
        }


        //Password show checkbox if checked then it will show password
        private void pwd_show_chk_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = (pwd_show_chk.Checked ? '\0' : '*');
            txtPasswordConfirm.PasswordChar = (pwd_show_chk.Checked ? '\0' : '*');
        }
        private void loginForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (signup == true)
            {
                //empty label when password starts typing
                lblDisplayPasswordError.Text = String.Empty;

                //show password strengh depending on length and digits
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    if (txtPassword.Text.Length > 8 && txtPassword.Text.Any(char.IsDigit))
                    {
                        lblDisplayPasswordError.BackColor = Color.Green;
                        lblDisplayPasswordError.Text = "STRONG";
                        lblDisplayPasswordError.ForeColor = Color.White;
                        lblDisplayPasswordError.Font = new Font("Arial Black", 10, FontStyle.Bold);
                    }
                    else if (txtPassword.Text.Length > 4)
                    {
                        lblDisplayPasswordError.BackColor = Color.Gold;
                        lblDisplayPasswordError.Text = "OKAY";
                        lblDisplayPasswordError.ForeColor = Color.White;
                        lblDisplayPasswordError.Font = new Font("Arial Black", 10, FontStyle.Bold);
                    }
                    else
                    {
                        lblDisplayPasswordError.BackColor = Color.DarkRed;
                        lblDisplayPasswordError.Text = "WEAK";
                        lblDisplayPasswordError.ForeColor = Color.White;
                        lblDisplayPasswordError.Font = new Font("Arial Black", 10, FontStyle.Bold);
                    }
                }
                else
                {
                    lblDisplayPasswordError.BackColor = SystemColors.GradientActiveCaption;
                    lblDisplayPasswordError.Text = String.Empty;
                    lblDisplayPasswordError.ForeColor = Color.DarkRed;
                    lblDisplayPasswordError.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);

                }
            }
            else
            {
                lblDisplayPasswordError.BackColor = SystemColors.GradientActiveCaption;
                lblDisplayPasswordError.Text = String.Empty;
                lblDisplayPasswordError.ForeColor = Color.DarkRed;
                lblDisplayPasswordError.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            }
        }
    }
}