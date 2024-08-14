namespace ProgrammsSwitcher
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOn = new Button();
            lblWinStatusText = new Label();
            btnOff = new Button();
            btnRefresh = new Button();
            lblCmdStatusText = new Label();
            lblWinStatus = new Label();
            lblCmdStatus = new Label();
            SuspendLayout();
            // 
            // btnOn
            // 
            btnOn.Location = new Point(42, 42);
            btnOn.Name = "btnOn";
            btnOn.Size = new Size(75, 23);
            btnOn.TabIndex = 0;
            btnOn.Text = "ON";
            btnOn.UseVisualStyleBackColor = true;
            btnOn.Click += btnOn_Click;
            // 
            // lblWinStatusText
            // 
            lblWinStatusText.AutoSize = true;
            lblWinStatusText.Location = new Point(42, 99);
            lblWinStatusText.Name = "lblWinStatusText";
            lblWinStatusText.Size = new Size(65, 15);
            lblWinStatusText.TabIndex = 1;
            lblWinStatusText.Text = "Win status:";
            // 
            // btnOff
            // 
            btnOff.Location = new Point(140, 42);
            btnOff.Name = "btnOff";
            btnOff.Size = new Size(75, 23);
            btnOff.TabIndex = 0;
            btnOff.Text = "OFF";
            btnOff.UseVisualStyleBackColor = true;
            btnOff.Click += btnOff_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(245, 42);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 23);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // lblCmdStatusText
            // 
            lblCmdStatusText.AutoSize = true;
            lblCmdStatusText.Location = new Point(42, 124);
            lblCmdStatusText.Name = "lblCmdStatusText";
            lblCmdStatusText.Size = new Size(70, 15);
            lblCmdStatusText.TabIndex = 1;
            lblCmdStatusText.Text = "Cmd status:";
            // 
            // lblWinStatus
            // 
            lblWinStatus.AutoSize = true;
            lblWinStatus.Location = new Point(124, 99);
            lblWinStatus.Name = "lblWinStatus";
            lblWinStatus.Size = new Size(65, 15);
            lblWinStatus.TabIndex = 1;
            lblWinStatus.Text = "Win status:";
            // 
            // lblCmdStatus
            // 
            lblCmdStatus.AutoSize = true;
            lblCmdStatus.Location = new Point(124, 124);
            lblCmdStatus.Name = "lblCmdStatus";
            lblCmdStatus.Size = new Size(70, 15);
            lblCmdStatus.TabIndex = 1;
            lblCmdStatus.Text = "Cmd status:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 213);
            Controls.Add(lblCmdStatus);
            Controls.Add(lblCmdStatusText);
            Controls.Add(lblWinStatus);
            Controls.Add(lblWinStatusText);
            Controls.Add(btnOff);
            Controls.Add(btnRefresh);
            Controls.Add(btnOn);
            Name = "Form1";
            Text = "Включатель";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOn;
        private Label lblWinStatusText;
        private Button btnOff;
        private Button btnRefresh;
        private Label lblCmdStatusText;
        private Label lblWinStatus;
        private Label lblCmdStatus;
    }
}
