/*
 * GUI Control Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/15
 * This module links GUI widgets to the different modules of the 
 * Companion Cube Calculator.
 * ---------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CompanionCubeCalculator
{
    public partial class frm_Main : Form
    {
        private static string logMessages = "";

        public frm_Main()
        {
            InitializeComponent();
        }

        /*
         * The UpdateLog function is used to collect exception messages
         * and any other process messages that should be communicated to
         * the user.
         */
        public static void UpdateLog(string logMessage)
        {
            logMessages += logMessage;
            return;
        }

        private static void ClearLog()
        {
            logMessages = "";
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            if (!ControlFlow.Initialize())
            { 
                UpdateLog("Error: Program could not be initialized.");
                Txt_Equation.Enabled = false;
                Grid_Vars.Enabled = false;
            }

            Lbl_RangeValue.Text = "-";

            Txt_Log.AppendText(logMessages);
            ClearLog();
        }

        private void Txt_Equation_TextChanged(object sender, EventArgs e)
        {
            if (Input.RemoveWhitespace(Txt_Equation.Text, false) != "")
            {
                Btn_ExtractVars.Enabled = true;
            }
            else
            {
                Btn_ExtractVars.Enabled = false;
            }

            return;
        }

        private void Btn_ExtractVars_Click(object sender, EventArgs e)
        {
            string eq = Input.RemoveWhitespace(Txt_Equation.Text, false);
            string[] variables;
            string[] varTableRow;

            if (eq != "")
            {
                EquationConversion.MakeEquationTree(eq);
                variables = EquationConversion.GetVariableList();

                if (variables.Length > 0)
                {
                    varTableRow = new string[] { "", "", "" };
                    foreach (string varName in variables)
                    {
                        varTableRow[0] = varName;
                        Grid_Vars.Rows.Add(varTableRow);
                    }
                }
                else
                {
                    Btn_Calculate.Enabled = true;
                }
            }
            else
            {
                UpdateLog("Error: Equation field is empty. Please enter an equation into the Equation field to proceed." + Environment.NewLine);
            }

            Txt_Log.AppendText(logMessages);
            ClearLog();

            return;
        }

        private void Btn_Calculate_Click(object sender, EventArgs e)
        {
            string equation = Input.RemoveWhitespace(Txt_Equation.Text, false);
            string variables = "";
            List<string> varNames = new List<string>();

            Grid_Vars.AllowUserToAddRows = false;
            foreach (DataGridViewRow var in Grid_Vars.Rows)
            {

                if (var.Cells[0].Value != null && var.Cells[1].Value != null && var.Cells[2].Value != null)
                {
                    if (var.Cells[0].Value.ToString() != "" && var.Cells[1].Value.ToString() != "" && var.Cells[2].Value.ToString() != "")
                    {
                        if (!varNames.Contains(Regex.Replace(var.Cells[0].Value.ToString(), @"\s+", "")))
                        {
                            variables += var.Cells[0].Value.ToString() + Regex.Unescape(Input.GetFieldDelimiter()) + var.Cells[1].Value.ToString() + Regex.Unescape(Input.GetFieldDelimiter()) + var.Cells[2].Value.ToString() + Regex.Unescape(Input.GetLineDelimiter());
                            varNames.Add(Regex.Replace(var.Cells[0].Value.ToString(), @"\s+", ""));
                        }
                        else
                        {
                            UpdateLog("Warning: Found duplicate variable name (" + Regex.Replace(var.Cells[0].Value.ToString(), @"\s+", "") + "). Removing from the list." + Environment.NewLine);
                            Grid_Vars.AllowUserToAddRows = false;
                            Grid_Vars.Rows.Remove(var);
                            Grid_Vars.AllowUserToAddRows = true;
                        }
                    }
                }
                else
                {
                    Grid_Vars.Rows.Remove(var);
                }
                
            }
            Grid_Vars.AllowUserToAddRows = true;

            string[] results = ControlFlow.ControlDirect(equation, variables);
            if(results != null)
            {

                Grp_Outputs.Enabled = true;
                Lbl_RangeHeader.Enabled = true;
                Lbl_RangeValue.Enabled = true;
                Lbl_TreeHeader.Enabled = true;
                Txt_DisplayTree.Enabled = true;

                Lbl_RangeValue.Text = results[0];
                Txt_DisplayTree.Text = results[1];

                Btn_ExtractVars.Enabled = false;

                
                string[,] varInfo = ControlFlow.GetVariableInfo();
                if (varInfo != null)
                {
                    ResetVarTable();
                    DataGridViewRow varRow;
                    for (int i = 0; i < varInfo.GetLength(0); i++)
                    {
                        varRow = (DataGridViewRow)Grid_Vars.Rows[0].Clone();
                        varRow.Cells[0].Value = varInfo[i, 0];
                        varRow.Cells[1].Value = varInfo[i, 1];
                        varRow.Cells[2].Value = varInfo[i, 2];
                        Grid_Vars.Rows.Add(varRow);
                    }
                }
            }

            Txt_Log.AppendText(logMessages);
            ClearLog();

            return;
        }

        private void Grid_Vars_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool enableCalculate = true;

            foreach (DataGridViewRow var in Grid_Vars.Rows)
            {
                if (var.Cells[0].Value != null && var.Cells[1].Value != null && var.Cells[2].Value != null)
                {
                    if (var.Cells[0].Value.ToString() == "" || var.Cells[1].Value.ToString() == "" && var.Cells[2].Value.ToString() == "")
                    {
                        enableCalculate = false;
                    }
                }
                
            }

            if (enableCalculate)
            {
                Btn_Calculate.Enabled = true;
            }
            else
            {
                Btn_Calculate.Enabled = false;
            }

            return;
        }

        private void MenuItem_Load_Click(object sender, EventArgs e)
        {
            Reset();

            OpenFileDialog getUserFile = new OpenFileDialog();
            getUserFile.Title = "Load Equation Data";
            getUserFile.Multiselect = false;
            getUserFile.CheckFileExists = true;
            getUserFile.CheckPathExists = true;

            string[] fileExtensions = Input.GetValidFileTypes();
            if(fileExtensions != null)
            {
                string filter = fileExtensions[0] + "|" + fileExtensions[0];
                for (int i = 1; i < fileExtensions.Length; i++)
                {
                    filter += "|" + fileExtensions[i] + "|" + fileExtensions[i];
                }

                getUserFile.Filter = filter;
            }

            if (getUserFile.ShowDialog() == DialogResult.OK)
            {
                UpdateLog("Reading from: " + getUserFile.FileName + Environment.NewLine);

                string[] fileContents = ControlFlow.ControlFile(getUserFile.FileName);
                if (fileContents != null)
                {
                    Txt_Equation.Text = fileContents[0];

                    string[] results = ControlFlow.ControlDirect(fileContents[0], fileContents[1]);

                    if (results != null)
                    {
                        Grp_Outputs.Enabled = true;
                        Lbl_RangeHeader.Enabled = true;
                        Lbl_RangeValue.Enabled = true;
                        Lbl_TreeHeader.Enabled = true;
                        Txt_DisplayTree.Enabled = true;

                        Lbl_RangeValue.Text = results[0];
                        Txt_DisplayTree.Text = results[1];

                        Btn_ExtractVars.Enabled = false;                        
                    }

                    string[,] varInfo = ControlFlow.GetVariableInfo();
                    if (varInfo != null)
                    {
                        ResetVarTable();
                        DataGridViewRow varRow;
                        for (int i = 0; i < varInfo.GetLength(0); i++)
                        {
                            varRow = (DataGridViewRow)Grid_Vars.Rows[0].Clone();
                            varRow.Cells[0].Value = varInfo[i, 0];
                            varRow.Cells[1].Value = varInfo[i, 1];
                            varRow.Cells[2].Value = varInfo[i, 2];
                            Grid_Vars.Rows.Add(varRow);
                        }
                    }
                }
                
                Txt_Log.AppendText(logMessages);
                ClearLog();
            }
            else
            {
                UpdateLog("Error: Open File Dialog was closed." + Environment.NewLine);
            }

            Txt_Log.AppendText(logMessages);
            ClearLog();

            return;
        }

        private void Reset()
        {
            Txt_Equation.Text = "";

            ResetVarTable();

            Grp_Outputs.Enabled = false;
            Lbl_RangeHeader.Enabled = false;
            Lbl_RangeValue.Enabled = false;
            Lbl_TreeHeader.Enabled = false;
            Txt_DisplayTree.Enabled = false;

            Lbl_RangeValue.Text = "-";
            Txt_DisplayTree.Text = "";

            return;
        }

        private void ResetVarTable()
        {
            Grid_Vars.AllowUserToAddRows = false;
            Grid_Vars.Rows.Clear();
            Grid_Vars.AllowUserToAddRows = true;

            return;
        }
    }
}