namespace CompanionCubeCalculator
{
    partial class frm_Main
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
            this.txt_UserFeedback = new System.Windows.Forms.TextBox();
            this.txt_min = new System.Windows.Forms.TextBox();
            this.txt_max = new System.Windows.Forms.TextBox();
            this.btn_go = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_UserFeedback
            // 
            this.txt_UserFeedback.Location = new System.Drawing.Point(12, 297);
            this.txt_UserFeedback.Multiline = true;
            this.txt_UserFeedback.Name = "txt_UserFeedback";
            this.txt_UserFeedback.ReadOnly = true;
            this.txt_UserFeedback.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_UserFeedback.Size = new System.Drawing.Size(446, 80);
            this.txt_UserFeedback.TabIndex = 0;
            // 
            // txt_min
            // 
            this.txt_min.Location = new System.Drawing.Point(23, 182);
            this.txt_min.Name = "txt_min";
            this.txt_min.Size = new System.Drawing.Size(100, 20);
            this.txt_min.TabIndex = 1;
            // 
            // txt_max
            // 
            this.txt_max.Location = new System.Drawing.Point(23, 224);
            this.txt_max.Name = "txt_max";
            this.txt_max.Size = new System.Drawing.Size(100, 20);
            this.txt_max.TabIndex = 2;
            // 
            // btn_go
            // 
            this.btn_go.Location = new System.Drawing.Point(47, 268);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(75, 23);
            this.btn_go.TabIndex = 3;
            this.btn_go.Text = "button1";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 389);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.txt_max);
            this.Controls.Add(this.txt_min);
            this.Controls.Add(this.txt_UserFeedback);
            this.Name = "frm_Main";
            this.Text = "CompanionCubeCalculator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_UserFeedback;
        private System.Windows.Forms.TextBox txt_min;
        private System.Windows.Forms.TextBox txt_max;
        private System.Windows.Forms.Button btn_go;
    }
}

