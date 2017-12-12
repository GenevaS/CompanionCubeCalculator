/*
 * Interval Conversion Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/12
 * Corresponds to IntervalConversion MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CompanionCubeCalculator
{
    public static class IntervalConversion
    {
        private static char lineDelimiter = '\n';
        private static char fieldDelimiter = ',';

        /* PARSING FUNCTION */
        public static IntervalStruct[] ConvertToIntervals(string varList)
        {
            List<IntervalStruct> intervals = new List<IntervalStruct>();
            IntervalStruct currentInterval;
            string[] vars = varList.Split(lineDelimiter);
            string[] currentVar;

            for(int i = 0; i < vars.Length; i++)
            {
                // Split the string on the delimiter (',') and remove all whitespaces
                currentVar = vars[i].Split(fieldDelimiter);
                for(int j = 0; j < currentVar.Length; j++)
                {
                    currentVar[j] = Regex.Replace(currentVar[j], @"\s+", "");
                }

                // There are the right number of fields -> add to list if the interval != null
                if(currentVar.Length == 3)
                {
                    currentInterval = MakeInterval(currentVar[0], currentVar[1], currentVar[2]);
                    if(currentInterval != null)
                    {
                        intervals.Add(currentInterval);
                    }
                }
                // There is one missing field, so add an empty placeholder and let the MakeInterval function
                // handle the rest -> add to list if the interval != null
                else if (currentVar.Length == 2)
                {
                    currentInterval = MakeInterval(currentVar[0], currentVar[1], "");
                    if (currentInterval != null)
                    {
                        intervals.Add(currentInterval);
                    }
                }
                // There are too many fields -> skip and notify the user
                else if(currentVar.Length > 3)
                {
                    frm_Main.UpdateLog("Error: Encountered a variable with more than three fields (Line " + (i + 1) + "). Skipping line." + System.Environment.NewLine);
                }
                // There is one or fewer fields on the current line -> skip and notify the user
                else
                {
                    frm_Main.UpdateLog("Error: No fields found for variable (Line " + (i + 1) + "). Skipping line." + System.Environment.NewLine);
                }
            }

            return intervals.ToArray();
        }

        /* CONVERSION FUNCTION */
        private static IntervalStruct MakeInterval(string varName, string min, string max)
        {
            IntervalStruct iv = null;
            bool proceed = true;
            double cMin = 0;
            double cMax = 0;

            // Check that a non-empty variable name was provided
            if (CheckVarName(varName) && CheckBoundExistence(min, max))
            {
                // Try to convert the min to a string
                try
                {
                    if (min == "")
                    {
                        frm_Main.UpdateLog("Warning: No minimum interval bound given. Setting it to the same value as the maximum bound." + System.Environment.NewLine);
                        min = max;
                    }
                    cMin = System.Convert.ToDouble(min);
                 }
                catch (System.FormatException)
                {
                    frm_Main.UpdateLog("Error: The string provided for the minimum bound cannot be converted to a real number." + System.Environment.NewLine);
                    proceed = false;
                }

                // Try to convert the max to a string
                try
                {
                    if (max == "")
                    {
                        frm_Main.UpdateLog("Warning: No maximum interval bound given. Setting it to the same value as the minimum bound." + System.Environment.NewLine);
                        max = min;
                    }
                    cMax = System.Convert.ToDouble(max);
                }
                catch (System.FormatException)
                {
                    frm_Main.UpdateLog("Error: The string provided for the maximum bound cannot be converted to a real number." + System.Environment.NewLine);
                    proceed = false;
                }

                // If you have reached this point, all the inputs are available to create an IntervalStruct object
                if(proceed)
                {
                    iv = new IntervalStruct(varName, cMin, cMax);
                }
            }  

            return iv;
        }

        /* HELPER METHODS */
        private static bool CheckVarName(string varName)
        {
            bool proceed = true;

            // If the variable name is empty or a value, display an error and tell calling method not to continue
            try
            {
                System.Convert.ToDouble(varName);
                frm_Main.UpdateLog("Error: Intervals must have an associated variable name." + System.Environment.NewLine);
                proceed = false;
            }
            catch(System.FormatException)
            { 
                if(varName == "")
                {
                    frm_Main.UpdateLog("Error: Intervals must have an associated variable name." + System.Environment.NewLine);
                    proceed = false;
                }
            }

            return proceed;
        }

        private static bool CheckBoundExistence(string min, string max)
        {
            bool proceed = true;

            // If both of the provided bounds are empty, display an error and tell the calling method not to proceed
            if ((min == "") && (max == ""))
            {
                frm_Main.UpdateLog("Error: No values provided for either interval bound." + System.Environment.NewLine);
                proceed = false;
            }

            return proceed;
        }
    }
}