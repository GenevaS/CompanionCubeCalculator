/*
 * Equation Conversion and Data Structure Tests
 * ---------------------------------------------------------------------
 * Author: Geneva Smith (GenevaS)
 * Updated 2017/12/10
 * ---------------------------------------------------------------------
 */

using CompanionCubeCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests_CompanionCubeCalculator
{
    [TestClass]
    public class EquationTests
    {
        [TestMethod]
        public void TestConstantValueFunction()
        {

            if(EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators()))
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
            if (EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators()))
            {
                // test-input missingFunctionValue1
                EquationStruct incompleteEq = EquationConversion.MakeEquationTree("x+");
                Assert.AreEqual(null, incompleteEq);

                incompleteEq = EquationConversion.MakeEquationTree("*x");
                Assert.AreEqual(null, incompleteEq);

                //  test-input missingFunctionValue2
                incompleteEq = EquationConversion.MakeEquationTree("x+*y");
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
            if (EquationConversion.ConfigureParser(Solver.GetValidOperators(), Solver.GetValidTerminators()))
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
