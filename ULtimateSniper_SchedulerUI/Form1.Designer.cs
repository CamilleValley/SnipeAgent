namespace ULtimateSniper_SchedulerUI
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBoxSnipe = new System.Windows.Forms.CheckBox();
            this.checkBoxBidOptimizer = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(183, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(183, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBoxSnipe
            // 
            this.checkBoxSnipe.AutoSize = true;
            this.checkBoxSnipe.Location = new System.Drawing.Point(65, 119);
            this.checkBoxSnipe.Name = "checkBoxSnipe";
            this.checkBoxSnipe.Size = new System.Drawing.Size(93, 17);
            this.checkBoxSnipe.TabIndex = 2;
            this.checkBoxSnipe.Text = "Snipes enable";
            this.checkBoxSnipe.UseVisualStyleBackColor = true;
            // 
            // checkBoxBidOptimizer
            // 
            this.checkBoxBidOptimizer.AutoSize = true;
            this.checkBoxBidOptimizer.Location = new System.Drawing.Point(65, 155);
            this.checkBoxBidOptimizer.Name = "checkBoxBidOptimizer";
            this.checkBoxBidOptimizer.Size = new System.Drawing.Size(122, 17);
            this.checkBoxBidOptimizer.TabIndex = 3;
            this.checkBoxBidOptimizer.Text = "Bid Optimizer enable";
            this.checkBoxBidOptimizer.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.checkBoxBidOptimizer);
            this.Controls.Add(this.checkBoxSnipe);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBoxSnipe;
        private System.Windows.Forms.CheckBox checkBoxBidOptimizer;
    }
}

