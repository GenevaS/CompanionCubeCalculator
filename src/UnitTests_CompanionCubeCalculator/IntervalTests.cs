/*
 * Interval Conversion and Data Structure Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/07
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
            IntervalStruct interval = new IntervalStruct("x", 4.0, 3.0);
            Assert.AreEqual(interval.GetMinBound(), 3);
            Assert.AreEqual(interval.GetMaxBound(), 4);
        }

        [TestMethod]
        public void TestSetMethodsSimple()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4);
            interval.SetMinBound(2);
            interval.SetMaxBound(5);
            Assert.AreEqual(interval.GetMinBound(), 2);
            Assert.AreEqual(interval.GetMaxBound(), 5);
        }

        [TestMethod]
        public void TestSetMinHigherThanMax()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4);
            interval.SetMinBound(5);
            Assert.AreEqual(interval.GetMinBound(), 4);
            Assert.AreEqual(interval.GetMaxBound(), 5);
        }

        [TestMethod]
        public void TestSetMaxLowerThanMin()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4);
            interval.SetMaxBound(-1);
            Assert.AreEqual(interval.GetMinBound(), -1);
            Assert.AreEqual(interval.GetMaxBound(), 3);
        }

        [TestMethod]
        public void TestSetVariableName()
        {
            //
            IntervalStruct interval = new IntervalStruct("x", 3, 4);
            interval.SetVariableName("y");
            Assert.AreEqual(interval.GetVariableName(), "y");
        }
    }

    [TestClass]
    public class IntervalConversionTests
    {
        [TestMethod]
        public void TestEmptyMinBound()
        {
            //test-input missingMinDomainValue
            IntervalStruct interval = IntervalConversion.MakeInterval("x", "", "4.0");
            Assert.AreEqual(interval.GetMinBound(), 4);
        }

        [TestMethod]
        public void TestEmptyMaxBound()
        {
            //test-input missingMaxDomainValue
            IntervalStruct interval = IntervalConversion.MakeInterval("x", "3.0", "");
            Assert.AreEqual(interval.GetMaxBound(), 3);
        }

        [TestMethod]
        public void TestEmptyBounds()
        {
            //
            IntervalStruct interval = IntervalConversion.MakeInterval("x", "", "");
            Assert.AreEqual(interval, null);
        }

        [TestMethod]
        public void TestEmptyVarName()
        {
            //
            IntervalStruct interval = IntervalConversion.MakeInterval("", "3.0", "4.0");
            Assert.AreEqual(interval, null);
        }

        [TestMethod]
        public void TestNonNumericMinValue()
        {
            //test-input nonNumberInDomain
            IntervalStruct interval = IntervalConversion.MakeInterval("x", "a", "4.0");
            Assert.AreEqual(interval, null);
        }

        [TestMethod]
        public void TestNonNumericMaxValue()
        {
            //test-input nonNumberInDomain
            IntervalStruct interval = IntervalConversion.MakeInterval("x", "3.0", "b");
            Assert.AreEqual(interval, null);
        }
    }
}
