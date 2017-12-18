/*
 * Control Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/18
 * ---------------------------------------------------------------------
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CompanionCubeCalculator;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class ControlTests
    {
        [TestMethod]
        public void TestInitialize()
        {
            bool success = ControlFlow.Initialize();
            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void TestConditionFunction()
        {
            string conditioned = ControlFlow.ConditionRawInput("x + y", true);
            Assert.AreEqual("x+y", conditioned);
        }

        [TestMethod]
        public void TestControlFile()
        {
            string[] delimiters = ControlFlow.GetDelimiters();
            string[] fileContents = ControlFlow.ControlFile(@"TestFiles/test.txt");
            Assert.AreEqual(2, fileContents.Length);
            Assert.AreEqual("x+y", fileContents[0]);
            Assert.AreEqual("x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] +  "5", fileContents[1]);
        }
    }
}
