/*
 * Interval Conversion and Data Structure Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/15
 * ---------------------------------------------------------------------
 */

using CompanionCubeCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class IntervalStructTests
    {
        [TestMethod]
        public void TestReversedBounds()
        {
            //test-parse domainOrder
            IntervalStruct interval = new IntervalStruct("x", 4.0, 3.0, true, true);
            Assert.AreEqual(3, interval.GetMinBound());
            Assert.AreEqual(4, interval.GetMaxBound());
        }

        [TestMethod]
        public void TestSetMethodsSimple()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4, false, true);

            interval.SetMinBound(2);
            interval.SetMaxBound(5);
            interval.SetLeftBoundClosed(true);
            interval.SetRightBoundClosed(false);

            Assert.AreEqual(2, interval.GetMinBound());
            Assert.AreEqual(5, interval.GetMaxBound());
            Assert.AreEqual(true, interval.IsLeftBoundClosed());
            Assert.AreEqual(false, interval.IsRightBoundClosed());
        }

        [TestMethod]
        public void TestSetMinHigherThanMax()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4, true, true);
            interval.SetMinBound(5);
            Assert.AreEqual(4, interval.GetMinBound());
            Assert.AreEqual(5, interval.GetMaxBound());
        }

        [TestMethod]
        public void TestSetMaxLowerThanMin()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4, true, true);
            interval.SetMaxBound(-1);
            Assert.AreEqual(-1, interval.GetMinBound());
            Assert.AreEqual(3, interval.GetMaxBound());
        }

        [TestMethod]
        public void TestSetVariableName()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4, true, true);
            interval.SetVariableName("y");
            Assert.AreEqual("y", interval.GetVariableName());
        }
    }

    [TestClass]
    public class IntervalConversionTests
    {
        [TestMethod]
        public void TestEmptyMinBound()
        {
            //test-input missingMinDomainValue
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,,4.0", System.Environment.NewLine, ",");
            Assert.AreEqual(4, interval[0].GetMinBound());
        }
        
        [TestMethod]
        public void TestEmptyMaxBound()
        {
            //test-input missingMaxDomainValue
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,3.0,", System.Environment.NewLine, ",");
            Assert.AreEqual(3, interval[0].GetMaxBound());
        }

        [TestMethod]
        public void TestMissingMaxBound()
        {
            //test-input missingMaxDomainValue
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,3.0", System.Environment.NewLine, ",");
            Assert.AreEqual(3, interval[0].GetMaxBound());
        }

        [TestMethod]
        public void TestMissingBounds()
        {
            //
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }

        [TestMethod]
        public void TestEmptyBounds()
        {
            //
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,,", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }

        [TestMethod]
        public void TestMissingVarName()
        {
            //
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("3.0,4.0", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }

        [TestMethod]
        public void TestEmptyVarName()
        {
            //
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals(",3.0,4.0", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }

        [TestMethod]
        public void TestNonNumericMinValue()
        {
            //test-input nonNumberInDomain
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,a,4.0", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }

        [TestMethod]
        public void TestNonNumericMaxValue()
        {
            //test-input nonNumberInDomain
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,3.0,b", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }

        [TestMethod]
        public void TestMissingWithNonNumericMinValue()
        {
            //test-input nonNumberInDomain
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,a", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }

        [TestMethod]
        public void TestTooManyFields()
        {
            IntervalStruct[] interval = IntervalConversion.ConvertToIntervals("x,3,4,5", System.Environment.NewLine, ",");
            Assert.AreEqual(0, interval.Length);
        }
    }
}
