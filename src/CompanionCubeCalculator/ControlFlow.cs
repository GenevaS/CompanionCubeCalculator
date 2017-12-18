/*
 * Control Flow Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/18
 * Corresponds to Control Flow Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

using System.Text.RegularExpressions;

namespace CompanionCubeCalculator
{
    public static class ControlFlow
    {
        private static bool hasRun = false;
        private static int successCode = 1;

        public static bool Initialize()
        {
            bool success = Consolidate.Initialize();
            if (success)
            {
                frm_Main.UpdateLog("Parser initialized." + System.Environment.NewLine);
            }
            
            return success;
        }

        public static string ConditionRawInput(string input, bool preserveSpecialChars)
        {
            return Input.RemoveWhitespace(input, preserveSpecialChars);
        }


        public static string[] ControlFile(string fileName)
        {
            string[] inputs = Input.ReadFile(fileName);

            return inputs;
        }

        public static string[] ControlDirect(string eq, string variables)
        {
            return CalculateRange(eq, variables);
        }

        private static string[] CalculateRange(string rawEquation, string variables)
        {
            int success = Consolidate.ConvertAndCheckInputs(rawEquation, variables, Solver.GetValidOperators(), Solver.GetValidTerminators(), Input.GetLineDelimiter(), Input.GetFieldDelimiter());
            string[] results = null;

            if (success == 0)
            {
                EquationStruct eq = Consolidate.GetEquationStruct();
                IntervalStruct[] intervals = Consolidate.GetIntervalStructList();
                IntervalStruct range = Solver.FindRange(eq, intervals);
                if (range != null)
                {
                    results = new string[] { Output.PrintInterval(range, false), Output.PrintEquationTree(eq) };
                    frm_Main.UpdateLog("Range calculated successfully." + System.Environment.NewLine);
                    hasRun = true;
                }
            }
            else
            {
                successCode = success;
            }

            return results;
        }

        /* GETTERS */
        public static int GetSuccessCode()
        {
            return successCode;
        }

        public static string[] GetValidFileTypes()
        {
            return Input.GetValidFileTypes();
        }

        public static string[] GetDelimiters()
        {
            return new string[2] { Regex.Unescape(Input.GetLineDelimiter()), Regex.Unescape(Input.GetFieldDelimiter()) };
        }

        public static string[] ExtractVariables(string eq)
        {
            return Consolidate.ExtractVariablesFromEquation(eq);
        }

        public static string[,] GetVariableInfo()
        {
            string[,] ivInfo = null;

            if (hasRun)
            {
                IntervalStruct[] intervals = Consolidate.GetIntervalStructList();
                if (intervals != null)
                {
                    ivInfo = new string[intervals.Length, 3];

                    for (int i = 0; i < intervals.Length; i++)
                    {
                        ivInfo[i, 0] = intervals[i].GetVariableName();
                        ivInfo[i, 1] = intervals[i].GetMinBound().ToString();
                        ivInfo[i, 2] = intervals[i].GetMaxBound().ToString();
                    }
                }
            }

            return ivInfo;
        }
    }
}
