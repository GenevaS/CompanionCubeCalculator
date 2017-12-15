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
            this.Txt_Log = new System.Windows.Forms.TextBox();
            this.Txt_Equation = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuStrip_File = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem_Load = new System.Windows.Forms.ToolStripMenuItem();
            this.Lbl_EquationHeader = new System.Windows.Forms.Label();
            this.Lbl_LogHeader = new System.Windows.Forms.Label();
            this.Lbl_Split = new System.Windows.Forms.Label();
            this.btn_go = new System.Windows.Forms.Button();
            this.Grid_Vars = new System.Windows.Forms.DataGridView();
            this.Col_VarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_MinBound = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_MaxBound = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Btn_Calculate = new System.Windows.Forms.Button();
            this.Btn_ExtractVars = new System.Windows.Forms.Button();
            this.Txt_DisplayTree = new System.Windows.Forms.TextBox();
            this.Lbl_TreeHeader = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Vars)).BeginInit();
            this.SuspendLayout();
            // 
            // Txt_Log
            // 
            this.Txt_Log.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_Log.Location = new System.Drawing.Point(12, 387);
            this.Txt_Log.Multiline = true;
            this.Txt_Log.Name = "Txt_Log";
            this.Txt_Log.ReadOnly = true;
            this.Txt_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Txt_Log.Size = new System.Drawing.Size(961, 80);
            this.Txt_Log.TabIndex = 0;
            // 
            // Txt_Equation
            // 
            this.Txt_Equation.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_Equation.Location = new System.Drawing.Point(69, 62);
            this.Txt_Equation.Name = "Txt_Equation";
            this.Txt_Equation.Size = new System.Drawing.Size(343, 26);
            this.Txt_Equation.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuStrip_File});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(985, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuStrip_File
            // 
            this.MenuStrip_File.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.MenuStrip_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItem_Load});
            this.MenuStrip_File.Name = "MenuStrip_File";
            this.MenuStrip_File.Size = new System.Drawing.Size(37, 20);
            this.MenuStrip_File.Text = "File";
            // 
            // MenuItem_Load
            // 
            this.MenuItem_Load.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.MenuItem_Load.Name = "MenuItem_Load";
            this.MenuItem_Load.Size = new System.Drawing.Size(152, 22);
            this.MenuItem_Load.Text = "Load";
            // 
            // Lbl_EquationHeader
            // 
            this.Lbl_EquationHeader.AutoSize = true;
            this.Lbl_EquationHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_EquationHeader.Location = new System.Drawing.Point(12, 65);
            this.Lbl_EquationHeader.Name = "Lbl_EquationHeader";
            this.Lbl_EquationHeader.Size = new System.Drawing.Size(55, 20);
            this.Lbl_EquationHeader.TabIndex = 5;
            this.Lbl_EquationHeader.Text = "f(x) = ";
            // 
            // Lbl_LogHeader
            // 
            this.Lbl_LogHeader.AutoSize = true;
            this.Lbl_LogHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_LogHeader.Location = new System.Drawing.Point(13, 368);
            this.Lbl_LogHeader.Name = "Lbl_LogHeader";
            this.Lbl_LogHeader.Size = new System.Drawing.Size(34, 16);
            this.Lbl_LogHeader.TabIndex = 6;
            this.Lbl_LogHeader.Text = "Log";
            // 
            // Lbl_Split
            // 
            this.Lbl_Split.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Lbl_Split.Location = new System.Drawing.Point(12, 362);
            this.Lbl_Split.Name = "Lbl_Split";
            this.Lbl_Split.Size = new System.Drawing.Size(961, 2);
            this.Lbl_Split.TabIndex = 7;
            // 
            // btn_go
            // 
            this.btn_go.Location = new System.Drawing.Point(421, 321);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(75, 23);
            this.btn_go.TabIndex = 3;
            this.btn_go.Text = "button1";
            this.btn_go.UseVisualStyleBackColor = true;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // Grid_Vars
            // 
            this.Grid_Vars.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid_Vars.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_VarName,
            this.Col_MinBound,
            this.Col_MaxBound});
            this.Grid_Vars.Location = new System.Drawing.Point(69, 160);
            this.Grid_Vars.Name = "Grid_Vars";
            this.Grid_Vars.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Grid_Vars.Size = new System.Drawing.Size(343, 150);
            this.Grid_Vars.TabIndex = 8;
            // 
            // Col_VarName
            // 
            this.Col_VarName.HeaderText = "Variable";
            this.Col_VarName.Name = "Col_VarName";
            // 
            // Col_MinBound
            // 
            this.Col_MinBound.HeaderText = "Minimum";
            this.Col_MinBound.Name = "Col_MinBound";
            // 
            // Col_MaxBound
            // 
            this.Col_MaxBound.HeaderText = "Maximum";
            this.Col_MaxBound.Name = "Col_MaxBound";
            // 
            // Btn_Calculate
            // 
            this.Btn_Calculate.Enabled = false;
            this.Btn_Calculate.Location = new System.Drawing.Point(463, 78);
            this.Btn_Calculate.Name = "Btn_Calculate";
            this.Btn_Calculate.Size = new System.Drawing.Size(75, 202);
            this.Btn_Calculate.TabIndex = 9;
            this.Btn_Calculate.Text = "Calculate Range";
            this.Btn_Calculate.UseVisualStyleBackColor = true;
            // 
            // Btn_ExtractVars
            // 
            this.Btn_ExtractVars.Enabled = false;
            this.Btn_ExtractVars.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_ExtractVars.Location = new System.Drawing.Point(131, 111);
            this.Btn_ExtractVars.Name = "Btn_ExtractVars";
            this.Btn_ExtractVars.Size = new System.Drawing.Size(218, 23);
            this.Btn_ExtractVars.TabIndex = 10;
            this.Btn_ExtractVars.Text = "AutoGenerate Variable Names";
            this.Btn_ExtractVars.UseVisualStyleBackColor = true;
            // 
            // Txt_DisplayTree
            // 
            this.Txt_DisplayTree.BackColor = System.Drawing.SystemColors.Control;
            this.Txt_DisplayTree.Enabled = false;
            this.Txt_DisplayTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_DisplayTree.Location = new System.Drawing.Point(571, 195);
            this.Txt_DisplayTree.Multiline = true;
            this.Txt_DisplayTree.Name = "Txt_DisplayTree";
            this.Txt_DisplayTree.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Txt_DisplayTree.Size = new System.Drawing.Size(402, 149);
            this.Txt_DisplayTree.TabIndex = 11;
            // 
            // Lbl_TreeHeader
            // 
            this.Lbl_TreeHeader.AutoSize = true;
            this.Lbl_TreeHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_TreeHeader.Location = new System.Drawing.Point(568, 174);
            this.Lbl_TreeHeader.Name = "Lbl_TreeHeader";
            this.Lbl_TreeHeader.Size = new System.Drawing.Size(100, 18);
            this.Lbl_TreeHeader.TabIndex = 12;
            this.Lbl_TreeHeader.Text = "Equation Tree";
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 479);
            this.Controls.Add(this.Lbl_TreeHeader);
            this.Controls.Add(this.Txt_DisplayTree);
            this.Controls.Add(this.Btn_ExtractVars);
            this.Controls.Add(this.Btn_Calculate);
            this.Controls.Add(this.Grid_Vars);
            this.Controls.Add(this.Lbl_Split);
            this.Controls.Add(this.Lbl_LogHeader);
            this.Controls.Add(this.Lbl_EquationHeader);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.Txt_Equation);
            this.Controls.Add(this.Txt_Log);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frm_Main";
            this.Text = "CompanionCubeCalculator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid_Vars)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Txt_Log;
        private System.Windows.Forms.TextBox Txt_Equation;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuStrip_File;
        private System.Windows.Forms.ToolStripMenuItem MenuItem_Load;
        private System.Windows.Forms.Label Lbl_EquationHeader;
        private System.Windows.Forms.Label Lbl_LogHeader;
        private System.Windows.Forms.Label Lbl_Split;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.DataGridView Grid_Vars;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_VarName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_MinBound;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_MaxBound;
        private System.Windows.Forms.Button Btn_Calculate;
        private System.Windows.Forms.Button Btn_ExtractVars;
        private System.Windows.Forms.TextBox Txt_DisplayTree;
        private System.Windows.Forms.Label Lbl_TreeHeader;
    }
}

