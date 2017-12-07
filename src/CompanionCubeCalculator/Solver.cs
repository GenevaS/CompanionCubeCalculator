/*
 * Solver Module
 * ---------------------------------------------------------------------
 * Updated 2017/12/07 (Incomplete)
 * Corresponds to the Solver Module MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * Calculations performed using the Instance Models from the SRS:
 * https://github.com/GenevaS/CAS741/blob/master/Doc/SRS/SRS.pdf
 * ---------------------------------------------------------------------
 */

namespace CompanionCubeCalculator
{
    public static class Solver
    {
        private static OperatorStruct[] supportedOps = new OperatorStruct[] 
        { new OperatorStruct("+", 1, false, true, false, true),
          new OperatorStruct("-", 1, false, true, false, true),
          new OperatorStruct("*", 2, false, true, false, true),
          new OperatorStruct("/", 2, false, true, false, true),
          new OperatorStruct("^", 3, false, true, false, false)
        };

        private static string[] supportedTerminators = new string[] { "(", ")" };

        /* GETTERS */
        public static OperatorStruct[] GetValidOperators()
        {
            return supportedOps;
        }

        public static string[] GetValidTerminators()
        {
            return supportedTerminators;
        }

        /* CALCULATION METHODS */
        private static IntervalStruct IntervalAddition(IntervalStruct x, IntervalStruct y)
        {
            return new IntervalStruct("", x.GetMinBound() + y.GetMinBound(), x.GetMaxBound() + y.GetMaxBound());
        }

        private static IntervalStruct IntervalSubtraction(IntervalStruct x, IntervalStruct y)
        {
            return new IntervalStruct("", x.GetMinBound() - y.GetMinBound(), x.GetMaxBound() - y.GetMaxBound());
        }

        private static IntervalStruct IntervalMultiplication(IntervalStruct x, IntervalStruct y)
        {
            // Comparing a1 * a2 and a1 * b2
            double min = System.Math.Min(x.GetMinBound() * x.GetMaxBound(), x.GetMinBound() * y.GetMaxBound());
            double max = System.Math.Max(x.GetMinBound() * x.GetMaxBound(), x.GetMinBound() * y.GetMaxBound());

            // Comparing the previous results to b1 * a2
            min = System.Math.Min(min, y.GetMinBound() * x.GetMaxBound());
            max = System.Math.Max(max, y.GetMinBound() * x.GetMaxBound());

            // Comparing the previous results to b1 * b2
            min = System.Math.Min(min, y.GetMinBound() * y.GetMaxBound());
            max = System.Math.Max(max, y.GetMinBound() * y.GetMaxBound());

            return new IntervalStruct("", min, max);
        }

        private static IntervalStruct IntervalDivisionPositiveDivisor(IntervalStruct x, IntervalStruct y)
        {
            double min = 0;
            double max = 0;

            // 0 < a1 <= b1
            if (x.GetMinBound() > 0)
            {
                min = x.GetMinBound() / y.GetMaxBound();
                max = y.GetMinBound() / x.GetMaxBound();
            }
            // a1 = 0, a1 <= b1
            else if (x.GetMinBound() == 0)
            {
                min = 0;
                max = x.GetMaxBound() / y.GetMinBound();
            }
            // a1 < 0 < b1
            else if (x.GetMinBound() < 0 && x.GetMaxBound() > 0)
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
            // Unaccounted for case
            else
            {
                throw new System.Exception("Error: Encountered unexpected division case.");
            }

            return new IntervalStruct("", min, max);
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
            else if (x.GetMinBound() < 0 && x.GetMaxBound() > 0)
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
            // Unaccounted for case
            else
            {
                throw new System.Exception("Error: Encountered unexpected division case.");
            }

            return new IntervalStruct("", min, max);
        }
    }
}
