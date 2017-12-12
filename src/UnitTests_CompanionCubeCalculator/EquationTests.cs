/*
 * Equation Conversion and Data Structure Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/12
 * ---------------------------------------------------------------------
 */

using CompanionCubeCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class EquationStructTests
    {
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException), "Error: Equation structures must be assigned an operator during initialization.")]
        public void TestEquationStructNoOperator()
        {
            EquationStruct eq = new EquationStruct("", "", null, null);
        }

        [TestMethod]
        public void TestEquationStructConstructorWithNulls()
        {
            EquationStruct eq = new EquationStruct("+", "x", null, null);
            Assert.AreEqual("+", eq.GetOperator());
            Assert.AreEqual("x", eq.GetVariableName());
            Assert.AreEqual(null, eq.GetLeftOperand());
            Assert.AreEqual(null, eq.GetRightOperand());
        }

        [TestMethod]
        public void TestEquationStructConstructor()
        {
            EquationStruct eq = new EquationStruct("+", "x", new EquationStruct("VAR", "y", null, null), new EquationStruct("VAR", "z", null, null));
            Assert.AreEqual("+", eq.GetOperator());
            Assert.AreEqual("x", eq.GetVariableName());
            Assert.AreEqual("y", eq.GetLeftOperand().GetVariableName());
            Assert.AreEqual("z", eq.GetRightOperand().GetVariableName());
        }

        [TestMethod]
        public void TestEqSetVariableName()
        {
            EquationStruct eq = new EquationStruct("+", "x", null, null);
            eq.SetVariableName("y");
            Assert.AreEqual("y", eq.GetVariableName());
        }

        [TestMethod]
        public void TestEqSetLeftOperand()
        {
            EquationStruct eq = new EquationStruct("+", "x", null, null);
            eq.SetLeftOperand(new EquationStruct("VAR", "y", null, null));
            Assert.AreEqual("y", eq.GetLeftOperand().GetVariableName());
        }

        [TestMethod]
        public void TestEqSetRightOperand()
        {
            EquationStruct eq = new EquationStruct("+", "x", null, null);
            eq.SetRightOperand(new EquationStruct("VAR", "z", null, null));
            Assert.AreEqual("z", eq.GetRightOperand().GetVariableName());
        }
    }

    [TestClass]
    public class EquationConversionTests
    {
        [TestMethod]
        public void TestConstantValueFunction()
        {

            if(!EquationConversion.IsReady())
            {
                EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators());
            }

            if(EquationConversion.IsReady())
            {
                string constToken = EquationConversion.GetConstToken();

                EquationStruct constEq = EquationConversion.MakeEquationTree("42");
                EquationStruct targetStructure = new EquationStruct(constToken, "42", null, null);

                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(constEq));
            }
            else
            {
                Assert.Fail("Equation Parser could not be initialized.");
            }
        }

        [TestMethod]
        public void TestIncompleteFunction()
        {
            if (!EquationConversion.IsReady())
            {
                EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators());
            }

            if (EquationConversion.IsReady())
            {
                // test-input missingFunctionValue1
                EquationStruct incompleteEq = EquationConversion.MakeEquationTree("x+");
                Assert.AreEqual(null, incompleteEq);

                incompleteEq = EquationConversion.MakeEquationTree("*x");
                Assert.AreEqual(null, incompleteEq);

                //  test-input missingFunctionValue2
                incompleteEq = EquationConversion.MakeEquationTree("x+*y");
                Assert.AreEqual(null, incompleteEq);

                // 
                incompleteEq = EquationConversion.MakeEquationTree("(x+y");
                Assert.AreEqual(null, incompleteEq);

                // 
                incompleteEq = EquationConversion.MakeEquationTree("x+y)");
                Assert.AreEqual(null, incompleteEq);
            }
            else
            {
                Assert.Fail("Equation Parser could not be initialized.");
            }
        }

        [TestMethod]
        public void TestVariableNames()
        {
            if (!EquationConversion.IsReady())
            {
                EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators());
            }

            if (EquationConversion.IsReady())
            {
                string varToken = EquationConversion.GetVariableToken();

                // test-input simpleVariableName
                EquationStruct varEq = EquationConversion.MakeEquationTree("x+y");
                EquationStruct targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x", null, null), new EquationStruct(varToken, "y", null, null));
                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(varEq));

                // test-input multiCharacterVariableName1
                varEq = EquationConversion.MakeEquationTree("x1+y");
                targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x1", null, null), new EquationStruct(varToken, "y", null, null));
                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(varEq));

                // test-input multiCharacterVariableName2
                varEq = EquationConversion.MakeEquationTree("x_+y");
                targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x_", null, null), new EquationStruct(varToken, "y", null, null));
                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(varEq));

                // test-input multiCharacterVariableName3
                varEq = EquationConversion.MakeEquationTree("x_1+y");
                targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x_1", null, null), new EquationStruct(varToken, "y", null, null));
                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(varEq));

                // test-input multiCharacterVariableName4
                varEq = EquationConversion.MakeEquationTree("x_i+y");
                targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x_i", null, null), new EquationStruct(varToken, "y", null, null));
                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(varEq));

                // test-input multiCharacterVariableName5
                varEq = EquationConversion.MakeEquationTree("x''+y");
                targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "x''", null, null), new EquationStruct(varToken, "y", null, null));
                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(varEq));

                // test-input multiCharacterVariableName6
                varEq = EquationConversion.MakeEquationTree("xy+y");
                targetStructure = new EquationStruct("+", "", new EquationStruct(varToken, "xy", null, null), new EquationStruct(varToken, "y", null, null));
                Assert.AreEqual(PrintEquation(targetStructure), PrintEquation(varEq));

            }
            else
            {
                Assert.Fail("Equation Parser could not be initialized.");
            }
        }

        /* HELPER FUNCTIONS */
        private static string PrintEquation(EquationStruct node)
        {
            string equation = "";

            if (node.GetLeftOperand() == null)
            {
                equation = node.GetOperator() + ": " + node.GetVariableName();
            }
            else if (node.GetRightOperand() == null)
            {
                equation = node.GetOperator() + "(" + PrintEquation(node.GetLeftOperand()) + ")";
            }
            else
            {
                equation = node.GetOperator() + "(" + PrintEquation(node.GetLeftOperand()) + ", " + PrintEquation(node.GetRightOperand()) + ")";
            }

            return equation;
        }

        private static bool CheckVariableList(string[] expected, string[] produced)
        {
            return true;
        }
    }
}
