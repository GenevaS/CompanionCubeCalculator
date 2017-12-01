
using System.Collections.Generic;

namespace CompanionCubeCalculator
{
    public static class EquationConversion
    {
        private static List<string> variableList = new List<string>();
        public static EquationStruct MakeEquationTree(string userEquation, string[] supportedOperations)
        {
            EquationStruct eqRoot = null;
            string[] components;
            double constant;

            try
            {
                constant = System.Convert.ToDouble(userEquation);
                eqRoot = new EquationStruct("Const", "var" + userEquation, null, null);
                variableList.Add("var" + userEquation);

                frm_Main.UpdateLog("Warning: The user equation is a constant value and the range will only include this value.");
            }
            catch(System.FormatException)
            {
                for (int i = supportedOperations.Length - 1; i >= 0; i--)
                {
                    components = userEquation.Split(supportedOperations[i].ToCharArray()[0]);
                    for (int j = 0; j < components.Length; j++)
                    {
                        frm_Main.UpdateLog(components[j] + System.Environment.NewLine);
                    }
                }
            }

            return eqRoot;
        }

        private static EquationStruct[] MakeEquationTreeNodes(string[] eqComponents, string[] suprtOps)
        {
            List<EquationStruct> nodes = new List<EquationStruct>();
            EquationStruct leftOpnd = null;
            EquationStruct rightOpnd = null;
            string[] cmpts;

            // The equation has been split by all known operators and the atomic 
            // equation values have been reached
            if (suprtOps.Length == 0)
            {

            }
            else
            {
                foreach (string eqC in eqComponents)
                {
                    cmpts = eqC.Split(suprtOps[suprtOps.Length - 1].ToCharArray()[0]);
                }
            }

            return nodes.ToArray();
        }

        /* GETTERS */
        public static string[] GetVariableList()
        {
            return variableList.ToArray();
        }
    }
}
