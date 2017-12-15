/*
 * Solver Module
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/14
 * Corresponds to the Solver Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * 
 * Calculations performed using the Instance Models from the SRS:
 * https://github.com/GenevaS/CAS741/blob/master/Doc/SRS/SRS.pdf
 * ---------------------------------------------------------------------
 */

using System.Collections.Generic;

namespace CompanionCubeCalculator
{
    public static class Solver
    {
        private static readonly OperatorStruct[] supportedOps = new OperatorStruct[] 
        { new OperatorStruct("+", 1, false, true, false, true),
          new OperatorStruct("-", 1, false, true, false, true),
          new OperatorStruct("*", 2, false, true, false, true),
          new OperatorStruct("/", 2, false, true, false, true),
          new OperatorStruct("^", 3, false, true, false, false)
        };

        private static readonly string[][] supportedTerminators = new string[][] { new string[] { "(", ")" } };

        /* GETTERS */
        public static OperatorStruct[] GetValidOperators()
        {
            return supportedOps;
        }

        public static string[][] GetValidTerminators()
        {
            return supportedTerminators;
        }

        /* CALCULATION */
        public static IntervalStruct FindRange(EquationStruct eqRoot, IntervalStruct[] intervals)
        {
            string[] varNames = GetVariableNamesFromIntervals(intervals);

            return CalculateRange(eqRoot, intervals, varNames);
        }

        private static IntervalStruct CalculateRange(EquationStruct eqTree, IntervalStruct[] intervals, string[] intervalNames)
        {
            IntervalStruct range = null;
            IntervalStruct leftResult = null;
            IntervalStruct rightResult = null;
            double constant = 0;
            int varIndex = -1;

            // This is either a variable or a constant
            if(eqTree.GetLeftOperand() == null && eqTree.GetRightOperand() == null)
            {
                try
                {
                    constant = System.Convert.ToDouble(eqTree.GetVariableName());
                    range = new IntervalStruct("", constant, constant, true, true);
                }
                catch(System.FormatException)
                {
                    varIndex = System.Array.IndexOf(intervalNames, eqTree.GetVariableName());
                    if (varIndex > -1)
                    {
                        range = intervals[varIndex];
                    }
                    else
                    {
                        frm_Main.UpdateLog("Error: Could not find an associated interval for variable " + eqTree.GetVariableName() + System.Environment.NewLine);
                    }
                    
                }
            }
            // This is an equation, so calculate the result
            else if(eqTree.GetOperator() == "+")
            {
                leftResult = CalculateRange(eqTree.GetLeftOperand(), intervals, intervalNames);
                rightResult = CalculateRange(eqTree.GetRightOperand(), intervals, intervalNames);
                if(leftResult != null && rightResult != null)
                {
                    range = IntervalAddition(leftResult, rightResult);
                }
            }
            else if (eqTree.GetOperator() == "-")
            {
                leftResult = CalculateRange(eqTree.GetLeftOperand(), intervals, intervalNames);
                rightResult = CalculateRange(eqTree.GetRightOperand(), intervals, intervalNames);
                if (leftResult != null && rightResult != null)
                {
                    range = IntervalSubtraction(leftResult, rightResult);
                }
            }
            else if (eqTree.GetOperator() == "*")
            {
                leftResult = CalculateRange(eqTree.GetLeftOperand(), intervals, intervalNames);
                rightResult = CalculateRange(eqTree.GetRightOperand(), intervals, intervalNames);
                if (leftResult != null && rightResult != null)
                {
                    range = IntervalMultiplication(leftResult, rightResult);
                }
            }
            else if (eqTree.GetOperator() == "/")
            {
                leftResult = CalculateRange(eqTree.GetLeftOperand(), intervals, intervalNames);
                rightResult = CalculateRange(eqTree.GetRightOperand(), intervals, intervalNames);
                if (leftResult != null && rightResult != null)
                {
                    range = IntervalDivision(leftResult, rightResult);
                }
            }
            else if (eqTree.GetOperator() == "^")
            {
                leftResult = CalculateRange(eqTree.GetLeftOperand(), intervals, intervalNames);
                rightResult = CalculateRange(eqTree.GetRightOperand(), intervals, intervalNames);
                if (leftResult != null && rightResult != null)
                {
                    range = IntervalExponents(leftResult, rightResult);
                }
            }
            else if (eqTree.GetOperator() == "()")
            {
                range = CalculateRange(eqTree.GetLeftOperand(), intervals, intervalNames);
            }
            else
            {
                frm_Main.UpdateLog("Error: An unsupported operation was encountered while solving for the range of the equation (Unknown operator)." + System.Environment.NewLine);
            }

            return range;
        }

