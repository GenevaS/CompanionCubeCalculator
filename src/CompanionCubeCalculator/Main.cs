/*
 * Main
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/11/30
 * Starts the GUI program.
 * ---------------------------------------------------------------------
 */

using System;
using System.Windows.Forms;

namespace CompanionCubeCalculator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frm_Main());
            
            return;
        }
    }
}
