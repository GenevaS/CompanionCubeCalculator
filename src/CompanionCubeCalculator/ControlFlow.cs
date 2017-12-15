/*
 * Control Flow Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/15
 * Corresponds to Control Flow Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

namespace CompanionCubeCalculator
{
    public static class ControlFlow
    {
        public static void ControlFile(string fileName)
        {
            string[] inputs = Input.ReadFile(fileName);
            frm_Main.UpdateLog(inputs[0] + System.Environment.NewLine);
            frm_Main.UpdateLog(inputs[1]);

            bool success = Consolidate.ConvertAndCheckInputs(inputs[0], inputs[1], Solver.GetValidOperators(), Solver.GetValidTerminators(), Input.GetLineDelimiter(), Input.GetFieldDelimiter());

            if(success)
            {
                EquationStruct eq = Consolidate.GetEquationStruct();
                IntervalStruct[] intervals = Consolidate.GetIntervalStructList();
                IntervalStruct range = Solver.FindRange(eq, intervals);
                frm_Main.UpdateLog("Range" + Output.PrintInterval(range) + System.Environment.NewLine);
                frm_Main.UpdateLog("Equation Tree: " + System.Environment.NewLine + "---------------------------" + System.Environment.NewLine);
                frm_Main.UpdateLog(Output.PrintEquationTree(eq) + System.Environment.NewLine);
                foreach(IntervalStruct iv in intervals)
                {
                    frm_Main.UpdateLog(Output.PrintInterval(iv) + System.Environment.NewLine);
                }
            }

            return;
        }
    }
}