        /* CALCULATION -- ADDITION */
        private static IntervalStruct IntervalAddition(IntervalStruct x, IntervalStruct y)
        {
            return new IntervalStruct("", x.GetMinBound() + y.GetMinBound(), x.GetMaxBound() + y.GetMaxBound(), true, true);
        }

        /* CALCULATION -- SUBTRACTION */
        private static IntervalStruct IntervalSubtraction(IntervalStruct x, IntervalStruct y)
        {
            return new IntervalStruct("", x.GetMinBound() - y.GetMinBound(), x.GetMaxBound() - y.GetMaxBound(), true, true);
        }

        /* CALCULATION -- MULTIPLICATION */
        private static IntervalStruct IntervalMultiplication(IntervalStruct x, IntervalStruct y)
        {
            // Comparing a1 * a2 and a1 * b2
            double min = System.Math.Min(x.GetMinBound() * y.GetMinBound(), x.GetMinBound() * y.GetMaxBound());
            double max = System.Math.Max(x.GetMinBound() * y.GetMinBound(), x.GetMinBound() * y.GetMaxBound());

            // Comparing the previous results to b1 * a2
            min = System.Math.Min(min, x.GetMaxBound() * y.GetMinBound());
            max = System.Math.Max(max, x.GetMaxBound() * y.GetMinBound());

            // Comparing the previous results to b1 * b2
            min = System.Math.Min(min, x.GetMaxBound() * y.GetMaxBound());
            max = System.Math.Max(max, x.GetMaxBound() * y.GetMaxBound());

            return new IntervalStruct("", min, max, true, true);
        }

        /* CALCULATION -- DIVISION */
        private static IntervalStruct IntervalDivision(IntervalStruct x, IntervalStruct y)
        {
            IntervalStruct divInterval = null;

            // 0 < a2 <= b2
            if (y.GetMinBound() > 0)
            {
                divInterval = IntervalDivisionPositiveDivisor(x, y);
            }
            // a2 <= b2 < 0
            else if (y.GetMaxBound() < 0)
            {
                divInterval = IntervalDivisionNegativeDivisor(x, y);
            }
            // a2 = 0 v b2 = 0
            else
            {
                frm_Main.UpdateLog("Error: An unsupported operation was encountered while solving for the range of the equation (Mixed interval division)." + System.Environment.NewLine);
            }

            return divInterval;
        }

        private static IntervalStruct IntervalDivisionPositiveDivisor(IntervalStruct x, IntervalStruct y)
        {
            double min = 0;
            double max = 0;

            // 0 < a1 <= b1
            if (x.GetMinBound() > 0)
            {
                min = x.GetMinBound() / y.GetMaxBound();
                max = x.GetMaxBound() / y.GetMinBound();
            }
            // a1 = 0, a1 <= b1
            else if (x.GetMinBound() == 0)
            {
                min = 0;
                max = x.GetMaxBound() / y.GetMinBound();
            }
            // a1 < 0 < b1
            else if (x.GetMaxBound() > 0)
            {
                min = x.GetMinBound() / y.GetMinBound();
                max = x.GetMaxBound() / y.GetMinBound();
            }
            // b1 = 0, a1 <= b1
            else if (x.GetMaxBound() == 0)
            {
                min = x.GetMinBound() / y.GetMinBound();
                max = 0;
            }
            // a1 <= b1 < 0
            else if (x.GetMaxBound() < 0)
            {
                min = x.GetMinBound() / y.GetMinBound();
                max = x.GetMaxBound() / y.GetMaxBound();
            }

            return new IntervalStruct("", min, max, true, true);
        }

