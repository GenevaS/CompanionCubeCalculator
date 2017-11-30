/*
 * Interval Conversion Module
 * ---------------------------------------------------------------------
 * Updated 2017/11/30
 * Corresponds to IntervalConversion MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

namespace CompanionCubeCalculator
{
    public static class IntervalConversion
    {
        public static IntervalStruct MakeInterval(string varName, string min, string max)
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
                catch (System.InvalidCastException)
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
                catch (System.InvalidCastException)
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

            // If the variable name is empty, display an error and tell calling method
            // not to continue
            if(varName == "")
            { 
                frm_Main.UpdateLog("Error: Intervals must have an associated variable name." + System.Environment.NewLine);
                proceed = false;
            }

            return proceed;
        }

        private static bool CheckBoundExistence(string min, string max)
        {
            bool proceed = true;

            // If both of the provided bounds are empty, display an error and tell the
            // calling method not to proceed
            if (!((min != "") && (max != "")))
            {
                frm_Main.UpdateLog("Error: No values provided for either interval bound." + System.Environment.NewLine);
                proceed = false;
            }

            return proceed;
        }
    }
}
