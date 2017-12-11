/*
 * Variable Consolidation Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/11
 * Corresponds to the Variable Consolidation Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;

namespace CompanionCubeCalculator
{
    public static class Consolidate
    {
        private static EquationStruct equationTreeRoot = null;
        private static List<IntervalStruct> intervalList = new List<IntervalStruct>();

        public static void ConvertAndCheckInputs(string eqString, string varList, OperatorStruct[] operators, string[][] terminators)
        {
            if (!EquationConversion.IsReady())
            {
                EquationConversion.ConfigureParser(operators, terminators);
            }

            if (EquationConversion.IsReady())
            {
                equationTreeRoot = EquationConversion.MakeEquationTree(eqString);

                if(equationTreeRoot != null)
                {
                    // Convert varList into an array of IntervalStruct
                    // Compare the variable list from the equation parser to the interval list and find missing variables
                }
            }
            else
            {
                frm_Main.UpdateLog("Error: Equation parser could not be configured.");
            }

            return;
        }

        /* GETTERS */
        public static EquationStruct GetEquationStruct()
        {
            return equationTreeRoot;
        }

        public static IntervalStruct[] GetIntervalStructList()
        {
            return intervalList.ToArray();
        }
    }
}