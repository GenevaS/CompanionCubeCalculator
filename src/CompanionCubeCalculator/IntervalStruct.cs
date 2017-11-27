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
            minBound = minB;
            maxBound = maxB;

            /*
             * If the constructor has been called with minB > maxB, 
             * swap the arguements and add a warning to the program
             * log.
             */
            if (minBound > maxBound)
            {
                SwapBounds();
                frm_Main.UpdateLog("WARNING: Value provided for intervals are not in increasing order.The values have been exchanged to maintain the interval ordering." + System.Environment.NewLine);
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
            minBound = minB;

            /*
             * If the new minimum bound is larger than the current
             * maximum bound, swap the values
             */
            if (minBound > maxBound)
            {
                SwapBounds();
                frm_Main.UpdateLog("WARNING: Value provided for minimum bound is greater than the current maximum bound. The values have been exchanged to maintain the interval ordering." + System.Environment.NewLine);
            }
            return;
        }

        public void SetMaxBound(double maxB)
        {
            maxBound = maxB;

            /*
            * If the new maximum bound is larger than the current
            * maximum bound, swap the values
            */
            if (maxBound < minBound)
            {
                SwapBounds();
                frm_Main.UpdateLog("WARNING: Value provided for maximum bound is smaller than the current minimum bound. The values have been exchanged to maintain the interval ordering." + System.Environment.NewLine);
            }

            return;
        }

        /*
         * Value swapping function for maintaining the 
         * order of the interval bounds 
         */
        private void SwapBounds()
        {
            double temp = minBound;
            minBound = maxBound;
            maxBound = temp;

            return;
        }
    }
}