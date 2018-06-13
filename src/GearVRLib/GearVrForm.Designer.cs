namespace Driver4VR.GearVR
{
    partial class GearVrForm
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
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBitStart = new System.Windows.Forms.TextBox();
			this.textBitEnd = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.labelFoundNo = new System.Windows.Forms.Label();
			this.buttonStart = new System.Windows.Forms.Button();
			this.buttonConnect = new System.Windows.Forms.Button();
			this.labelConnected = new System.Windows.Forms.Label();
			this.labelResult = new System.Windows.Forms.Label();
			this.buttonKickEvents = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 30;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label1.Location = new System.Drawing.Point(632, 169);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 14);
			this.label1.TabIndex = 11;
			this.label1.Text = "label1";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(632, 491);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "label2";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(549, 21);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(168, 23);
			this.button3.TabIndex = 38;
			this.button3.Text = "Find controllers";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.richTextBox1.Location = new System.Drawing.Point(12, 61);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(531, 545);
			this.richTextBox1.TabIndex = 40;
			this.richTextBox1.Text = "";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(632, 156);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 41;
			this.label3.Text = "Controller 1 status";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(632, 478);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(91, 13);
			this.label4.TabIndex = 42;
			this.label4.Text = "Controller 2 status";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 13);
			this.label5.TabIndex = 43;
			this.label5.Text = "Controller 1 bits";
			// 
			// textBitStart
			// 
			this.textBitStart.Location = new System.Drawing.Point(665, 349);
			this.textBitStart.Name = "textBitStart";
			this.textBitStart.Size = new System.Drawing.Size(100, 20);
			this.textBitStart.TabIndex = 44;
			this.textBitStart.Text = "0";
			this.textBitStart.TextChanged += new System.EventHandler(this.textBits_TextChanged);
			this.textBitStart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBits_TextChanged);
			// 
			// textBitEnd
			// 
			this.textBitEnd.Location = new System.Drawing.Point(665, 375);
			this.textBitEnd.Name = "textBitEnd";
			this.textBitEnd.Size = new System.Drawing.Size(100, 20);
			this.textBitEnd.TabIndex = 45;
			this.textBitEnd.Text = "0";
			this.textBitEnd.TextChanged += new System.EventHandler(this.textBits_TextChanged);
			this.textBitEnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBits_TextChanged);
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(665, 401);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(100, 20);
			this.textBox3.TabIndex = 46;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label6.Location = new System.Drawing.Point(580, 352);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(70, 14);
			this.label6.TabIndex = 47;
			this.label6.Text = "bit start";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label7.Location = new System.Drawing.Point(594, 378);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 14);
			this.label7.TabIndex = 48;
			this.label7.Text = "bit end";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label8.Location = new System.Drawing.Point(608, 404);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(42, 14);
			this.label8.TabIndex = 49;
			this.label8.Text = "value";
			// 
			// labelFoundNo
			// 
			this.labelFoundNo.AutoSize = true;
			this.labelFoundNo.Location = new System.Drawing.Point(737, 26);
			this.labelFoundNo.Name = "labelFoundNo";
			this.labelFoundNo.Size = new System.Drawing.Size(49, 13);
			this.labelFoundNo.TabIndex = 50;
			this.labelFoundNo.Text = "Found: 0";
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(549, 80);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(168, 23);
			this.buttonStart.TabIndex = 51;
			this.buttonStart.Text = "Initialize and subscribe to events";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// buttonConnect
			// 
			this.buttonConnect.Location = new System.Drawing.Point(549, 50);
			this.buttonConnect.Name = "buttonConnect";
			this.buttonConnect.Size = new System.Drawing.Size(168, 23);
			this.buttonConnect.TabIndex = 52;
			this.buttonConnect.Text = "Connect";
			this.buttonConnect.UseVisualStyleBackColor = true;
			this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click_1);
			// 
			// labelConnected
			// 
			this.labelConnected.AutoSize = true;
			this.labelConnected.Location = new System.Drawing.Point(737, 55);
			this.labelConnected.Name = "labelConnected";
			this.labelConnected.Size = new System.Drawing.Size(77, 13);
			this.labelConnected.TabIndex = 53;
			this.labelConnected.Text = "Connected: no";
			// 
			// labelResult
			// 
			this.labelResult.AutoSize = true;
			this.labelResult.Location = new System.Drawing.Point(737, 85);
			this.labelResult.Name = "labelResult";
			this.labelResult.Size = new System.Drawing.Size(72, 13);
			this.labelResult.TabIndex = 54;
			this.labelResult.Text = "Listening: n/a";
			// 
			// button1
			// 
			this.buttonKickEvents.Location = new System.Drawing.Point(549, 109);
			this.buttonKickEvents.Name = "button1";
			this.buttonKickEvents.Size = new System.Drawing.Size(168, 23);
			this.buttonKickEvents.TabIndex = 55;
			this.buttonKickEvents.Text = "Kick events";
			this.buttonKickEvents.UseVisualStyleBackColor = true;
			this.buttonKickEvents.Click += new System.EventHandler(this.buttonKickEvents_Click);
			// 
			// GearVrForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(905, 714);
			this.Controls.Add(this.buttonKickEvents);
			this.Controls.Add(this.labelResult);
			this.Controls.Add(this.labelConnected);
			this.Controls.Add(this.buttonConnect);
			this.Controls.Add(this.buttonStart);
			this.Controls.Add(this.labelFoundNo);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBitEnd);
			this.Controls.Add(this.textBitStart);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "GearVrForm";
			this.Text = "ColorForm";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBitStart;
		private System.Windows.Forms.TextBox textBitEnd;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label labelFoundNo;
		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonConnect;
		private System.Windows.Forms.Label labelConnected;
		private System.Windows.Forms.Label labelResult;
		private System.Windows.Forms.Button buttonKickEvents;
	}
}