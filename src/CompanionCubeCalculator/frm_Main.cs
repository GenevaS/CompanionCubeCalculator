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
            string[] fileContents = Input.ReadFile(@"C:\Users\smith\Desktop\VisualStudioProjects\CompanionCubeCalculator\CompanionCubeCalculator\test.txt");

            if (fileContents != null)
            {
                UpdateLog(fileContents[0] + Environment.NewLine);
                UpdateLog(fileContents[1] + Environment.NewLine);
            }
            else
            {
                UpdateLog("File read returned null." + Environment.NewLine);
            }

            string[] test = fileContents[1].Split('\n');
            UpdateLog(test.Length.ToString());
            

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
