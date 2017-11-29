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
            IntervalStruct interval = new IntervalStruct("x", 4.2, 3.0);
            UpdateLog(interval.GetVariableName() + " = ");
            UpdateLog(interval.GetMinBound().ToString() + ", ");
            UpdateLog(interval.GetMaxBound().ToString() + Environment.NewLine);
            txt_UserFeedback.Text = logMessages;
        }
    }
}
