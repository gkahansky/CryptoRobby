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
            this.retentionMinText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.thresholdMinText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonGo = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.DefaultSLMinText = new System.Windows.Forms.TextBox();
            this.DynamicSLMinText = new System.Windows.Forms.TextBox();
            this.PatternsListBox = new System.Windows.Forms.CheckedListBox();
            this.DynamicSLMaxText = new System.Windows.Forms.TextBox();
            this.DefaultSLMaxText = new System.Windows.Forms.TextBox();
            this.thresholdMaxText = new System.Windows.Forms.TextBox();
            this.retentionMaxText = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SymbolList = new System.Windows.Forms.TextBox();
            this.IntervalList = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.DynamicSLIncText = new System.Windows.Forms.TextBox();
            this.DefaultSLIncText = new System.Windows.Forms.TextBox();
            this.thresholdIncText = new System.Windows.Forms.TextBox();
            this.retentionIncText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(51, 45);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(608, 22);
            this.filePathTextBox.TabIndex = 0;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(683, 45);
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
            this.label1.Location = new System.Drawing.Point(51, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Moving Average Retention";
            // 
            // retentionMinText
            // 
            this.retentionMinText.Location = new System.Drawing.Point(268, 108);
            this.retentionMinText.Name = "retentionMinText";
            this.retentionMinText.Size = new System.Drawing.Size(63, 22);
            this.retentionMinText.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Trend Change Threshold";
            // 
            // thresholdMinText
            // 
            this.thresholdMinText.Location = new System.Drawing.Point(268, 155);
            this.thresholdMinText.Name = "thresholdMinText";
            this.thresholdMinText.Size = new System.Drawing.Size(63, 22);
            this.thresholdMinText.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 303);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Symbol";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 350);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Interval";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(683, 88);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(96, 63);
            this.buttonGo.TabIndex = 16;
            this.buttonGo.Text = "GO";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(51, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(188, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Default Stop Loss Threshold";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(197, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Dynamic Stop Loss Threshold";
            // 
            // DefaultSLMinText
            // 
            this.DefaultSLMinText.Location = new System.Drawing.Point(268, 205);
            this.DefaultSLMinText.Name = "DefaultSLMinText";
            this.DefaultSLMinText.Size = new System.Drawing.Size(63, 22);
            this.DefaultSLMinText.TabIndex = 6;
            // 
            // DynamicSLMinText
            // 
            this.DynamicSLMinText.Location = new System.Drawing.Point(266, 252);
            this.DynamicSLMinText.Name = "DynamicSLMinText";
            this.DynamicSLMinText.Size = new System.Drawing.Size(63, 22);
            this.DynamicSLMinText.TabIndex = 8;
            // 
            // PatternsListBox
            // 
            this.PatternsListBox.FormattingEnabled = true;
            this.PatternsListBox.Location = new System.Drawing.Point(567, 179);
            this.PatternsListBox.Name = "PatternsListBox";
            this.PatternsListBox.Size = new System.Drawing.Size(212, 259);
            this.PatternsListBox.TabIndex = 15;
            this.PatternsListBox.SelectedIndexChanged += new System.EventHandler(this.PatternsListBox_SelectedIndexChanged);
            // 
            // DynamicSLMaxText
            // 
            this.DynamicSLMaxText.Location = new System.Drawing.Point(356, 252);
            this.DynamicSLMaxText.Name = "DynamicSLMaxText";
            this.DynamicSLMaxText.Size = new System.Drawing.Size(63, 22);
            this.DynamicSLMaxText.TabIndex = 9;
            // 
            // DefaultSLMaxText
            // 
            this.DefaultSLMaxText.Location = new System.Drawing.Point(358, 205);
            this.DefaultSLMaxText.Name = "DefaultSLMaxText";
            this.DefaultSLMaxText.Size = new System.Drawing.Size(63, 22);
            this.DefaultSLMaxText.TabIndex = 7;
            // 
            // thresholdMaxText
            // 
            this.thresholdMaxText.Location = new System.Drawing.Point(358, 155);
            this.thresholdMaxText.Name = "thresholdMaxText";
            this.thresholdMaxText.Size = new System.Drawing.Size(63, 22);
            this.thresholdMaxText.TabIndex = 5;
            // 
            // retentionMaxText
            // 
            this.retentionMaxText.Location = new System.Drawing.Point(358, 108);
            this.retentionMaxText.Name = "retentionMaxText";
            this.retentionMaxText.Size = new System.Drawing.Size(63, 22);
            this.retentionMaxText.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(282, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 17);
            this.label7.TabIndex = 105;
            this.label7.Text = "Min";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(373, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 17);
            this.label8.TabIndex = 106;
            this.label8.Text = "Max";
            // 
            // SymbolList
            // 
            this.SymbolList.Location = new System.Drawing.Point(128, 303);
            this.SymbolList.Name = "SymbolList";
            this.SymbolList.Size = new System.Drawing.Size(381, 22);
            this.SymbolList.TabIndex = 10;
            // 
            // IntervalList
            // 
            this.IntervalList.Location = new System.Drawing.Point(128, 345);
            this.IntervalList.Name = "IntervalList";
            this.IntervalList.Size = new System.Drawing.Size(381, 22);
            this.IntervalList.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(564, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 17);
            this.label9.TabIndex = 107;
            this.label9.Text = "Patterns";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(445, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 17);
            this.label10.TabIndex = 108;
            this.label10.Text = "Increment";
            // 
            // DynamicSLIncText
            // 
            this.DynamicSLIncText.Location = new System.Drawing.Point(446, 252);
            this.DynamicSLIncText.Name = "DynamicSLIncText";
            this.DynamicSLIncText.Size = new System.Drawing.Size(63, 22);
            this.DynamicSLIncText.TabIndex = 112;
            // 
            // DefaultSLIncText
            // 
            this.DefaultSLIncText.Location = new System.Drawing.Point(448, 205);
            this.DefaultSLIncText.Name = "DefaultSLIncText";
            this.DefaultSLIncText.Size = new System.Drawing.Size(63, 22);
            this.DefaultSLIncText.TabIndex = 111;
            // 
            // thresholdIncText
            // 
            this.thresholdIncText.Location = new System.Drawing.Point(448, 155);
            this.thresholdIncText.Name = "thresholdIncText";
            this.thresholdIncText.Size = new System.Drawing.Size(63, 22);
            this.thresholdIncText.TabIndex = 110;
            // 
            // retentionIncText
            // 
            this.retentionIncText.Location = new System.Drawing.Point(448, 108);
            this.retentionIncText.Name = "retentionIncText";
            this.retentionIncText.Size = new System.Drawing.Size(63, 22);
            this.retentionIncText.TabIndex = 109;
            // 
            // CryptoRuleTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DynamicSLIncText);
            this.Controls.Add(this.DefaultSLIncText);
            this.Controls.Add(this.thresholdIncText);
            this.Controls.Add(this.retentionIncText);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.IntervalList);
            this.Controls.Add(this.SymbolList);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.DynamicSLMaxText);
            this.Controls.Add(this.DefaultSLMaxText);
            this.Controls.Add(this.thresholdMaxText);
            this.Controls.Add(this.retentionMaxText);
            this.Controls.Add(this.PatternsListBox);
            this.Controls.Add(this.DynamicSLMinText);
            this.Controls.Add(this.DefaultSLMinText);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.thresholdMinText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.retentionMinText);
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
        private System.Windows.Forms.TextBox retentionMinText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox thresholdMinText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox DefaultSLMinText;
        private System.Windows.Forms.TextBox DynamicSLMinText;
        private System.Windows.Forms.CheckedListBox PatternsListBox;
        private System.Windows.Forms.TextBox DynamicSLMaxText;
        private System.Windows.Forms.TextBox DefaultSLMaxText;
        private System.Windows.Forms.TextBox thresholdMaxText;
        private System.Windows.Forms.TextBox retentionMaxText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox SymbolList;
        private System.Windows.Forms.TextBox IntervalList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox DynamicSLIncText;
        private System.Windows.Forms.TextBox DefaultSLIncText;
        private System.Windows.Forms.TextBox thresholdIncText;
        private System.Windows.Forms.TextBox retentionIncText;
    }
}

