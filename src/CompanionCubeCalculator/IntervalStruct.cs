/*
 * Interval Data Structure
 * ---------------------------------------------------------------------
 * Updated 2017/11/27
 * Corresponds to IntervalStruct MIS from
 * https://github.com/GenevaS/CAS741/blob/master/Doc/Design/MIS/MIS.pdf
 * ---------------------------------------------------------------------
 */

namespace CompanionCubeCalculator
{
    internal class IntervalStruct
    {
        private double minBound;
        private double maxBound;

        /* CONSTRUCTOR */
        public IntervalStruct(double minB, double maxB)
        {
            /*
             * If the constructor has been called with minB > maxB, 
             * swap the arguements and add a warning to the program
             * log.
             */
            if (minB <= maxB)
            {
                minBound = minB;
                maxBound = maxB;
            }
            else
            {
                frm_Main.UpdateLog("WARNING: Value provided for intervals are not in increasing order.The values have been exchanged to maintain the interval ordering." + System.Environment.NewLine);
                minBound = maxB;
                maxBound = minB;
            }

            return;
        }

        /* GETTERS */
        public double GetMinBound()
        {
            return minBound;
        }

        public double GetMaxBound()
        {
            return maxBound;
        }

        /* SETTERS */
        public void SetMinBound(double minB)
        {
            return;
        }

        public void SetMaxBound(double maxB)
        {
            return;
        }
    }
}