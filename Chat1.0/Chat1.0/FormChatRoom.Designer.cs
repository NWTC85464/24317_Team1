namespace Chat1._0
{
    partial class FormChatRoom
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
            this.messageText = new System.Windows.Forms.TextBox();
            this.messageBox = new System.Windows.Forms.ListBox();
            this.sendBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblChatRoomName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // messageText
            // 
            this.messageText.Location = new System.Drawing.Point(12, 442);
            this.messageText.MaxLength = 1000;
            this.messageText.Multiline = true;
            this.messageText.Name = "messageText";
            this.messageText.Size = new System.Drawing.Size(682, 43);
            this.messageText.TabIndex = 1;
            // 
            // messageBox
            // 
            this.messageBox.FormattingEnabled = true;
            this.messageBox.Location = new System.Drawing.Point(12, 51);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(755, 381);
            this.messageBox.TabIndex = 2;
            // 
            // sendBtn
            // 
            this.sendBtn.BackgroundImage = global::Chat1._0.Properties.Resources.greenArrow;
            this.sendBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.sendBtn.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.sendBtn.Location = new System.Drawing.Point(700, 442);
            this.sendBtn.Name = "sendBtn";
            this.sendBtn.Size = new System.Drawing.Size(67, 43);
            this.sendBtn.TabIndex = 0;
            this.sendBtn.Text = "Send";
            this.sendBtn.UseVisualStyleBackColor = true;
            this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Chat1._0.Properties.Resources.zchat;
            this.pictureBox1.Location = new System.Drawing.Point(250, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(85, 44);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // lblChatRoomName
            // 
            this.lblChatRoomName.AutoSize = true;
            this.lblChatRoomName.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChatRoomName.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblChatRoomName.Location = new System.Drawing.Point(341, 9);
            this.lblChatRoomName.Name = "lblChatRoomName";
            this.lblChatRoomName.Size = new System.Drawing.Size(137, 30);
            this.lblChatRoomName.TabIndex = 11;
            this.lblChatRoomName.Text = "Chat Room";
            // 
            // FormChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(794, 512);
            this.Controls.Add(this.lblChatRoomName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.sendBtn);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FormChatRoom";
            this.Text = "FormChatRoom";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.ListBox messageBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblChatRoomName;
    }
}

