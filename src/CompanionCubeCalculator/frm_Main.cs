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
            string varToken = EquationConversion.GetVariableToken();
            EquationStruct equation = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct("-", "", new EquationStruct(varToken, "y", null, null), new EquationStruct(varToken, "z", null, null)));
            IntervalStruct[] intervals = new IntervalStruct[] { new IntervalStruct("x", 2, 4, true, true), new IntervalStruct("y", 3, 5, true, true), new IntervalStruct("z", 2, 2, true, true) };

            IntervalStruct range = Solver.FindRange(equation, intervals);

            UpdateLog("Results: min = " + range.GetMinBound().ToString() + ", max = " + range.GetMaxBound().ToString() + System.Environment.NewLine);

            UpdateLog(Output.PrintEquationTree(equation));

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
