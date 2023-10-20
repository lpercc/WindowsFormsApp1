namespace WindowsFormsApp1.test
{
    partial class car5_set
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
            this.btncar5set = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btncar5set
            // 
            this.btncar5set.Location = new System.Drawing.Point(243, 182);
            this.btncar5set.Name = "btncar5set";
            this.btncar5set.Size = new System.Drawing.Size(75, 23);
            this.btncar5set.TabIndex = 0;
            this.btncar5set.Text = "设置";
            this.btncar5set.UseVisualStyleBackColor = true;
            this.btncar5set.Click += new System.EventHandler(this.btncar5set_Click);
            // 
            // car5_set
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 276);
            this.Controls.Add(this.btncar5set);
            this.Name = "car5_set";
            this.Text = "天津五维地磁车检器设置";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btncar5set;
    }
}