        private static IntervalStruct IntervalDivisionNegativeDivisor(IntervalStruct x, IntervalStruct y)
        {
            double min = 0;
            double max = 0;

            // 0 < a1 <= b1
            if (x.GetMinBound() > 0)
            {
                min = x.GetMaxBound() / y.GetMaxBound();
                max = x.GetMinBound() / y.GetMinBound();
            }
            // a1 = 0, a1 <= b1
            else if (x.GetMinBound() == 0)
            {
                min = x.GetMaxBound() / y.GetMaxBound();
                max = 0;
            }
            // a1 < 0 < b1
            else if (x.GetMaxBound() > 0)
            {
                min = x.GetMaxBound() / y.GetMaxBound();
                max = x.GetMinBound() / y.GetMaxBound();
            }
            // b1 = 0, a1 <= b1
            else if (x.GetMaxBound() == 0)
            {
                min = 0;
                max = x.GetMinBound() / y.GetMaxBound();
            }
            //a1 <= b1 < 0
            else if (x.GetMaxBound() < 0)
            {
                min = x.GetMaxBound() / y.GetMinBound();
                max = x.GetMinBound() / y.GetMaxBound();
            }

            return new IntervalStruct("", min, max, true, true);
        }

        /* CALCULATION -- EXPONENTS */
        private static IntervalStruct IntervalExponents(IntervalStruct x, IntervalStruct y)
        {
            IntervalStruct exp = null;

            if (x.GetMinBound() == x.GetMaxBound())
            {
                exp = IntervalAsExponent(x.GetMinBound(), y);
            }
            else if (y.GetMinBound() == y.GetMaxBound())
            {
                exp = IntervalAsBase(x, y.GetMinBound());
            }
            else
            {
                frm_Main.UpdateLog("Error: An unsupported operation was encountered while solving for the range of the equation (Exponents)." + System.Environment.NewLine);
            }

            return exp;
        }

        private static IntervalStruct IntervalAsExponent(double b, IntervalStruct x)
        {
            IntervalStruct exp = null;

            if(b > 1)
            {
                exp = new IntervalStruct("", System.Math.Pow(b, x.GetMinBound()), System.Math.Pow(b, x.GetMaxBound()), true, true);
            }
            else
            {
                frm_Main.UpdateLog("Error: An unsupported operation was encountered while solving for the range of the equation (Exponent base <= 1)." + System.Environment.NewLine);
            }

            return exp;
        }

        private static IntervalStruct IntervalAsBase(IntervalStruct x, double n)
        {
            IntervalStruct exp = null;
            double roundedN = System.Math.Round(n);

            if(n >= 0)
            {
                if (n != roundedN)
                {
                    frm_Main.UpdateLog("Warning: The value provided for the exponent" + System.Convert.ToString(n) + "is not a natural number. It has been rounded to " + System.Convert.ToString(roundedN) + System.Environment.NewLine);
                }

                if (roundedN % 2 != 0)
                {
                    exp = new IntervalStruct("", System.Math.Pow(x.GetMinBound(), roundedN), System.Math.Pow(x.GetMaxBound(), roundedN), true, true);
                }
                else if (x.GetMinBound() >= 0)
                {
                    exp = new IntervalStruct("", System.Math.Pow(x.GetMinBound(), roundedN), System.Math.Pow(x.GetMaxBound(), roundedN), true, true);
                }
                else if (x.GetMaxBound() < 0)
                {
                    exp = new IntervalStruct("", System.Math.Pow(x.GetMaxBound(), roundedN), System.Math.Pow(x.GetMinBound(), roundedN), true, true);
                }
                else
                {
                    exp = new IntervalStruct("", 0, System.Math.Max(System.Math.Pow(x.GetMinBound(), roundedN), System.Math.Pow(x.GetMaxBound(), roundedN)), true, true);
                }
            }
            else
            {
                frm_Main.UpdateLog("Error: An unsupported operation was encountered while solving for the range of the equation (Exponent < 0)." + System.Environment.NewLine);
            }


            return exp;
        }

        /* HELPER FUNCTIONS */
        private static string[] GetVariableNamesFromIntervals(IntervalStruct[] intervals)
        {
            List<string> names = new List<string>();
            if (intervals != null)
            {
                foreach (IntervalStruct iv in intervals)
                {
                    names.Add(iv.GetVariableName());
                }
            }
            
            return names.ToArray();
        }
    } 
}