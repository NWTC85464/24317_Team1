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
            this.joinButton = new System.Windows.Forms.Button();
            this.searchTextbox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.friendList = new System.Windows.Forms.ListBox();
            this.friendListLabel = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.createChatBtn = new System.Windows.Forms.Button();
            this.chatList1 = new System.Windows.Forms.ListBox();
            this.chatRoomListLabel = new System.Windows.Forms.Label();
            this.createText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // joinButton
            // 
            this.joinButton.BackColor = System.Drawing.Color.Green;
            this.joinButton.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.joinButton.ForeColor = System.Drawing.SystemColors.Control;
            this.joinButton.Location = new System.Drawing.Point(250, 83);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(110, 23);
            this.joinButton.TabIndex = 1;
            this.joinButton.Text = "Join Chat";
            this.joinButton.UseVisualStyleBackColor = false;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            // 
            // searchTextbox
            // 
            this.searchTextbox.Location = new System.Drawing.Point(250, 132);
            this.searchTextbox.Name = "searchTextbox";
            this.searchTextbox.Size = new System.Drawing.Size(110, 20);
            this.searchTextbox.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.searchButton.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchButton.ForeColor = System.Drawing.SystemColors.Control;
            this.searchButton.Location = new System.Drawing.Point(250, 158);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(110, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search Chat";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // friendList
            // 
            this.friendList.FormattingEnabled = true;
            this.friendList.Location = new System.Drawing.Point(250, 271);
            this.friendList.Name = "friendList";
            this.friendList.Size = new System.Drawing.Size(103, 147);
            this.friendList.TabIndex = 6;
            // 
            // friendListLabel
            // 
            this.friendListLabel.AutoSize = true;
            this.friendListLabel.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.friendListLabel.Location = new System.Drawing.Point(250, 245);
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
            // createChatBtn
            // 
            this.createChatBtn.BackColor = System.Drawing.Color.Green;
            this.createChatBtn.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createChatBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.createChatBtn.Location = new System.Drawing.Point(250, 54);
            this.createChatBtn.Name = "createChatBtn";
            this.createChatBtn.Size = new System.Drawing.Size(110, 23);
            this.createChatBtn.TabIndex = 9;
            this.createChatBtn.Text = "Create Chat";
            this.createChatBtn.UseVisualStyleBackColor = false;
            this.createChatBtn.Click += new System.EventHandler(this.createChatBtn_Click);
            // 
            // chatList1
            // 
            this.chatList1.FormattingEnabled = true;
            this.chatList1.Location = new System.Drawing.Point(12, 54);
            this.chatList1.Name = "chatList1";
            this.chatList1.Size = new System.Drawing.Size(218, 446);
            this.chatList1.TabIndex = 10;
            // 
            // chatRoomListLabel
            // 
            this.chatRoomListLabel.AutoSize = true;
            this.chatRoomListLabel.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatRoomListLabel.Location = new System.Drawing.Point(48, 21);
            this.chatRoomListLabel.Name = "chatRoomListLabel";
            this.chatRoomListLabel.Size = new System.Drawing.Size(150, 30);
            this.chatRoomListLabel.TabIndex = 11;
            this.chatRoomListLabel.Text = "Chat Rooms";
            // 
            // createText
            // 
            this.createText.Location = new System.Drawing.Point(250, 80);
            this.createText.Name = "createText";
            this.createText.Size = new System.Drawing.Size(110, 20);
            this.createText.TabIndex = 12;
            this.createText.Visible = false;
            // 
            // FormChatManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(372, 512);
            this.Controls.Add(this.createText);
            this.Controls.Add(this.chatRoomListLabel);
            this.Controls.Add(this.chatList1);
            this.Controls.Add(this.createChatBtn);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.friendListLabel);
            this.Controls.Add(this.friendList);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchTextbox);
            this.Controls.Add(this.joinButton);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FormChatManager";
            this.Text = "FormChatManager";
            this.Load += new System.EventHandler(this.FormChatManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.TextBox searchTextbox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ListBox friendList;
        private System.Windows.Forms.Label friendListLabel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button createChatBtn;
        private System.Windows.Forms.ListBox chatList1;
        private System.Windows.Forms.Label chatRoomListLabel;
        private System.Windows.Forms.TextBox createText;
    }
}