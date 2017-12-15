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
        public static bool Initialize()
        {
            bool success = EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators());
            if (success)
            {
                frm_Main.UpdateLog("Parser initialized." + System.Environment.NewLine);
            }
            
            return success;
        }

        public static string[] ControlFile(string fileName)
        {
            string[] inputs = Input.ReadFile(fileName);

            return CalculateRange(inputs[0], inputs[1]);
        }

        public static string[] ControlDirect(string eq, string variables)
        {
            return CalculateRange(eq, variables);
        }

        private static string[] CalculateRange(string rawEquation, string variables)
        {
            bool success = Consolidate.ConvertAndCheckInputs(rawEquation, variables, Solver.GetValidOperators(), Solver.GetValidTerminators(), Input.GetLineDelimiter(), Input.GetFieldDelimiter());
            string[] results = null;

            if (success)
            {
                EquationStruct eq = Consolidate.GetEquationStruct();
                IntervalStruct[] intervals = Consolidate.GetIntervalStructList();
                IntervalStruct range = Solver.FindRange(eq, intervals);
                results = new string[] { Output.PrintInterval(range, false), Output.PrintEquationTree(eq) };
            }

            return results;
        }
    }
}
