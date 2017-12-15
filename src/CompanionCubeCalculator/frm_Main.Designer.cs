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
            this.Txt_Equation = new System.Windows.Forms.TextBox();
            this.txt_max = new System.Windows.Forms.TextBox();
            this.btn_go = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuStrip_File = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.Lbl_EquationHeader = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
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
            // Txt_Equation
            // 
            this.Txt_Equation.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_Equation.Location = new System.Drawing.Point(12, 58);
            this.Txt_Equation.Name = "Txt_Equation";
            this.Txt_Equation.Size = new System.Drawing.Size(446, 26);
            this.Txt_Equation.TabIndex = 1;
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
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStrip_File});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(470, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuStrip_File
            // 
            this.MenuStrip_File.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.MenuStrip_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.MenuStrip_File.Name = "MenuStrip_File";
            this.MenuStrip_File.Size = new System.Drawing.Size(37, 20);
            this.MenuStrip_File.Text = "File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Load";
            // 
            // Lbl_EquationHeader
            // 
            this.Lbl_EquationHeader.AutoSize = true;
            this.Lbl_EquationHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_EquationHeader.Location = new System.Drawing.Point(12, 35);
            this.Lbl_EquationHeader.Name = "Lbl_EquationHeader";
            this.Lbl_EquationHeader.Size = new System.Drawing.Size(81, 20);
            this.Lbl_EquationHeader.TabIndex = 5;
            this.Lbl_EquationHeader.Text = "Equation";
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 389);
            this.Controls.Add(this.Lbl_EquationHeader);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.txt_max);
            this.Controls.Add(this.Txt_Equation);
            this.Controls.Add(this.txt_UserFeedback);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frm_Main";
            this.Text = "CompanionCubeCalculator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_UserFeedback;
        private System.Windows.Forms.TextBox Txt_Equation;
        private System.Windows.Forms.TextBox txt_max;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuStrip_File;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Label Lbl_EquationHeader;
    }
}

