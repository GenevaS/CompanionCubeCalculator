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
            // unittest-controlinit
            bool success = ControlFlow.Initialize();
            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void TestConditionFunction()
        {
            // unittest-controlcondition
            string conditioned = ControlFlow.ConditionRawInput("x + y", true);
            Assert.AreEqual("x+y", conditioned);
        }

        [TestMethod]
        public void TestGetFileTypes()
        {
            // unittest-controlgetfiletypes
            string[] fileTypes = ControlFlow.GetValidFileTypes();
            string expectedTypes = "*.txt";
            Assert.AreEqual(expectedTypes, fileTypes[0]);
        }

        [TestMethod]
        public void TestExtractVars()
        {
            EquationConversion.ResetEquationConversion();
            if (ControlFlow.Initialize())
            {
                // unittest-controlextractvars
                string[] vars = ControlFlow.ExtractVariables("x+y");
                Assert.AreEqual("x", vars[0]);
                Assert.AreEqual("y", vars[1]);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestGetVariableInfo()
        {
            if (ControlFlow.Initialize())
            {
                // unittest-controlgetvarinfo
                string[] delimiters = ControlFlow.GetDelimiters();
                string[] results = ControlFlow.ControlDirect("x+y", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5");
                string targetRange = "[5, 9]";

                Assert.AreEqual(targetRange, results[0]);

                string[,] varInfo = ControlFlow.GetVariableInfo();

                Assert.AreEqual(6, varInfo.Length);

                Assert.AreEqual("x", varInfo[0, 0]);
                Assert.AreEqual("2", varInfo[0, 1]);
                Assert.AreEqual("4", varInfo[0, 2]);

                Assert.AreEqual("y", varInfo[1, 0]);
                Assert.AreEqual("3", varInfo[1, 1]);
                Assert.AreEqual("5", varInfo[1, 2]);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestControlFile()
        {
            string[] delimiters = ControlFlow.GetDelimiters();

            // unittest-controlfile
            string[] fileContents = ControlFlow.ControlFile(@"TestFiles/test.txt");
            Assert.AreEqual(2, fileContents.Length);
            Assert.AreEqual("x+y", fileContents[0]);
            Assert.AreEqual("x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] +  "5", fileContents[1]);
        }

        [TestMethod]
        public void TestDirectInputs()
        {
            // test-directinput
            if (ControlFlow.Initialize())
            {
                string[] delimiters = ControlFlow.GetDelimiters();
                string[] results = ControlFlow.ControlDirect("x+y", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5");
                string targetRange = "[5, 9]";
                string targetEquationTree = "";
                targetEquationTree += "+- {+}" + System.Environment.NewLine;
                targetEquationTree += "|   +- {VAR} x" + System.Environment.NewLine;
                targetEquationTree += "|   +- {VAR} y" + System.Environment.NewLine;

                Assert.AreEqual(targetRange, results[0]);
                Assert.AreEqual(System.Text.RegularExpressions.Regex.Replace(targetEquationTree, @"\s+", ""), System.Text.RegularExpressions.Regex.Replace(results[1], @"\s+", ""));
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestNoEquation()
        {
            // test-input\_noFunction
            if (ControlFlow.Initialize())
            {
                string[] delimiters = ControlFlow.GetDelimiters();
                string[] results = ControlFlow.ControlDirect("", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5");
              
                Assert.AreEqual(-3, ControlFlow.GetSuccessCode());
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void PrecedenceTests()
        {
            if (ControlFlow.Initialize())
            {
                string[] delimiters = ControlFlow.GetDelimiters();

                // test-control\_commutativityOfAdditionSubstraction
                string[] results1 = ControlFlow.ControlDirect("x+y-z", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5" + delimiters[0] + "z" + delimiters[1] + "2" + delimiters[1] + "2");
                string[] results2 = ControlFlow.ControlDirect("(x+y)-z", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5" + delimiters[0] + "z" + delimiters[1] + "2" + delimiters[1] + "2");

                Assert.AreEqual(results1[0], results2[0]);

                // test-control\_commutativityOfMultiplicationDivision
                results1 = ControlFlow.ControlDirect("x*y/z", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5" + delimiters[0] + "z" + delimiters[1] + "2" + delimiters[1] + "2");
                results2 = ControlFlow.ControlDirect("(x*y)/z", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5" + delimiters[0] + "z" + delimiters[1] + "2" + delimiters[1] + "2");

                Assert.AreEqual(results1[0], results2[0]);

                // test-control\_precedenceOfOperators1
                results1 = ControlFlow.ControlDirect("x+y*z", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5" + delimiters[0] + "z" + delimiters[1] + "2" + delimiters[1] + "2");
                results2 = ControlFlow.ControlDirect("x+(y*z)", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5" + delimiters[0] + "z" + delimiters[1] + "2" + delimiters[1] + "2");

                Assert.AreEqual(results1[0], results2[0]);

                // test-control\_precedenceOfOperators2
                results1 = ControlFlow.ControlDirect("2^x*y", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5");
                results2 = ControlFlow.ControlDirect("(2^x)*y", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5");

                Assert.AreEqual(results1[0], results2[0]);

                // test-control\_precedenceOfOperators4
                results1 = ControlFlow.ControlDirect("x*(y+z)", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5" + delimiters[0] + "z" + delimiters[1] + "2" + delimiters[1] + "2");
                string expected = "[10, 28]";
                Assert.AreEqual(expected, results1[0]);

                // test-control\_precedenceOfOperators5
                results1 = ControlFlow.ControlDirect("2(x+y)^2/3^z", "x" + delimiters[1] + "1" + delimiters[1] + "2" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "4" + delimiters[0] + "z" + delimiters[1] + "5" + delimiters[1] + "6");
                expected = "[0.0438957475994513, " + System.Environment.NewLine + " 0.296296296296296]";
                Assert.AreEqual(expected, results1[0]);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestStartingBrackets()
        {
            if (ControlFlow.Initialize())
            {
                string[] delimiters = ControlFlow.GetDelimiters();

                // test-control\_precedenceOfOperators3
                // Failing with: Error: Unrecognized sequence encountered during Atomic Equation parsing. Remaining equation = )*y
                string[] results1 = ControlFlow.ControlDirect("x^2*y", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5");
                string[] results2 = ControlFlow.ControlDirect("(x^2)*y", "x" + delimiters[1] + "2" + delimiters[1] + "4" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "5");

                Assert.AreEqual(results1[0], results2[0]);

                // test-control\_precedenceOfOperators6
                // Failing with: Error: Unrecognized sequence encountered during Atomic Equation parsing. Remaining equation = )/(3^z)
                results1 = ControlFlow.ControlDirect("(2(x+y)^2)/(3^z)", "x" + delimiters[1] + "1" + delimiters[1] + "2" + delimiters[0] + "y" + delimiters[1] + "3" + delimiters[1] + "4" + delimiters[0] + "z" + delimiters[1] + "5" + delimiters[1] + "6");
                string expected = "[0.0438957475994513, " + System.Environment.NewLine + " 0.296296296296296]";
                Assert.AreEqual(expected, results1[0]);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
