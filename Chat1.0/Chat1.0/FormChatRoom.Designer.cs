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
            this.exitButton = new System.Windows.Forms.Button();
            this.chatManagerButton = new System.Windows.Forms.Button();
            this.sendBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageText
            // 
            this.messageText.Location = new System.Drawing.Point(12, 442);
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
            this.messageBox.SelectedIndexChanged += new System.EventHandler(this.messageBox_SelectedIndexChanged);
            // 
            // exitButton
            // 
            this.exitButton.BackColor = System.Drawing.Color.Maroon;
            this.exitButton.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitButton.ForeColor = System.Drawing.SystemColors.Control;
            this.exitButton.Location = new System.Drawing.Point(674, 22);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(93, 23);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // chatManagerButton
            // 
            this.chatManagerButton.BackColor = System.Drawing.Color.Green;
            this.chatManagerButton.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatManagerButton.ForeColor = System.Drawing.SystemColors.Control;
            this.chatManagerButton.Location = new System.Drawing.Point(566, 22);
            this.chatManagerButton.Name = "chatManagerButton";
            this.chatManagerButton.Size = new System.Drawing.Size(102, 23);
            this.chatManagerButton.TabIndex = 4;
            this.chatManagerButton.Text = "New";
            this.chatManagerButton.UseVisualStyleBackColor = false;
            this.chatManagerButton.Click += new System.EventHandler(this.chatManagerButton_Click);
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
            // FormChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(794, 512);
            this.Controls.Add(this.chatManagerButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.messageText);
            this.Controls.Add(this.sendBtn);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FormChatRoom";
            this.Text = "FormChatRoom";
            this.Load += new System.EventHandler(this.FormChatRoom_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button sendBtn;
        private System.Windows.Forms.TextBox messageText;
        private System.Windows.Forms.ListBox messageBox;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button chatManagerButton;
    }
}

