namespace RuleTester
{
    partial class CryptoRuleTester
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
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.retentionText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.thresholdText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.symbolText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.intervalText = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(51, 45);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(538, 22);
            this.filePathTextBox.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(619, 45);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(96, 23);
            this.browseButton.TabIndex = 1;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_OnClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Moving Average Retention";
            // 
            // retentionText
            // 
            this.retentionText.Location = new System.Drawing.Point(248, 90);
            this.retentionText.Name = "retentionText";
            this.retentionText.Size = new System.Drawing.Size(84, 22);
            this.retentionText.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Trend Change Threshold";
            // 
            // thresholdText
            // 
            this.thresholdText.Location = new System.Drawing.Point(248, 130);
            this.thresholdText.Name = "thresholdText";
            this.thresholdText.Size = new System.Drawing.Size(84, 22);
            this.thresholdText.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "CoinPair";
            // 
            // symbolText
            // 
            this.symbolText.Location = new System.Drawing.Point(248, 179);
            this.symbolText.Name = "symbolText";
            this.symbolText.Size = new System.Drawing.Size(84, 22);
            this.symbolText.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 218);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Interval";
            // 
            // intervalText
            // 
            this.intervalText.Location = new System.Drawing.Point(248, 218);
            this.intervalText.Name = "intervalText";
            this.intervalText.Size = new System.Drawing.Size(84, 22);
            this.intervalText.TabIndex = 9;
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(619, 90);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(96, 63);
            this.buttonGo.TabIndex = 10;
            this.buttonGo.Text = "GO";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // CryptoRuleTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.intervalText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.symbolText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.thresholdText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.retentionText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.filePathTextBox);
            this.Name = "CryptoRuleTester";
            this.Text = "`";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox retentionText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox thresholdText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox symbolText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox intervalText;
        private System.Windows.Forms.Button buttonGo;
    }
}

