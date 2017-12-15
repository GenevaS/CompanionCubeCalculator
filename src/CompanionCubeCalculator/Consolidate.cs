/*
 * Variable Consolidation Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/15
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

        public static bool ConvertAndCheckInputs(string eqString, string varList, OperatorStruct[] operators, string[][] terminators, string lineDelimiter, string fieldDelimiter)
        {
            string[] equationVars;
            List<int> eqVarIndex = new List<int>();
            string[] intervalVars;
            List<int> ivVarIndex = new List<int>();
            string[] intervalVars2;
            List<int> removeIndex = new List<int>();
            int index;
            bool success = true;
            string message = "";

            EquationConversion.ResetEquationConversion();
            EquationConversion.ConfigureParser(operators, terminators);

            if (EquationConversion.IsReady())
            {
                equationTreeRoot = EquationConversion.MakeEquationTree(eqString);

                if(equationTreeRoot != null)
                {
                    intervalList = IntervalConversion.ConvertToIntervals(varList, lineDelimiter, fieldDelimiter);

                    intervalVars = GetVariableNamesFromIntervals(intervalList);
                    equationVars = EquationConversion.GetVariableList();

                    // Find matching variable names from the interval structures and the equation
                    for(int i = 0; i < equationVars.Length; i++)
                    {
                        index = Array.IndexOf(intervalVars, equationVars[i]);
                        if(index > -1)
                        {
                            ivVarIndex.Add(index);
                            eqVarIndex.Add(i);
                        }
                    }

                    // Found fewer matching entries in the equation
                    if(ivVarIndex.Count < intervalVars.Length)
                    {
                        message += "Warning: Extraneous variables found in interval list (";

                        // Sort list and reverse so that it is in descending order -> enables correct 
                        // behaviour for RemoveName(string[], int)
                        ivVarIndex.Sort();
                        ivVarIndex.Reverse();

                        foreach (int v in ivVarIndex)
                        {
                            intervalVars = RemoveName(intervalVars, v);
                        }

                        foreach(string vName in intervalVars)
                        {
                           message += vName + ", ";
                        }
                        frm_Main.UpdateLog(message.Substring(0, message.Length - 2) + ")." + Environment.NewLine);

                        // Find and remove extra intervals from the list
                        intervalVars2 = GetVariableNamesFromIntervals(intervalList);
                        for (int i = 0; i < intervalVars.Length; i++)
                        {
                            index = Array.IndexOf(intervalVars2, intervalVars[i]);
                            if (index > -1)
                            {
                                removeIndex.Add(index);
                            }
                        }

                        removeIndex.Sort();
                        removeIndex.Reverse();
                        foreach(int r in removeIndex)
                        {
                            intervalList = RemoveInterval(intervalList, r);
                        }
                    }

                    // Found fewer matching entries in the variable list
                    if (eqVarIndex.Count < equationVars.Length)
                    {
                        // This is a fail state
                        success = false;

                        message = "Error: Cannot find intervals for variable name(s): ";

                        foreach(int v in eqVarIndex)
                        {
                            equationVars = RemoveName(equationVars, v);
                        }

                        foreach (string vName in equationVars)
                        {
                            message += vName + ", ";
                        }
                        frm_Main.UpdateLog(message.Substring(0, message.Length - 2) + "." + Environment.NewLine);
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

        private static IntervalStruct[] RemoveInterval(IntervalStruct[] nameList, int index)
        {
            IntervalStruct[] newIvList = new IntervalStruct[nameList.Length - 1];
            int i = 0;
            int j = 0;

            while (i < nameList.Length)
            {
                if (i != index)
                {
                    newIvList[j] = nameList[i];
                    j++;
                }
                i++;
            }

            return newIvList;
        }
    }
}