namespace RuleTester
{
    partial class RuleTester
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
            this.LoadConfiguration_Button = new System.Windows.Forms.Button();
            this.ConfigFile_textBox = new System.Windows.Forms.TextBox();
            this.Browse_button = new System.Windows.Forms.Button();
            this.Execute_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DataFile_textBox = new System.Windows.Forms.TextBox();
            this.BrowseData_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LoadConfiguration_Button
            // 
            this.LoadConfiguration_Button.Location = new System.Drawing.Point(609, 105);
            this.LoadConfiguration_Button.Name = "LoadConfiguration_Button";
            this.LoadConfiguration_Button.Size = new System.Drawing.Size(105, 50);
            this.LoadConfiguration_Button.TabIndex = 0;
            this.LoadConfiguration_Button.Text = "Load Configuration";
            this.LoadConfiguration_Button.UseVisualStyleBackColor = true;
            this.LoadConfiguration_Button.Click += new System.EventHandler(this.LoadConfiguration_Button_Click);
            // 
            // ConfigFile_textBox
            // 
            this.ConfigFile_textBox.Location = new System.Drawing.Point(59, 76);
            this.ConfigFile_textBox.Name = "ConfigFile_textBox";
            this.ConfigFile_textBox.Size = new System.Drawing.Size(527, 22);
            this.ConfigFile_textBox.TabIndex = 1;
            this.ConfigFile_textBox.Text = "c:\\Crypto\\RuleConfig.js";
            // 
            // Browse_button
            // 
            this.Browse_button.Location = new System.Drawing.Point(609, 76);
            this.Browse_button.Name = "Browse_button";
            this.Browse_button.Size = new System.Drawing.Size(105, 23);
            this.Browse_button.TabIndex = 2;
            this.Browse_button.Text = "Browse";
            this.Browse_button.UseVisualStyleBackColor = true;
            this.Browse_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // Execute_button
            // 
            this.Execute_button.Location = new System.Drawing.Point(609, 248);
            this.Execute_button.Name = "Execute_button";
            this.Execute_button.Size = new System.Drawing.Size(105, 53);
            this.Execute_button.TabIndex = 3;
            this.Execute_button.Text = "GO";
            this.Execute_button.UseVisualStyleBackColor = true;
            this.Execute_button.Click += new System.EventHandler(this.Execute_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Configuration File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Data File";
            // 
            // DataFile_textBox
            // 
            this.DataFile_textBox.Location = new System.Drawing.Point(59, 219);
            this.DataFile_textBox.Name = "DataFile_textBox";
            this.DataFile_textBox.Size = new System.Drawing.Size(527, 22);
            this.DataFile_textBox.TabIndex = 6;
            // 
            // BrowseData_button
            // 
            this.BrowseData_button.Location = new System.Drawing.Point(609, 219);
            this.BrowseData_button.Name = "BrowseData_button";
            this.BrowseData_button.Size = new System.Drawing.Size(105, 23);
            this.BrowseData_button.TabIndex = 7;
            this.BrowseData_button.Text = "Browse";
            this.BrowseData_button.UseVisualStyleBackColor = true;
            this.BrowseData_button.Click += new System.EventHandler(this.BrowseData_button_Click);
            // 
            // RuleTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BrowseData_button);
            this.Controls.Add(this.DataFile_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Execute_button);
            this.Controls.Add(this.Browse_button);
            this.Controls.Add(this.ConfigFile_textBox);
            this.Controls.Add(this.LoadConfiguration_Button);
            this.Name = "RuleTester";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadConfiguration_Button;
        private System.Windows.Forms.TextBox ConfigFile_textBox;
        private System.Windows.Forms.Button Browse_button;
        private System.Windows.Forms.Button Execute_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DataFile_textBox;
        private System.Windows.Forms.Button BrowseData_button;
    }
}

