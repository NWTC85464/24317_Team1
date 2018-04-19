namespace Chat1._0
{
    partial class FormChatManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chatList = new System.Windows.Forms.ListView();
            this.joinButton = new System.Windows.Forms.Button();
            this.searchTextbox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.friendList = new System.Windows.Forms.ListBox();
            this.friendListLabel = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatList
            // 
            this.chatList.Location = new System.Drawing.Point(27, 29);
            this.chatList.Name = "chatList";
            this.chatList.Size = new System.Drawing.Size(217, 471);
            this.chatList.TabIndex = 0;
            this.chatList.UseCompatibleStateImageBehavior = false;
            // 
            // joinButton
            // 
            this.joinButton.BackColor = System.Drawing.Color.Green;
            this.joinButton.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.joinButton.ForeColor = System.Drawing.SystemColors.Control;
            this.joinButton.Location = new System.Drawing.Point(250, 144);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(110, 35);
            this.joinButton.TabIndex = 1;
            this.joinButton.Text = "Join";
            this.joinButton.UseVisualStyleBackColor = false;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // searchTextbox
            // 
            this.searchTextbox.Location = new System.Drawing.Point(250, 29);
            this.searchTextbox.Name = "searchTextbox";
            this.searchTextbox.Size = new System.Drawing.Size(110, 20);
            this.searchTextbox.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.searchButton.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.SystemColors.Control;
            this.searchButton.Location = new System.Drawing.Point(250, 55);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(110, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // friendList
            // 
            this.friendList.FormattingEnabled = true;
            this.friendList.Location = new System.Drawing.Point(250, 230);
            this.friendList.Name = "friendList";
            this.friendList.Size = new System.Drawing.Size(110, 212);
            this.friendList.TabIndex = 6;
            //this.friendList.SelectedIndexChanged += new System.EventHandler(this.friendList_SelectedIndexChanged);
            // 
            // friendListLabel
            // 
            this.friendListLabel.AutoSize = true;
            this.friendListLabel.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.friendListLabel.Location = new System.Drawing.Point(250, 207);
            this.friendListLabel.Name = "friendListLabel";
            this.friendListLabel.Size = new System.Drawing.Size(103, 23);
            this.friendListLabel.TabIndex = 7;
            this.friendListLabel.Text = "Friend List";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Maroon;
            this.btnExit.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.SystemColors.Control;
            this.btnExit.Location = new System.Drawing.Point(250, 477);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(110, 23);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormChatManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(380, 512);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.friendListLabel);
            this.Controls.Add(this.friendList);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchTextbox);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.chatList);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FormChatManager";
            this.Text = "FormChatManager";
            this.Load += new System.EventHandler(this.FormChatManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView chatList;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.TextBox searchTextbox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListBox friendList;
        private System.Windows.Forms.Label friendListLabel;
        private System.Windows.Forms.Button btnExit;
    }
}