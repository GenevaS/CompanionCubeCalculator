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
                string testVar = "x2";
                EquationStruct testNode = EquationConversion.MakeEquationTree(testVar);
                UpdateLog(PrintEquation(testNode) + Environment.NewLine);
                UpdateLog("Variable Equation -> Node value = " + testNode.GetVariableName() + ", node type = " + testNode.GetOperator() + ", Variable list = ");
                string[] vars = EquationConversion.GetVariableList();
                foreach (string var in vars)
                {
                    UpdateLog(var + ", ");
                }
                UpdateLog(System.Environment.NewLine + System.Environment.NewLine);

                EquationStruct testNode2 = new EquationStruct("+", "", new EquationStruct("+", "", testNode, testNode), testNode);
                UpdateLog(PrintEquation(testNode2) + Environment.NewLine);

                EquationStruct testParse = EquationConversion.MakeEquationTree("x1^x2+x3*x4-x5+4x");
                UpdateLog(PrintEquation(testParse) + Environment.NewLine);

                EquationStruct testParse2 = EquationConversion.MakeEquationTree("42");
                UpdateLog(PrintEquation(testParse2) + Environment.NewLine);

                UpdateLog(PrintEquation(new EquationStruct("neg", "", testNode, null)) + Environment.NewLine);
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
