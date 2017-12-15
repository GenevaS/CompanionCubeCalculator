using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            if (!ControlFlow.Initialize())
            { 
                UpdateLog("Error: Program could not be initialized.");
                Txt_Equation.Enabled = false;
                Grid_Vars.Enabled = false;
            }

            Lbl_RangeValue.Text = "-";

            Txt_Log.Text = logMessages;
        }
        /*
                private static string PrintEquation(EquationStruct node)
                {
                    string equation = "";

                    if (node.GetLeftOperand() == null)
                    {
                        equation = node.GetOperator() + ": " + node.GetVariableName();
                    }
                    else if (node.GetRightOperand() == null)
                    {
                        equation = node.GetOperator() + "(" + PrintEquation(node.GetLeftOperand()) + ")";
                    }
                    else
                    {
                        equation = node.GetOperator() + "(" + PrintEquation(node.GetLeftOperand()) + ", " + PrintEquation(node.GetRightOperand()) + ")";
                    }

                    return equation;
                }*/

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
                UpdateLog("Error: Equation field is empty. Please enter an equation into the Equation field to proceed." + System.Environment.NewLine);
            }

            

            Txt_Log.Text = logMessages;

            return;
        }

        private void Btn_Calculate_Click(object sender, EventArgs e)
        {
            string equation = Input.RemoveWhitespace(Txt_Equation.Text, false);
            string variables = "";

            foreach (DataGridViewRow var in Grid_Vars.Rows)
            {
                if (var.Cells[0].Value != null)
                {
                    if (var.Cells[0].Value.ToString() != "" && var.Cells[1].Value.ToString() != "" && var.Cells[2].Value.ToString() != "")
                    {
                        variables += var.Cells[0].Value.ToString() + Input.GetFieldDelimiter() + var.Cells[1].Value.ToString() + Input.GetFieldDelimiter() + var.Cells[2].Value.ToString() + Input.GetLineDelimiter();
                    }
                }
                
            }

            string[] results = ControlFlow.ControlDirect(equation, variables);
            if(results != null)
            {
                Lbl_RangeValue.Text = results[0];
                Txt_DisplayTree.Text = results[1];
            }

            Txt_Log.Text = logMessages;

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
    }
}
