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

        private void btn_go_Click(object sender, EventArgs e)
        {
            if(EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators()))
            {
                EquationStruct testParse = EquationConversion.MakeEquationTree("x+y");
                if(testParse != null)
                {
                    UpdateLog(Environment.NewLine + PrintEquation(testParse) + Environment.NewLine);
                    string[] vars = EquationConversion.GetVariableList();
                    foreach (string v in vars)
                    {
                        UpdateLog(v + ",");
                    }
                    bool success = Consolidate.ConvertAndCheckInputs("x+y", "x,2,3\ny,4,5", Solver.GetValidOperators(), Solver.GetValidTerminators());
                    UpdateLog(success.ToString());
                }
                else
                {
                    UpdateLog("Equation is null -> cannot print results.");
                }
                
            }

            

            txt_UserFeedback.Text = logMessages;
        }

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
        }
    }
}
