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
        private static IntervalStruct[] intervalList;

        public static bool ConvertAndCheckInputs(string eqString, string varList, OperatorStruct[] operators, string[][] terminators)
        {
            string[] equationVars;
            string[] intervalVars;
            int index;
            bool success = true;

            if (!EquationConversion.IsReady())
            {
                EquationConversion.ConfigureParser(operators, terminators);
            }

            if (EquationConversion.IsReady())
            {
                equationTreeRoot = EquationConversion.MakeEquationTree(eqString);

                if(equationTreeRoot != null)
                {
                    intervalList = IntervalConversion.ConvertToIntervals(varList);

                    intervalVars = GetVariableNamesFromIntervals(intervalList);
                    equationVars = EquationConversion.GetVariableList();

                    for(int i = 0; i < equationVars.Length; i++)
                    {
                        index = Array.IndexOf(intervalVars, equationVars[i]);
                        if(index > -1)
                        {
                            intervalVars = RemoveName(intervalVars, index);
                            equationVars = RemoveName(equationVars, i);
                        }
                    }

                    if(intervalVars.Length > 0)
                    {
                        frm_Main.UpdateLog("Warning: Extraneous variables found in interval list (");
                        foreach (string v in intervalVars)
                        {
                            frm_Main.UpdateLog(v + ",");
                        }
                        frm_Main.UpdateLog(")." + Environment.NewLine);
                    }

                    if(equationVars.Length > 0)
                    {
                        frm_Main.UpdateLog("Error: Cannot find intervals for variables ");
                        foreach (string v in equationVars)
                        {
                            frm_Main.UpdateLog(v + ",");
                        }
                        frm_Main.UpdateLog("." + Environment.NewLine);
                        success = false;
                    }

                }
            }
            else
            {
                frm_Main.UpdateLog("Error: Equation parser could not be configured.");
                success = false;
            }

            return success;
        }

        /* GETTERS */
        public static EquationStruct GetEquationStruct()
        {
            return equationTreeRoot;
        }

        public static IntervalStruct[] GetIntervalStructList()
        {
            return intervalList;
        }

        /* HELPER FUNCTIONS */
        private static string[] GetVariableNamesFromIntervals(IntervalStruct[] intervals)
        {
            List<string> names = new List<string>();

            foreach (IntervalStruct iv in intervals)
            {
                names.Add(iv.GetVariableName());
            }

            return names.ToArray();
        }

        private static string[] RemoveName(string[] nameList, int index)
        {
            string[] newNameList = new string[nameList.Length - 1];
            int i = 0;
            int j = 0;

            while(i < nameList.Length)
            {
                if(i != index)
                {
                    newNameList[j] = nameList[i];
                    j++;
                }
                i++;
            }

            return newNameList;
        }
    }
}