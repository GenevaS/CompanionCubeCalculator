/*
 * Output Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/14
 * Corresponds to Output Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

namespace CompanionCubeCalculator
{
    public class Output
    {
        public static string PrintInterval(IntervalStruct interval, bool withVarName)
        {
            string iv = "";

            if(interval.GetMinBound() == interval.GetMaxBound())
            {
                iv = "CONST: " + interval.GetMinBound();
            }
            else
            {
                if (withVarName)
                {
                    iv += interval.GetVariableName() + " = ";
                }
                
                if(interval.IsLeftBoundClosed())
                {
                    iv += "[";
                }
                else
                {
                    iv += "(";
                }

                if(interval.GetMinBound().ToString().Length > 12)
                {
                    iv += interval.GetMinBound() + ", " + System.Environment.NewLine + " " + interval.GetMaxBound();
                }
                else
                {
                    iv += interval.GetMinBound() + ", " + interval.GetMaxBound();
                }
                

                if(interval.IsRightBoundClosed())
                {
                    iv += "]";
                }
                else
                {
                    iv += ")";
                }
            }

            return iv;
        }

        public static string PrintEquationTree(EquationStruct eqRoot)
        {
            return PrintNodes(eqRoot, "", true);
        }

        private static string PrintNodes(EquationStruct node, string indentation, bool isRightOperand)
        {
            string eq = indentation + "+- {" + node.GetOperator() + "}";

            if(node.GetLeftOperand() == null && node.GetRightOperand() == null)
            {
                eq += " " + node.GetVariableName();
            }

            if (isRightOperand && node.GetLeftOperand() == null && node.GetRightOperand() == null)
            {
                indentation += "     ";
            }
            else
            {
                indentation += "|   ";
            }

            eq += System.Environment.NewLine;

            if (node.GetLeftOperand() != null)
            {
                eq += PrintNodes(node.GetLeftOperand(), indentation, false);
            }

            if (node.GetRightOperand() != null)
            {
                eq += PrintNodes(node.GetRightOperand(), indentation, true);
            }

            return eq;
        }
    }
}